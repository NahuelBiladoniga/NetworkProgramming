using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using DataAccess;
using Domain;
using TCPComm.Constants;
using TCPComm.Protocol;

namespace Server
{
    public class ServerProgram
    {
        public static async Task Main(string[] args)
        {
            Console.Clear();
            Console.Title = "ADMINISTRACIÓN DEL SERVIDOR";
            Console.WriteLine("ADMINISTRACIÓN DEL SERVIDOR\n\nPresione una tecla para continuar...\n");

            Console.ReadLine();

            var ipServer = ConsoleValidations.PromptIPsAvailablesOnPC("ADMINISTRACIÓN DEL SERVIDOR");
            
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ipServer), int.Parse(ServerConstants.SERVER_PORT.ToString()));
            var listener = new TcpListener(ipEndPoint);
            listener.Start(100);

            var server = new Server(listener, new Service(new Repository()));

            Menu(server);
        }

        static async Task WriteAsync(NetworkCommunication communication)
        {
            while (true)
            {
                var msg = Console.ReadLine();
                var data = System.Text.Encoding.UTF8.GetBytes(msg);
                var dataLength = BitConverter.GetBytes(data.Length);
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

        private static void Menu(Server server)
        {
            string[] options = new string[] { 
                "LISTAR CLIENTES CONECTADOS", 
                "ADMINISTRAR CLIENTES", 
                "Administrar Fotos",
                "APAGAR SERVIDOR" 
            };
            ConsoleValidations.ListOperations($"{"ADMINISTRACIÓN DEL SERVIDOR"}\n\nMENÚ PRINCIPAL:", options, true);

            string option = ConsoleValidations.ReadUntilValid( prompt: "\nIngrese opción",
                pattern: $"^[1-{options.Length}]$",
                errorMsg: $"Ingrese un número entre 1 y {options.Length}");

            ExecuteItemMenu(option, server);
            Console.ReadLine();
        }

        private static void ExecuteItemMenu(string option, Server server)
        {
            switch (option)
            {
                case "1":
                    ListSelectedOption(server,"CLIENTES CONECTADOS", server.GetConnectedClients());
                    break;
                case "2":
                    CrudClient(server);
                    break;
                case "3":
                    PhotosHandler(server);
                    break;
                case "4":
                    Environment.Exit(Environment.ExitCode);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }

        private static void ListSelectedOption(Server server, string title, string[] list)
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
        
        private static void PhotosHandler(Server server)
        {
            bool userAutenticated;
            do
            {
                Console.WriteLine("\nIngrese el email (<<q>> para cancelar) >>");
                string email = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());
                Exit(email, server);

                Console.WriteLine("\nIngrese la contrasenia (<<q>> para cancelar) >>");
                string password = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());
                Exit(password, server);

                var user = new User
                {
                    Email = email,
                    Password = password
                };

                userAutenticated = server.Service.AutenticateUser(user);
            } while (!userAutenticated);

            string[] menu_options = new string[] {
                "Cargar Foto",
                "Listar Foto",
                "Comentar Foto",
                "Volver" 
            };

            ConsoleValidations.ListOperations("\nMENU Fotos:", menu_options, false);

            string option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese opción",
                pattern: $"^[1-{menu_options.Length}]$",
                errorMsg: $"Ingrese un número entre 1 y {menu_options.Length}");
            switch (option)
            {
                case "1":
                    ImageHandler.UploadPhoto(server);
                    break;
                case "2":
                    ImageHandler.ListPhotos(server);
                    break;
                case "3":
                    ImageHandler.CommentPhoto(server);
                    break;
                case "4":
                    Menu(server);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }
        
        private static void CrudClient(Server server)
        {
            var menu_options = new string[] {
                "Crear un cliente",
                "Borrar un cliente",
                "Modificar un cliente existente",
                "Volver" 
            };

            ConsoleValidations.ListOperations("\nMENU CLIENTE:", menu_options, false);

            string option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese opción",
                pattern: $"^[1-{menu_options.Length}]$",
                errorMsg: $"Ingrese un número entre 1 y {menu_options.Length}");
            switch (option)
            {
                case "1":
                    UserHandler.CreateUser(server);
                    break;
                case "2":
                    UserHandler.DeleteUser(server);
                    break;
                case "3":
                    UserHandler.ModifyUser(server);
                    break;
                case "4":
                    Menu(server);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }

        private static void Exit(string input, Server server)
        {
            if (input.ToLower() == "q")
            {
                Menu(server);
            }
        }

        // public static string ReadValidRange(string input, int min_value, int max_value)
        // {
        //     if (input.Trim().ToLower() == "q")
        //     {
        //         return input.Trim();
        //     }
        //
        //     bool isInt = false;
        //     int option = int.MinValue;
        //
        //     while (!isInt || option < min_value || option > max_value)
        //     {
        //         Console.WriteLine($"\nIngrese una opción entre {min_value} y {max_value}");
        //         input = Console.ReadLine();
        //         isInt = int.TryParse(input, out option);
        //     }
        //
        //     return input;
        // }
    }
}