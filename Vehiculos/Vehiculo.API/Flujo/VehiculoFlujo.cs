using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class VehiculoFlujo : IVehiculoFlujo
    {
        private IVehiculoDA _vehiculoDA;

        public VehiculoFlujo(IVehiculoDA vehiculoDA)
        {
            _vehiculoDA = vehiculoDA;
        }

        public Task<Guid> Agregar(VehiculoRequest vehiculo)
        {
            return _vehiculoDA.Agregar(vehiculo);
        }

        public Task<Guid> Editar(Guid Id, VehiculoRequest vehiculo)
        {
            return _vehiculoDA.Editar(Id, vehiculo);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return _vehiculoDA.Eliminar(Id);
        }

        public Task<IEnumerable<VehiculoResponse>> Obtener()
        {
           return _vehiculoDA.Obtener();
        }

        public Task<VehiculoDetalle> Obtener(Guid Id)
        {
           return _vehiculoDA.Obtener(Id);
        }
    }
}
