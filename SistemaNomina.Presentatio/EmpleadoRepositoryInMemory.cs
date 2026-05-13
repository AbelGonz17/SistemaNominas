using sistemaNomina.Domain.Entities;
using sistemaNomina.Domain.Interfaces;

namespace SistemaNomina.Infrastructure.Repositories
{
    public class EmpleadoRepositoryInMemory : IEmpleadoRepository
    {
        private readonly List<Empleado> _empleados;

        public EmpleadoRepositoryInMemory()
        {
            _empleados = new List<Empleado>();
        }

        public void AgregarEmpleado(Empleado empleado)
        {
            if (empleado == null)
                throw new ArgumentNullException(nameof(empleado), "El objeto empleado no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(empleado.NumeroSeguroSocial))
                throw new ArgumentException("El Número de Seguro Social es obligatorio para el registro.");

            if (_empleados.Any(e => e.NumeroSeguroSocial == empleado.NumeroSeguroSocial))
            {
                throw new InvalidOperationException($"No se puede registrar. Ya existe un empleado con el NSS: {empleado.NumeroSeguroSocial}");
            }

            _empleados.Add(empleado);
        }

        public void ActualizarEmpleado(Empleado empleado)
        {
            if (empleado == null)
                throw new ArgumentNullException(nameof(empleado), "El objeto empleado no puede ser nulo.");

            var index = _empleados.FindIndex(e => e.NumeroSeguroSocial == empleado.NumeroSeguroSocial);

            if (index != -1)
            {
                _empleados[index] = empleado;
            }
            else
            {
                throw new KeyNotFoundException($"No se encontró ningún empleado con el NSS: {empleado.NumeroSeguroSocial} para actualizar.");
            }
        }

        public IEnumerable<Empleado> ObtenerEmpleados()
        {
            return _empleados;
        }

        public Empleado ObtenerPorNumeroSeguroSocial(string numeroSeguroSocial)
        {
            if (string.IsNullOrWhiteSpace(numeroSeguroSocial))
                throw new ArgumentException("Debe proporcionar un Número de Seguro Social válido para la búsqueda.");

            return _empleados.FirstOrDefault(e => e.NumeroSeguroSocial == numeroSeguroSocial)!;
        }
    }
}