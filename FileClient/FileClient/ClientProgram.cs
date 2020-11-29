using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Domain;
using DataAccess;
using System.Linq;
using System.Text;
using TCPComm.Constants;
using TCPComm.Protocol;
using FileClient;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Servidor
{
    public class ClientProgram
    {
        public static async Task Main(string[] args)
        {
            Console.Clear();
            Console.Title = "ADMINISTRACIÓN DEL SERVIDOR";
            Console.WriteLine($"{"ADMINISTRACIÓN DEL SERVIDOR"}\n\nPresione una tecla para continuar...\n");

            Console.ReadLine();

            string ip_server = ConsoleValidations.PromptIPsAvailablesOnPC("ADMINISTRACIÓN DEL SERVIDOR");

            //Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip_server), int.Parse(ServerConstants.SERVER_PORT.ToString())));
            //serverSocket.Listen(100);

            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip_server), int.Parse(ServerConstants.SERVER_PORT.ToString()));
            var listener = new TcpListener(ipEndPoint);
            listener.Start(100);

            Client server = new Client(listener, new Service(new Repository()));

            Menu(server);
        }

        static async Task WriteAsync(NetworkCommunication communication)
        {
            while (true)
            {
                var msg = Console.ReadLine();
                var data = System.Text.Encoding.UTF8.GetBytes(msg);
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                await communication.WriteAsync(dataLength);
                await communication.WriteAsync(data);
            }
        }

        //public static async Task ReadAsync(NetworkCommunication communication)
        //{
        //    while (true)
        //    {
        //        byte[] dataLength = await communication.ReadAsync(ProtocolConstants.FixedDataSize);
        //        int dataSize = BitConverter.ToInt32(dataLength, 0);
        //        byte[] data = await communication.ReadAsync(dataSize);
        //        var msg = System.Text.Encoding.UTF8.GetString(data);
        //        Console.WriteLine(msg);
        //    }
        //}

        private static void Menu(Client server)
        {
            string[] options = new string[] { 
                "LISTAR CLIENTES CONECTADOS", 
                "ADMINISTRAR CLIENTES", 
                "APAGAR SERVIDOR" 
            };
            ConsoleValidations.ListOperations($"{"ADMINISTRACIÓN DEL SERVIDOR"}\n\nMENÚ PRINCIPAL:", options, true);

            string option = ConsoleValidations.ReadUntilValid( prompt: "\nIngrese opción",
                                            pattern: $"^[1-{options.Length}]$",
                                            errorMsg: $"Ingrese un número entre 1 y {options.Length}");

            ExecuteItemMenu(option, server);
            Console.ReadLine();
        }

        private static void ExecuteItemMenu(string option, Client server)
        {
            switch (option)
            {
                case "1":
                    //ListSelectedOption(server, client,"CLIENTES CONECTADOS", server.GetConnectedClients());
                    break;
                case "2":
                    CrudClient(server);
                    break;
                case "3":
                    Environment.Exit(Environment.ExitCode);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }

        private static void ListSelectedOption(Client server, string title, string[] list)
        {
            ConsoleValidations.ListOperations($"{"ADMINISTRACIÓN DEL SERVIDOR"}\n\n{title}:", list, true);
            string option = ConsoleValidations.ReadUntilValid(prompt: "\nPresione <<q>> para volver al menú principal",
                                           pattern: $"^[q|Q]$",
                                           errorMsg: $"Opción no válida");

            if (option.ToLower() == "q")
            {
                Menu(server);
            }
        }

        public static string ReadUntilIsNotEmpty(string input)
        {
            while (input.Trim() == string.Empty)
            {
                Console.WriteLine("No puede ingresar valores vacíos, ingrese nuevamente >>");
                input = Console.ReadLine();
            }

            return input;
        }

        private static void CreateClient(Client server)
        {
            Console.Clear();
            Console.WriteLine("ADMINISTRACIÓN DEL SERVIDOR" + "\n\nCREACIÓN DE CLIENTE");

            Console.WriteLine("\nIngrese el nombre del nuevo cliente (<<q>> para cancelar) >>");
            string name = ReadUntilIsNotEmpty(Console.ReadLine());
            Exit(name, server);

            Console.WriteLine("\nIngrese el email del nuevo cliente (<<q>> para cancelar) >>");
            string email = ReadUntilIsNotEmpty(Console.ReadLine());
            Exit(email, server);

            var client = new User
            {
                Name = name,
                Email = email
            };
            server.Service.AddUser(client);

            Console.WriteLine("\nPresione cualquier tecla para volver al Menú Principal >>");
            Console.ReadLine();

            Menu(server);
        }

        private static void CrudClient(Client server)
        {
  
                //Console.WriteLine(validation_message);
                //Console.WriteLine("Presione cualquier tecla para volver al menu");
                //string go_back = Console.ReadLine();
                //Menu(server);

            string[] menu_options = new string[] {"Crear un cliente",
                                                  "Borrar un cliente",
                                                  "Modificar un cliente existente",
                                                  "Volver" };

            ConsoleValidations.ListOperations("\nMENU CLIENTE:", menu_options, false);

            string option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese opción",
                                            pattern: $"^[1-{menu_options.Length}]$",
                                            errorMsg: $"Ingrese un número entre 1 y {menu_options.Length}");
            switch (option)
            {
                case "1":
                    CreateClient(server);
                    break;
                case "2":
                    DeleteClient(server);
                    break;
                case "3":
                    //ModifyClient(server);
                    break;
                case "4":
                    Menu(server);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }

        private static void Exit(string input, Client server)
        {
            if (input.ToLower() == "q")
            {
                Menu(server);
            }
        }

        public static string ReadValidRange(string input, int min_value, int max_value)
        {
            if (input.Trim().ToLower() == "q")
            {
                return input.Trim();
            }

            bool isInt = false;
            int option = int.MinValue;

            while (!isInt || option < min_value || option > max_value)
            {
                Console.WriteLine($"\nIngrese una opción entre {min_value} y {max_value}");
                input = Console.ReadLine();
                isInt = int.TryParse(input, out option);
            }

            return input;
        }

        private static void DeleteClient(Client server)
        {
            var clients = server.Service.GetAllClients();
            string[] options = clients.Select(g => g.ToString()).ToArray();

            ConsoleValidations.ListOperations("\nClientes disponibles:", options, false); ;
            string option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese número de cliente",
                                                              pattern: $"^[1-9]|10$",
                                                              errorMsg: $"Ingrese un número entre 1 y {options.Length}");

            
            var clientToDelete = clients.ElementAt(int.Parse(option) - 1);
            server.Service.DeleteUser(clientToDelete);

            Console.WriteLine("Presione cualqier tecla para volver al menu");
            Console.ReadLine();

            Menu(server);
        }

        private static List<User> ListClients(string server_clients)
        {
            string[] one_by_one = server_clients.Split('$');
            one_by_one = one_by_one.Take(one_by_one.Length - 1).ToArray();
            List<User> clients = new List<User>();
            foreach (string client in one_by_one)
            {
                clients.Add(new User(client.Split('#')));
            }
            return clients;
        }

        private static List<User> ListClient(string clientsFromServer)
        {
            string[] one_by_one = clientsFromServer.Split('$');
            one_by_one = one_by_one.Take(one_by_one.Length - 1).ToArray();
            List<User> clients = new List<User>();
            foreach (string client in one_by_one)
            {
                clients.Add(new User(client.Split('#')));
            }
            return clients;
        }

        private static void ModifyClient(Client server)
        {
        //    var clients = server.Service.GetAllClients();
        //    string[] options = clients.Select(g => g.ToString()).ToArray();

        //    ListOperations("\nClientes disponibles:", options, false); ;
        //    string option = ReadUntilValid(prompt: "\nIngrese número de cliente",
        //                                   pattern: $"^[1-9]|10$",
        //                                   errorMsg: $"Ingrese un número entre 1 y {options.Length}");


        //    var clientToModify = clients.ElementAt(int.Parse(option) - 1);
        //    server.Service.(clientToDelete);

        //    Console.WriteLine("Presione cualqier tecla para volver al menu");
        //    Console.ReadLine();

        //    Menu(server);

        //    data_to_send += listGenres.ElementAt(int.Parse(option) - 1).Id;
        //    data_to_send += "#Name=";
        //    Console.WriteLine("Ingrese el nombre para actualizar");
        //    data_to_send += Console.ReadLine();
        //    data_to_send += "#Description=";
        //    Console.WriteLine("Ingrese la descripcion para actualizar");
        //    data_to_send += Console.ReadLine();

        //    protocol.SendDataString(data_to_send);

        //    string message = protocol.RecieveDataString();
        //    Console.WriteLine(message);

        //    Console.WriteLine("Presione cualqier tecla para volver al menu");
        //    string back = Console.ReadLine();
           Menu(server);
        }

    }
}
