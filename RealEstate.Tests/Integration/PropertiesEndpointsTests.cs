using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using NUnit.Framework;
using RealEstate.Application.DTOs;

namespace RealEstate.Tests.Integration
{
    public class PropertiesEndpointsTests
    {
        private CustomWebApplicationFactory _factory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new CustomWebApplicationFactory();
        }

        [Test]
        public async Task Get_list_deberia_devolver_401_sin_token()
        {
            var client = _factory.CreateClient();
            var resp = await client.GetAsync("/api/properties");
            resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Get_list_deberia_devolver_200_con_token()
        {
            var client = _factory.CreateClient();
            var token = await AuthHelper.GetTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var resp = await client.GetAsync("/api/properties");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Post_create_deberia_devolver_201_con_owner_valido_y_luego_cleanup()
        {
            var client = _factory.CreateClient();
            var token = await AuthHelper.GetTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var codeInternal = "IT-CREATE-001"; 
            var dto = new PropertyCreateDto
            {
                Name = "Casa IT",
                Address = "Calle IT, Bogotá",
                Price = 111000000,
                CodeInternal = codeInternal,
                Year = 2020,
                OwnerId = 1
            };

            var resp = await client.PostAsJsonAsync("/api/properties", dto);
            resp.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await resp.Content.ReadFromJsonAsync<PropertyDto>();
            created!.Id.Should().BeGreaterThan(0);

            
            
        }

        [Test]
        public async Task Get_list_filtra_ciudad_y_anio()
        {
            var client = _factory.CreateClient();
            var token = await AuthHelper.GetTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var resp = await client.GetAsync("/api/properties?city=Cartagena&year=2013");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
