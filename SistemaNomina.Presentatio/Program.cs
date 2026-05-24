using sistemaNomina.Domain.Entities;
using sistemaNomina.Domain.Interfaces;
using SistemaNomina.Infrastructure.Repositories;

IEmpleadoRepository repositorio = new EmpleadoRepositoryInMemory();

PrecargarEmpleados(repositorio);

bool salir = false;

while (!salir)
{
    Console.Clear();
    Console.WriteLine("=== SISTEMA DE GESTIÓN DE PÁGOS ===");
    Console.WriteLine("1. Registrar Nuevo Empleado");
    Console.WriteLine("2. Generar Reporte Semanal");
    Console.WriteLine("3. Editar Empleado");
    Console.WriteLine("4. Buscar Empleado por NSS");
    Console.WriteLine("5. Salir");
    Console.Write("Seleccione una opción: ");

    string opcion = Console.ReadLine()!;

    switch (opcion)
    {
        case "1":
            RegistrarEmpleado(repositorio);
            break;
        case "2":
            GenerarReporte(repositorio);
            break;
        case "3":
            EditarEmpleado(repositorio);
            break;
        case "4":
            BuscarEmpleado(repositorio);
            break;
        case "5":
            salir = true;
            break;
        default:
            Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
            Console.ReadKey();
            break;
    }
}

static void RegistrarEmpleado(IEmpleadoRepository repo)
{
    Console.Clear();
    Console.WriteLine("--- Registrar Empleado ---");
    Console.WriteLine("1. Asalariado");
    Console.WriteLine("2. Por Horas");
    Console.WriteLine("3. Por Comisión");
    Console.WriteLine("4. Asalariado por Comisión");
    Console.Write("Seleccione el tipo de contrato: ");
    string tipo = Console.ReadLine()!;

    Console.Write("Primer Nombre: ");
    string nombre = Console.ReadLine()!;
    Console.Write("Apellido Paterno: ");
    string apellido = Console.ReadLine()!;
    Console.Write("Número de Seguro Social: ");
    string nss = Console.ReadLine()!;

    Empleado nuevoEmpleado = null!;

    switch (tipo)
    {
        case "1":
            var empAsalariado = new EmpleadoAsalariado { PrimerNombre = nombre, ApellidoPaterno = apellido, NumeroSeguroSocial = nss };
            Console.Write("Salario Semanal: ");
            empAsalariado.SalarioSemanal = Convert.ToDecimal(Console.ReadLine());
            nuevoEmpleado = empAsalariado;
            break;
        case "2":
            var empHoras = new EmpleadoPorHoras { PrimerNombre = nombre, ApellidoPaterno = apellido, NumeroSeguroSocial = nss };
            Console.Write("Sueldo por Hora: ");
            empHoras.SueldoPorHora = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Horas Trabajadas: ");
            empHoras.HorasTrabajadas = Convert.ToDecimal(Console.ReadLine());
            nuevoEmpleado = empHoras;
            break;
        case "3":
            var empComision = new EmpleadoPorComision { PrimerNombre = nombre, ApellidoPaterno = apellido, NumeroSeguroSocial = nss };
            Console.Write("Ventas Brutas: ");
            empComision.VentasBrutas = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Tarifa de Comisión (ej. 0.05 para 5%): ");
            empComision.TarifaComision = Convert.ToDecimal(Console.ReadLine());
            nuevoEmpleado = empComision;
            break;
        case "4":
            var empAsalComision = new EmpleadoAsalariadoPorComision { PrimerNombre = nombre, ApellidoPaterno = apellido, NumeroSeguroSocial = nss };
            Console.Write("Salario Base: ");
            empAsalComision.SalarioBase = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Ventas Brutas: ");
            empAsalComision.VentasBrutas = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Tarifa de Comisión (ej. 0.05 para 5%): ");
            empAsalComision.TarifaComision = Convert.ToDecimal(Console.ReadLine());
            nuevoEmpleado = empAsalComision;
            break;
        default:
            Console.WriteLine("Tipo de empleado no válido.");
            break;
    }

    if (nuevoEmpleado != null)
    {
        try
        {
            repo.AgregarEmpleado(nuevoEmpleado);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n¡Empleado registrado con éxito!");
            Console.ResetColor();
        }
        catch (InvalidOperationException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERROR DE DUPLICADO] {ex.Message}");
            Console.ResetColor();
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[DATOS INVÁLIDOS] {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERROR INESPERADO] {ex.Message}");
            Console.ResetColor();
        }
    }

    Console.WriteLine("\nPresione cualquier tecla para continuar...");
    Console.ReadKey();
}

Console.WriteLine("Presione cualquier tecla para continuar...");
Console.ReadKey();
static void BuscarEmpleado(IEmpleadoRepository repo)
{
    Console.Clear();
    Console.WriteLine("--- Buscar Empleado ---");
    Console.Write("Ingrese el Número de Seguro Social (NSS) a buscar: ");
    string nss = Console.ReadLine()!;

    var empleado = repo.ObtenerPorNumeroSeguroSocial(nss);

    if (empleado == null)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nNo se encontró ningún empleado con ese NSS.");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine("\n--- Datos del Empleado ---");
        Console.WriteLine($"Nombre: {empleado.PrimerNombre} {empleado.ApellidoPaterno}");
        Console.WriteLine($"NSS: {empleado.NumeroSeguroSocial}");
        Console.WriteLine($"Tipo de Contrato: {empleado.GetType().Name}");
        Console.WriteLine(new string('-', 30));

        switch (empleado)
        {
            case EmpleadoAsalariado empAsalariado:
                Console.WriteLine($"Salario Semanal: RD$ {empAsalariado.SalarioSemanal:N2}");
                break;
            case EmpleadoPorHoras empHoras:
                Console.WriteLine($"Sueldo por Hora: RD$ {empHoras.SueldoPorHora:N2}");
                Console.WriteLine($"Horas Trabajadas: {empHoras.HorasTrabajadas}");
                break;
            case EmpleadoAsalariadoPorComision empAsalComision:
                Console.WriteLine($"Salario Base: RD$ {empAsalComision.SalarioBase:N2}");
                Console.WriteLine($"Ventas Brutas: RD$ {empAsalComision.VentasBrutas:N2}");
                Console.WriteLine($"Tarifa de Comisión: {empAsalComision.TarifaComision}");
                break;
            case EmpleadoPorComision empComision:
                Console.WriteLine($"Ventas Brutas: RD$ {empComision.VentasBrutas:N2}");
                Console.WriteLine($"Tarifa de Comisión: {empComision.TarifaComision}");
                break;
        }

        Console.WriteLine(new string('-', 30));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Pago Calculado: RD$ {empleado.CalcularPago():N2}");
        Console.ResetColor();
    }

    Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
    Console.ReadKey();
}

static void EditarEmpleado(IEmpleadoRepository repo)
{
    Console.Clear();
    Console.WriteLine("--- Editar Empleado ---");
    Console.Write("Ingrese el Número de Seguro Social (NSS) a editar: ");
    string nss = Console.ReadLine()!;

    var empleado = repo.ObtenerPorNumeroSeguroSocial(nss);

    if (empleado == null)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nNo se encontró ningún empleado con ese NSS.");
        Console.ResetColor();
        Console.WriteLine("Presione cualquier tecla para volver...");
        Console.ReadKey();
        return;
    }

    Console.WriteLine($"\nEditando a: {empleado.PrimerNombre} {empleado.ApellidoPaterno} | Contrato: {empleado.GetType().Name}");
    Console.WriteLine("(Deje en blanco y presione Enter si no desea modificar un campo)\n");

    Console.Write($"Nuevo Primer Nombre ({empleado.PrimerNombre}): ");
    string nuevoNombre = Console.ReadLine()!;
    if (!string.IsNullOrWhiteSpace(nuevoNombre)) empleado.PrimerNombre = nuevoNombre;

    Console.Write($"Nuevo Apellido ({empleado.ApellidoPaterno}): ");
    string nuevoApellido = Console.ReadLine()!;
    if (!string.IsNullOrWhiteSpace(nuevoApellido)) empleado.ApellidoPaterno = nuevoApellido;

    switch (empleado)
    {
        case EmpleadoAsalariado empAsalariado:
            Console.Write($"Nuevo Salario Semanal ({empAsalariado.SalarioSemanal:N2}): ");
            string salInput = Console.ReadLine()!;
            if (decimal.TryParse(salInput, out decimal nuevoSalario)) empAsalariado.SalarioSemanal = nuevoSalario;
            break;

        case EmpleadoPorHoras empHoras:
            Console.Write($"Nuevo Sueldo por Hora ({empHoras.SueldoPorHora:N2}): ");
            string sueldoInput = Console.ReadLine()!;
            if (decimal.TryParse(sueldoInput, out decimal nuevoSueldo)) empHoras.SueldoPorHora = nuevoSueldo;

            Console.Write($"Nuevas Horas Trabajadas ({empHoras.HorasTrabajadas}): ");
            string horasInput = Console.ReadLine()!;
            if (decimal.TryParse(horasInput, out decimal nuevasHoras)) empHoras.HorasTrabajadas = nuevasHoras;
            break;

        case EmpleadoAsalariadoPorComision empAsalComision:
            Console.Write($"Nuevo Salario Base ({empAsalComision.SalarioBase:N2}): ");
            string baseInput = Console.ReadLine()!;
            if (decimal.TryParse(baseInput, out decimal nuevaBase)) empAsalComision.SalarioBase = nuevaBase;

            Console.Write($"Nuevas Ventas Brutas ({empAsalComision.VentasBrutas:N2}): ");
            string ventasACInput = Console.ReadLine()!;
            if (decimal.TryParse(ventasACInput, out decimal nuevasVentasAC)) empAsalComision.VentasBrutas = nuevasVentasAC;

            Console.Write($"Nueva Tarifa de Comisión ({empAsalComision.TarifaComision}): ");
            string tarifaACInput = Console.ReadLine()!;
            if (decimal.TryParse(tarifaACInput, out decimal nuevaTarifaAC)) empAsalComision.TarifaComision = nuevaTarifaAC;
            break;

        case EmpleadoPorComision empComision:
            Console.Write($"Nuevas Ventas Brutas ({empComision.VentasBrutas:N2}): ");
            string ventasInput = Console.ReadLine()!;
            if (decimal.TryParse(ventasInput, out decimal nuevasVentas)) empComision.VentasBrutas = nuevasVentas;

            Console.Write($"Nueva Tarifa de Comisión ({empComision.TarifaComision}): ");
            string tarifaInput = Console.ReadLine()!;
            if (decimal.TryParse(tarifaInput, out decimal nuevaTarifa)) empComision.TarifaComision = nuevaTarifa;
            break;
    }
    try
    {
        repo.ActualizarEmpleado(empleado);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n¡Empleado actualizado con éxito! El sistema recalculará su pago automáticamente.");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nError al actualizar: {ex.Message}");
    }

    Console.WriteLine("Presione cualquier tecla para continuar...");
    Console.ReadKey();
}

static void GenerarReporte(IEmpleadoRepository repo)
{
    Console.Clear();
    Console.WriteLine("--- Reporte Semanal de Pagos ---");

    var empleados = repo.ObtenerEmpleados();
    int contador = 0;

    foreach (var emp in empleados)
    {
        Console.WriteLine($"\nEmpleado: {emp.PrimerNombre} {emp.ApellidoPaterno}");
        Console.WriteLine($"NSS: {emp.NumeroSeguroSocial}");
        Console.WriteLine($"Tipo de Contrato: {emp.GetType().Name}");

        Console.WriteLine($"Pago Calculado: RD$ {emp.CalcularPago():N2}");
        Console.WriteLine(new string('-', 40));
        contador++;
    }

    if (contador == 0)
    {
        Console.WriteLine("No hay empleados registrados actualmente.");
    }

    Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
    Console.ReadKey();
}
static void PrecargarEmpleados(IEmpleadoRepository repo)
{
    Random rnd = new Random();
    string[] nombres = { "Amaudis", "Anderson", "Camil", "Carlos", "Abel", "Nayeli", "Laura", "Javier" };
    string[] apellidos = { "Almanzar", "Diaz", "Santana", "Castellanos", "Gonzalez", "Perez", "Ramirez", "Gomez" };

    for (int i = 1; i <= 50; i++)
    {
        string nombre = nombres[rnd.Next(nombres.Length)];
        string apellido = apellidos[rnd.Next(apellidos.Length)];
        string nss = $"NSS-{rnd.Next(100, 999)}-{rnd.Next(1000, 9999)}";

        int tipoContrato = rnd.Next(1, 5);

        switch (tipoContrato)
        {
            case 1:
                repo.AgregarEmpleado(new EmpleadoAsalariado
                {
                    PrimerNombre = nombre,
                    ApellidoPaterno = apellido,
                    NumeroSeguroSocial = nss,
                    SalarioSemanal = rnd.Next(10000, 25000)
                });
                break;
            case 2:
                repo.AgregarEmpleado(new EmpleadoPorHoras
                {
                    PrimerNombre = nombre,
                    ApellidoPaterno = apellido,
                    NumeroSeguroSocial = nss,
                    SueldoPorHora = rnd.Next(300, 800),
                    HorasTrabajadas = rnd.Next(20, 60)
                });
                break;
            case 3:
                repo.AgregarEmpleado(new EmpleadoPorComision
                {
                    PrimerNombre = nombre,
                    ApellidoPaterno = apellido,
                    NumeroSeguroSocial = nss,
                    VentasBrutas = rnd.Next(50000, 200000),
                    TarifaComision = (decimal)rnd.NextDouble() * 0.10m
                });
                break;
            case 4:
                repo.AgregarEmpleado(new EmpleadoAsalariadoPorComision
                {
                    PrimerNombre = nombre,
                    ApellidoPaterno = apellido,
                    NumeroSeguroSocial = nss,
                    SalarioBase = rnd.Next(8000, 15000),
                    VentasBrutas = rnd.Next(50000, 200000),
                    TarifaComision = (decimal)rnd.NextDouble() * 0.10m
                });
                break;
        }
    }
}