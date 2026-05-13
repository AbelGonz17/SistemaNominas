namespace sistemaNomina.Domain.Entities
{
    public class EmpleadoAsalariado : Empleado
    {
        public decimal SalarioSemanal { get; set; }

        public override decimal CalcularPago()
        {
            return SalarioSemanal;
        }
    }
}