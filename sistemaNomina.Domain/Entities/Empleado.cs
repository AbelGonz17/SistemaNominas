namespace sistemaNomina.Domain.Entities
{
    public abstract class Empleado
    {
        public string PrimerNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string NumeroSeguroSocial { get; set; }


        public abstract decimal CalcularPago();
    }
}