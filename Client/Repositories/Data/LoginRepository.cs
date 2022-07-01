using API.Models;
using API.ModelsInsert;
using API.ModelsResponse;
using Client.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class LoginRepository : GeneralRepository<Employees, string>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginRepository(Address address, string request = "Employee/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }

        public async Task<LoginResponse> Auth(EmployeeLoginModel EmployeeLoginModel)
        {
            LoginResponse token = null;

            StringContent content = new StringContent(JsonConvert.SerializeObject(EmployeeLoginModel), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("employee/login", content);

            string apiResponse = await result.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<LoginResponse>(apiResponse);

            return token;
        }
    }
}