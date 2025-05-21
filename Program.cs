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

            public void AgregarPelicula(string nombre, bool esPremium)
            {
                peliculas.Add(new Pelicula(nombre, esPremium));
            }

            public string VerPelicula(int dni, string nombrePelicula)
            {
                Cliente cliente = clientes.FirstOrDefault(c => c.DNI == dni);
                if (cliente == null)
                    return CLIENTE_INEXISTENTE;

                Pelicula pelicula = peliculas.FirstOrDefault(p => p.Nombre == nombrePelicula);
                if (pelicula == null)
                    return CONTENIDO_INEXISTENTE;

                if (cliente.EsDeudor())
                    return CLIENTE_DEUDOR;

                if (pelicula.EsPremium && cliente.Servicio != SERVICIO_PREMIUM)
                    return CONTENIDO_NO_DISPONIBLE;

                cliente.VerPelicula(nombrePelicula);
                return OK;
            }

            public void DarDeBaja(int dni)
            {
                Cliente cliente = clientes.FirstOrDefault(c => c.DNI == dni);
                if (cliente != null)
                {
                    clientes.Remove(cliente);
                    if (cliente.EsDeudor())
                        listaNegra.Add(cliente);
                }
            }

            public string DarDeAlta(int dni, string nombre, string servicio)
            {
                if (clientes.Any(c => c.DNI == dni))
                    return CLIENTE_EXISTENTE;

                Cliente exDeudor = listaNegra.FirstOrDefault(c => c.DNI == dni);
                if (exDeudor != null && exDeudor.EsDeudor())
                    return CLIENTE_DEUDOR_PREVIO;

                Cliente nuevoCliente = new Cliente(dni, nombre, servicio);
                clientes.Add(nuevoCliente);
                return ALTA_OK;
            }

            public void DepurarListaNegra(decimal importeTope)
            {
                listaNegra.RemoveAll(c => c.Saldo <= importeTope);
            }

            // Métodos auxiliares para ver el estado
            public void MostrarClientes()
            {
                Console.WriteLine("Clientes activos:");
                foreach (var c in clientes)
                {
                    Console.WriteLine($"DNI: {c.DNI}, Nombre: {c.Nombre}, Servicio: {c.Servicio}, Saldo: {c.Saldo}, Vió: {string.Join(", ", c.HistorialPeliculas)}");
                }
            }

            public void MostrarListaNegra()
            {
                Console.WriteLine("Lista Negra:");
                foreach (var c in listaNegra)
                {
                    Console.WriteLine($"DNI: {c.DNI}, Nombre: {c.Nombre}, Deuda: {c.Saldo}");
                }
            }

            public static void Main()
            {
                ORTFlix sistema = new ORTFlix();

                // Películas
                sistema.AgregarPelicula("Avengers", false);
                sistema.AgregarPelicula("Dune 2", true);

                // Alta de clientes
                Console.WriteLine(sistema.DarDeAlta(111, "Juan", SERVICIO_STANDARD)); // ALTA_OK
                Console.WriteLine(sistema.DarDeAlta(222, "Ana", SERVICIO_PREMIUM));   // ALTA_OK

                // Simular deuda
                sistema.clientes[0].Saldo = 100;

                // Pruebas ver película
                Console.WriteLine(sistema.VerPelicula(111, "Avengers"));  // CLIENTE_DEUDOR
                sistema.clientes[0].Saldo = 0;
                Console.WriteLine(sistema.VerPelicula(111, "Dune 2"));    // CONTENIDO_NO_DISPONIBLE
                Console.WriteLine(sistema.VerPelicula(222, "Dune 2"));    // OK

                // Dar de baja y agregar a lista negra
                sistema.clientes[0].Saldo = 50;
                sistema.DarDeBaja(111);

                // Intento de alta con deuda previa
                Console.WriteLine(sistema.DarDeAlta(111, "Juan", SERVICIO_STANDARD)); // CLIENTE_DEUDOR

                // Depurar lista negra
                sistema.DepurarListaNegra(50);

                // Alta luego de depuración
                Console.WriteLine(sistema.DarDeAlta(111, "Juan", SERVICIO_PREMIUM)); // ALTA_OK

                // Mostrar resultados
                Console.WriteLine();
                sistema.MostrarClientes();
                Console.WriteLine();
                sistema.MostrarListaNegra();
            }
        }
        }
}