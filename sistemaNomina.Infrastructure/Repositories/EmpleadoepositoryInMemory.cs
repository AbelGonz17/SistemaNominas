using sistemaNomina.Domain.Entities;
using sistemaNomina.Domain.Interfaces;

namespace sistemaNomina.Infrastructure.Repositories
{
    public class EmpleadoepositoryInMemory : IEmpleadoRepository
    {
        private readonly List<Empleado> _empleados;
        public EmpleadoepositoryInMemory()
        {
            _empleados = new List<Empleado>();
        }

        public void ActualizarEmpleado(Empleado empleado)
        {
            var index = _empleados.FindIndex(e => e.NumeroSeguroSocial == empleado.NumeroSeguroSocial);
            if (index != -1)
            {
                _empleados[index] = empleado;
            }
        }

        public void AgregarEmpleado(Empleado empleado)
        {
            if (!_empleados.Any(e => e.NumeroSeguroSocial == empleado.NumeroSeguroSocial))
            {
                _empleados.Add(empleado);
            }
        }

        public IEnumerable<Empleado> ObtenerEmpleados()
        {
            return _empleados;
        }

        public Empleado ObtenerPorNumeroSeguroSocial(string numeroSeguroSocial)
        {
            return _empleados.FirstOrDefault(e => e.NumeroSeguroSocial == numeroSeguroSocial)!;
        }
    }
}