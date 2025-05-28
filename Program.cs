using System;
using System.Collections.Generic;
using System.Linq;


namespace TP03_ORTFlix
{
    internal class Program
    {
        class ORTFlix{
            public const string CLIENTE_INEXISTENTE = "CLIENTE_INEXISTENTE";
            public const string CONTENIDO_INEXISTENTE = "CONTENIDO_INEXISTENTE";
            public const string CLIENTE_DEUDOR = "CLIENTE_DEUDOR";
            public const string CONTENIDO_NO_DISPONIBLE = "CONTENIDO_NO_DISPONIBLE";
            public const string OK = "OK";

            public const string CLIENTE_EXISTENTE = "CLIENTE_EXISTENTE";
            public const string ALTA_OK = "ALTA_OK";
            public const string CLIENTE_DEUDOR_PREVIO = "CLIENTE_DEUDOR";

            public const string SERVICIO_STANDARD = "Standard";
            public const string SERVICIO_PREMIUM = "Premium";

            class Pelicula
            {
                public string Nombre;
                public bool EsPremium;

                public Pelicula(string nombre, bool esPremium)
                {
                    Nombre = nombre;
                    EsPremium = esPremium;
                }
            }

            class Cliente
            {
                public int DNI;
                public string Nombre;
                public string Servicio;
                public decimal Saldo;
                public List<string> HistorialPeliculas;

                public Cliente(int dni, string nombre, string servicio)
                {
                    DNI = dni;
                    Nombre = nombre;
                    Servicio = servicio;
                    Saldo = 0;
                    HistorialPeliculas = new List<string>();
                }

                public bool EsDeudor()
                {
                    return Saldo > 0;
                }

                public void VerPelicula(string nombrePelicula)
                {
                    HistorialPeliculas.Add(nombrePelicula);
                }
            }

            List<Cliente> clientes = new List<Cliente>();
            List<Cliente> listaNegra = new List<Cliente>();
            List<Pelicula> peliculas = new List<Pelicula>();

            public void VerPelicula()
            {
                Console.Write("Ingrese el DNI del cliente: ");
                int dni = int.Parse(Console.ReadLine());

                Console.Write("Ingrese el nombre de la película: ");
                string nombrePelicula = Console.ReadLine();

                Cliente cliente = clientes.FirstOrDefault(c => c.DNI == dni);
                if (cliente == null)
                {
                    Console.WriteLine(CLIENTE_INEXISTENTE);
                    return;
                }

                Pelicula pelicula = peliculas.FirstOrDefault(p => p.Nombre == nombrePelicula);
                if (pelicula == null)
                {
                    Console.WriteLine(CONTENIDO_INEXISTENTE);
                    return;
                }

                if (cliente.EsDeudor())
                {
                    Console.WriteLine(CLIENTE_DEUDOR);
                    return;
                }

                if (pelicula.EsPremium && cliente.Servicio != SERVICIO_PREMIUM)
                {
                    Console.WriteLine(CONTENIDO_NO_DISPONIBLE);
                    return;
                }

                cliente.VerPelicula(nombrePelicula);
                Console.WriteLine(OK);
            }

            public void DarDeBaja()
            {
                Console.Write("Ingrese el DNI del cliente a dar de baja: ");
                int dni = int.Parse(Console.ReadLine());

                Cliente cliente = clientes.FirstOrDefault(c => c.DNI == dni);
                if (cliente != null)
                {
                    clientes.Remove(cliente);
                    if (cliente.EsDeudor())
                    {
                        listaNegra.Add(cliente);
                        Console.WriteLine("Cliente dado de baja y agregado a la lista negra.");
                    }
                    else
                    {
                        Console.WriteLine("Cliente dado de baja.");
                    }
                }
                else
                {
                    Console.WriteLine("Cliente no encontrado.");
                }
            }

            public void DarDeAlta()
            {
                Console.Write("Ingrese DNI: ");
                int dni = int.Parse(Console.ReadLine());

                if (clientes.Any(c => c.DNI == dni))
                {
                    Console.WriteLine(CLIENTE_EXISTENTE);
                    return;
                }

                Cliente exDeudor = listaNegra.FirstOrDefault(c => c.DNI == dni);
                if (exDeudor != null && exDeudor.EsDeudor())
                {
                    Console.WriteLine(CLIENTE_DEUDOR_PREVIO);
                    return;
                }

                Console.Write("Ingrese nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Ingrese tipo de servicio (Standard/Premium): ");
                string servicio = Console.ReadLine();

                Cliente nuevoCliente = new Cliente(dni, nombre, servicio);
                clientes.Add(nuevoCliente);
                Console.WriteLine(ALTA_OK);
            }

            public void DepurarListaNegra()
            {
                Console.Write("Ingrese el importe tope para limpiar lista negra: ");
                decimal tope = decimal.Parse(Console.ReadLine());

                int eliminados = listaNegra.RemoveAll(c => c.Saldo <= tope);
                Console.WriteLine($"Se eliminaron {eliminados} clientes de la lista negra.");
            }

            public void MostrarClientes()
            {
                Console.WriteLine("\nClientes activos:");
                foreach (var c in clientes)
                {
                    Console.WriteLine($"DNI: {c.DNI}, Nombre: {c.Nombre}, Servicio: {c.Servicio}, Saldo: {c.Saldo}, Historial: {string.Join(", ", c.HistorialPeliculas)}");
                }
                Console.WriteLine();
            }

            public void MostrarListaNegra()
            {
                Console.WriteLine("\nLista Negra:");
                foreach (var c in listaNegra)
                {
                    Console.WriteLine($"DNI: {c.DNI}, Nombre: {c.Nombre}, Deuda: {c.Saldo}");
                }
                Console.WriteLine();
            }

            public void PrecargarPeliculas()
            {
                peliculas.Add(new Pelicula("Pelicula", true));
                peliculas.Add(new Pelicula("Pelicula: La secuela", false));
                peliculas.Add(new Pelicula("Pelicula: La leyenda del cine", true));
                peliculas.Add(new Pelicula("Pelicula: Origenes", false));
            }

            public void Menu()
            {
                while (true)
                {
                    Console.WriteLine("========== ORTFlix ===========");
                    Console.WriteLine("| 1. Dar de alta cliente     |");
                    Console.WriteLine("| 2. Dar de baja cliente     |");
                    Console.WriteLine("| 3. Ver película            |");
                    Console.WriteLine("| 4. Depurar lista negra     |");
                    Console.WriteLine("| 5. Mostrar clientes        |");
                    Console.WriteLine("| 6. Mostrar lista negra     |");
                    Console.WriteLine("| 0. Salir                   |");
                    Console.WriteLine("==============================");
                    Console.WriteLine("Seleccione una opción");

                    string opcion = Console.ReadLine();

                    Console.WriteLine();

                    switch (opcion)
                    {
                        case "1":
                            DarDeAlta();
                            break;
                        case "2":
                            DarDeBaja();
                            break;
                        case "3":
                            VerPelicula();
                            break;
                        case "4":
                            DepurarListaNegra();
                            break;
                        case "5":
                            MostrarClientes();
                            break;
                        case "6":
                            MostrarListaNegra();
                            break;
                        case "0":
                            Console.WriteLine("Saliendo...");
                            return;
                        default:
                            Console.WriteLine("Opción inválida.");
                            break;
                    }

                    Console.WriteLine();
                }
            }

            public static void Main()
            {
                ORTFlix sistema = new ORTFlix();
                sistema.PrecargarPeliculas(); // Carga inicial de películas
                sistema.Menu(); // Muestra el menú interactivo
            }
        }
    }
}
