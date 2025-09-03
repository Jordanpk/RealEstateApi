using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using RealEstate.Application.DTOs;
using RealEstate.Application.Mappings;
using RealEstate.Application.Services;
using RealEstate.Domain.Repositories;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Tests.TestUtils;

namespace RealEstate.Tests.Unit
{
    public class PropertyServiceDbTests
    {
        private IMapper _mapper = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            _mapper = cfg.CreateMapper();
        }

        [Test]
        public async Task CreateAsync_deberia_fallar_si_owner_no_existe()
        {
            using var scope = new DbTestScope();

            // repos reales (con la misma BD):
            IPropertyRepository propRepo = new PropertyRepository(scope.Db);
            IUnitOfWork uow = new UnitOfWork(scope.Db);
            IOwnerRepository ownerRepo = new OwnerRepository(scope.Db);

            var sut = new PropertyService(propRepo, uow, _mapper, ownerRepo);

            var dto = new PropertyCreateDto
            {
                Name = "Casa X",
                Address = "Calle 1",
                Price = 1000m,
                CodeInternal = "UT-OWNER-NO-EXISTE",
                Year = 2020,
                OwnerId = 999999 
            };

            var act = async () => await sut.CreateAsync(dto, CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*no existe*");
            // Se hace rollback al salir del using
        }

        [Test]
        public async Task CreateAsync_deberia_crear_si_owner_existe()
        {
            using var scope = new DbTestScope();

            IPropertyRepository propRepo = new PropertyRepository(scope.Db);
            IUnitOfWork uow = new UnitOfWork(scope.Db);
            IOwnerRepository ownerRepo = new OwnerRepository(scope.Db);

            var sut = new PropertyService(propRepo, uow, _mapper, ownerRepo);

            var dto = new PropertyCreateDto
            {
                Name = "Casa Test",
                Address = "Calle 123, Bogotá",
                Price = 123000000m,
                CodeInternal = "UT-CREATE-OK-001",
                Year = 2021,
                OwnerId = 1 
            };

            var result = await sut.CreateAsync(dto, CancellationToken.None);

            result.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("Casa Test");
            // Rollback al Dispose()
        }

        [Test]
        public async Task ListAsync_deberia_filtrar_por_ciudad_y_anio_exactos()
        {
            using var scope = new DbTestScope();

            IPropertyRepository propRepo = new PropertyRepository(scope.Db);
            IUnitOfWork uow = new UnitOfWork(scope.Db);
            IOwnerRepository ownerRepo = new OwnerRepository(scope.Db);

            var sut = new PropertyService(propRepo, uow, _mapper, ownerRepo);

            var (items, total) = await sut.ListAsync(
                new PropertyFilterDto { City = "Cartagena", Year = 2013 },
                CancellationToken.None);

            total.Should().Be(1);
            items.Should().ContainSingle();
            items[0].Address.Should().Contain("Cartagena");
            items[0].Year.Should().Be(2013);
        }
    }
}
