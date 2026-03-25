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
    [Authorize]
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
       
        [BindProperty]
        public VehiculoResponse vehiculoResponse { get; set; }
        [BindProperty]
        public List<SelectListItem> marcas { get; set; }
        [BindProperty]
        public List<SelectListItem> modelos { get; set; }
        [BindProperty]
        public Guid marcaSeleccionada { get; set; }
        [BindProperty]
        public Guid modeloSeleccionado { get; set; }
        public async Task<ActionResult> OnGet(Guid? id )
        {
            if (id == Guid.Empty)
                return NotFound();
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerVehiculo");
            var cliente = ObtenerClienteConToken();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK) {
                await ObtenerMarcas();

                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                vehiculoResponse = JsonSerializer.Deserialize<VehiculoResponse>(resultado, opciones);
                if (vehiculoResponse != null)
                {
                    marcaSeleccionada = Guid.Parse(marcas.Where(m => m.Text == vehiculoResponse.Marca).FirstOrDefault().Value);
                    modelos = (await ObtenerModelos(marcaSeleccionada)).Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nombre,
                        Selected = m.Nombre == vehiculoResponse.Modelo
                    }).ToList();
                    modeloSeleccionado=Guid.Parse(modelos.Where(m => m.Text == vehiculoResponse.Modelo).FirstOrDefault().Value);
                }
            }
                
            
            
            return Page();

        }
        public async Task<ActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarVehiculo");
            var cliente = ObtenerClienteConToken();
            var respuesta = await cliente.PutAsJsonAsync<VehiculoRequest>(string.Format(endpoint, vehiculoResponse.Id),new
                VehiculoRequest
            {
                Placa = vehiculoResponse.Placa,
                IdModelo = modeloSeleccionado,
                Anio = vehiculoResponse.Anio,
                Color = vehiculoResponse.Color,
                Precio = vehiculoResponse.Precio,
                CorreoPropietario = vehiculoResponse.CorreoPropietario,
                TelefonoPropietario = vehiculoResponse.TelefonoPropietario
            });
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
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
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

