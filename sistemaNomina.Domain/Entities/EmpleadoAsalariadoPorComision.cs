namespace sistemaNomina.Domain.Entities
{
    public class EmpleadoAsalariadoPorComision : EmpleadoPorComision
    {
        public decimal SalarioBase { get; set; }

        public override decimal CalcularPago()
        {
            decimal comision = base.CalcularPago();
            decimal bonoAdicional = SalarioBase * 0.10m;

            return comision + SalarioBase + bonoAdicional;
        }

    }
}