using sistemaNomina.Domain.Entities;

namespace sistemaNomina.Domain.Interfaces
{
    public interface IEmpleadoRepository
    {
        void AgregarEmpleado(Empleado empleado);
        void ActualizarEmpleado(Empleado empleado);
        IEnumerable<Empleado> ObtenerEmpleados();
        Empleado ObtenerPorNumeroSeguroSocial(string numeroSeguroSocial);
    }
}