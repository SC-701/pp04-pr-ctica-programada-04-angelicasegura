using Abstracciones.Interfaces.Reglas;
using Microsoft.Extensions.Configuration;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private IConfiguration _configuracion;
        public Configuracion (IConfiguration configuration)
        {
            _configuracion = configuration;
        }
        public string ObtenerMetodo(string seccion, string nombre)
        {
            throw new NotImplementedException();
        }

        public string ObtenerValor(string llave)
        {
            throw new NotImplementedException();
        }
    }
}
