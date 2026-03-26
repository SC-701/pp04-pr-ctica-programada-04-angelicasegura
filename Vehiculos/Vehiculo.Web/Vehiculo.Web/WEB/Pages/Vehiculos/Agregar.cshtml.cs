using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace WEB.Pages.Vehiculos
{
    [Authorize(Roles = "2")]
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        [BindProperty]
        public VehiculoRequest vehiculo { get; set; }
        [BindProperty]
        public List<SelectListItem> marcas { get; set; }
        [BindProperty]
        public List<SelectListItem> modelos { get; set; }
        [BindProperty]
        public Guid marcaSeleccionada { get; set; }
        public async Task<ActionResult> OnGet()
        {
            await ObtenerMarcas();
            return Page();

        }
        public async Task<ActionResult> OnPost()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarVehiculo");
            var cliente = ObtenerClienteConToken();
            var solicitud = new HttpRequestMessage(HttpMethod.Post, endpoint);
            var respuesta = await cliente.PostAsJsonAsync(endpoint, vehiculo);
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");

        }


        private async Task ObtenerMarcas()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerMarcas");
            var cliente = ObtenerClienteConToken();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var resultadoDeserializado = JsonSerializer.Deserialize<List<Marca>>(resultado, opciones);
            marcas = resultadoDeserializado.Select(m =>
            new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nombre
            }
            ).ToList();
        }

        private async Task<List<Modelo>> ObtenerModelos(Guid marcaId)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerModelos");
            var cliente = ObtenerClienteConToken();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, marcaId));
            var respuesta = await cliente.SendAsync(solicitud);
            if (respuesta.StatusCode == HttpStatusCode.OK) {
                respuesta.EnsureSuccessStatusCode();
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<Modelo>>(resultado, opciones);

            }
           
                return new List<Modelo>();
            

                

        }
        public async Task<JsonResult> OnGetObtenerModelos(Guid marcaId)
        {
            var modelos = await ObtenerModelos(marcaId);
            return new JsonResult(modelos);
        }

        private HttpClient ObtenerClienteConToken()
        {
            var tokenClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Token");
            var cliente = new HttpClient();
            if (tokenClaim != null)
                cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", tokenClaim.Value);
            return cliente;
        }
    }
}
