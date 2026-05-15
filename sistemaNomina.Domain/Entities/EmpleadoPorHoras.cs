namespace sistemaNomina.Domain.Entities
{
    public class EmpleadoPorHoras : Empleado
    {
        public decimal SueldoPorHora { get; set; }
        public decimal HorasTrabajadas { get; set; }

        public override decimal CalcularPago()
        {
            const decimal horasRegulares = 40m;
            const decimal tarifaHorasExtra = 1.5m;

            if (HorasTrabajadas <= 40)
            {
                return SueldoPorHora * HorasTrabajadas;
            }
            else
            {
                decimal pagoBase = SueldoPorHora * horasRegulares;
                decimal horasExtra = HorasTrabajadas - horasRegulares;
                decimal pagoExtra = SueldoPorHora * tarifaHorasExtra * horasExtra;

                return pagoBase + pagoExtra;
            }
        }
    }
}