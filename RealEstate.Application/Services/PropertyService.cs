using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepo;

        public PropertyService(IPropertyRepository repo, IUnitOfWork uow, IMapper mapper, IOwnerRepository owner)
        {
            _repo = repo; _uow = uow; _mapper = mapper; _ownerRepo = owner;
        }

        public async Task<PropertyDto> CreateAsync(PropertyCreateDto dto, CancellationToken ct)
        {
            // Validar si el dueño existe
            var ownerExists = await _ownerRepo.ExistsAsync(dto.OwnerId, ct);
            if (!ownerExists)
                throw new KeyNotFoundException($"El dueño con Id={dto.OwnerId} no existe");

            var entity = _mapper.Map<Property>(dto);
            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return _mapper.Map<PropertyDto>(entity);
        }


        public async Task<PropertyDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return entity == null ? null : _mapper.Map<PropertyDto>(entity);
        }


        public async Task AddImageAsync(int propertyId, PropertyImageCreateDto dto, CancellationToken ct)
        {
            var img = _mapper.Map<PropertyImage>(dto);
            img.PropertyId = propertyId;
            await _repo.AddImageAsync(img, ct);
            await _uow.SaveChangesAsync(ct);
        }


        public async Task ChangePriceAsync(int propertyId, decimal newPrice, CancellationToken ct)
        {
            await _repo.ChangePriceAsync(propertyId, newPrice, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(int propertyId, PropertyUpdateDto dto, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(propertyId, ct) ?? throw new KeyNotFoundException("Property not found");
            _mapper.Map(dto, p);
            p.UpdatedAt = System.DateTime.UtcNow;
            await _repo.UpdateAsync(p, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task<(IReadOnlyList<PropertyDto> Items, int Total)> ListAsync(PropertyFilterDto filter, CancellationToken ct)
        {
            var (items, total) = await _repo.ListAsync(
                filter.PropertyId,
                filter.City,
                filter.State,
                filter.MinPrice,
                filter.MaxPrice,
                filter.Year,
                ct
            );

            return (_mapper.Map<IReadOnlyList<PropertyDto>>(items), total);
        }

    }
}
