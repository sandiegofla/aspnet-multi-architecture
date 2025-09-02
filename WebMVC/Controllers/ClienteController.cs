using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc; 
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class ClienteController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:5001/api/cliente"; 

        public ClienteController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<ClienteEndereco>>(json);
                return View(clientes);
            }

            return View(new List<ClienteEndereco>());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClienteEndereco clienteEndereco)
        {
            var json = JsonConvert.SerializeObject(clienteEndereco);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(clienteEndereco);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<ClienteEndereco>>(json);
                var cliente = clientes.Find(c => c.Cliente.Id == id);
                return View(cliente);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, ClienteEndereco clienteEndereco)
        {
            var json = JsonConvert.SerializeObject(clienteEndereco);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(clienteEndereco);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            return RedirectToAction("Index");
        }
    }
}
