using CustomerApi.Models;
using CustomerApi.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests
{
    public class CustomerEndpointTests
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public CustomerEndpointTests()
        {
            _webApplicationFactory = new WebApplicationFactory<Startup>();
            SeedData().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GetAllCustomers_WithNoQuery_ReturnsOk()
        {
            using (var client = _webApplicationFactory.CreateClient())
            {
                var getResult = await client.GetAsync("/customers");
                getResult.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetAllCustomers_WithQuery_ReturnsOk()
        {
            using (var client = _webApplicationFactory.CreateClient())
            {
                var getResult = await client.GetAsync("/customers?firstNameIncludes=jor&lastNameIncludes=le");
                getResult.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task CreateCustomer_WithInvalidData_ReturnsBadRequest()
        {
            using (var client = _webApplicationFactory.CreateClient())
            {
                var customerUpdateModel = new CustomerModel
                {
                    FirstName = "John",
                    LastName = null,
                    DateOfBirth = new DateTime(1980, 1, 1)
                };

                var invalidPostContent = new StringContent(
                        JsonSerializer.Serialize(customerUpdateModel),
                        Encoding.UTF8,
                        "application/json"
                        );
                var postResult = await client.PostAsync($"/customers", invalidPostContent);
                postResult.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task UpdateCustomer_WithInvalidData_ReturnsBadRequest()
        {
            using (var client = _webApplicationFactory.CreateClient())
            {
                var customerUpdateModel = new CustomerModel
                {
                    FirstName = "John",
                    LastName = null,
                    DateOfBirth = new DateTime(1980, 1, 1)
                };

                var invalidPutContent = new StringContent(
                        JsonSerializer.Serialize(customerUpdateModel),
                        Encoding.UTF8,
                        "application/json"
                        );
                var putResult = await client.PutAsync($"/customers/4", invalidPutContent);
                putResult.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task DeleteCustomer_ValidCustomer_ReturnsOk()
        {
            using (var client = _webApplicationFactory.CreateClient())
            {
                var deleteResult = await client.DeleteAsync($"/customers/4");
                deleteResult.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task DeleteCustomer_InvalidCustomer_ReturnsNotFound()
        {
            using (var client = _webApplicationFactory.CreateClient())
            {
                var deleteResult = await client.DeleteAsync($"/customers/99");
                deleteResult.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
            }
        }

        private async Task SeedData()
        {
            var testCustomerData = RepositoryTestHelper.GetMockCustomerData();
            using (var client = _webApplicationFactory.CreateClient())
            {
                foreach (var testCustomer in testCustomerData)
                {
                    var validNewCustomer = new CustomerModel
                    {
                        FirstName = testCustomer.FirstName,
                        LastName = testCustomer.LastName,
                        DateOfBirth = testCustomer.DateOfBirth
                    };
                    var validPostContent = new StringContent(
                        JsonSerializer.Serialize(validNewCustomer),
                        Encoding.UTF8,
                        "application/json"
                        );
                    await client.PostAsync("/customers", validPostContent);
                }
            }
        }
    }
}