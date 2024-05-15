using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http.Logging;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();
            try
            {
                //Get all regions from Web API

                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7230/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());
            }
            catch (Exception ex)
            {
                //Log the exception
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel addRegionViewModel) 
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7230/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegionViewModel), Encoding.UTF8, "application/json")
            };

            var httpresponeMessage = await client.SendAsync(httpRequestMessage);
            httpresponeMessage.EnsureSuccessStatusCode();

            var response = await httpresponeMessage.Content.ReadFromJsonAsync<RegionDTO>();

            if (response != null) 
            {
                return RedirectToAction("Index","Regions");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7230/api/regions/{id.ToString()}");
            if (response != null)
            {
                return View(response);
            }
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> Edit(RegionDTO request)
        {
            var client = httpClientFactory.CreateClient();
            var httprequestMsg = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7230/api/regions{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpresponeMessage = await client.SendAsync(httprequestMsg);
            httpresponeMessage.EnsureSuccessStatusCode();

            var response = await httpresponeMessage.Content.ReadFromJsonAsync<RegionDTO>();

            if (response != null)
            {
                return RedirectToAction("Edit", "Regions");
            }
            return View();

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(     request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7230/api/regions{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");

            }
            catch (Exception)
            {
                
                
            }
            return View("Edit");
        }
    }
}
