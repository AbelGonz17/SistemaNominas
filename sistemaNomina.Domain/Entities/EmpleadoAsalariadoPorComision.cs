namespace sistemaNomina.Domain.Entities
{
    public class EmpleadoAsalariadoPorComision : EmpleadoPorComision
    {
        public decimal SalarioBase { get; set; }

        public override decimal CalcularPago()
        {
            const decimal porcentajeBono = 0.10m;

            decimal comision = base.CalcularPago();
            decimal bonoAdicional = SalarioBase * porcentajeBono;

            return comision + SalarioBase + bonoAdicional;
        }
    }
}