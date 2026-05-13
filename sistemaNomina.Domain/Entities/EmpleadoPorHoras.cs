namespace sistemaNomina.Domain.Entities
{
    public class EmpleadoPorHoras : Empleado
    {
        public decimal SueldoPorHora { get; set; }
        public decimal HorasTrabajadas { get; set; }


        public override decimal CalcularPago()
        {
            if (HorasTrabajadas <= 40)
            {
                return SueldoPorHora * HorasTrabajadas;
            }
            else
            {
                decimal pagoBase = SueldoPorHora * 40m;
                decimal horasExtra = HorasTrabajadas - 40m;
                decimal pagoExtra = SueldoPorHora * 1.5m * horasExtra;

                return pagoBase + pagoExtra;
            }
        }
    }
}