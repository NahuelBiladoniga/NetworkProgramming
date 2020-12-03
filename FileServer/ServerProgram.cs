using System;
using System.Net;
using System.Net.Sockets;
using Repositories;
using TCPComm.Constants;
using Utils;

namespace FileServer
{
    public class ServerProgram
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.Title = "ADMINISTRACIÓN DEL SERVIDOR";
            Console.WriteLine("ADMINISTRACIÓN DEL SERVIDOR\n\nPresione una tecla para continuar...\n");

            Console.ReadLine();

            var ipServer = ConsoleValidations.PromptIPsAvailablesOnPC("ADMINISTRACIÓN DEL SERVIDOR");
            
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ipServer), int.Parse(ServerConstants.SERVER_PORT.ToString()));
            var listener = new TcpListener(ipEndPoint);
            listener.Start(100);

            var server = new FileServer.Server(listener, new Service(new Repository()));

            //var log = new Log();
            //Console.WriteLine("Enter Log Level:");
            //log.Level = Console.ReadLine();
            //Console.WriteLine("Enter message:");
            //log.Message = Console.ReadLine();
            //var stringLog = JsonSerializer.Serialize(log);
            //var result = await SendMessage(channel, stringLog);

            Menu(server);
        }
        
        private static void Menu(FileServer.Server server)
        {
            while (true)
            {
                string[] options = new string[] { 
                    "LISTAR CLIENTES CONECTADOS", 
                    "ADMINISTRAR CLIENTES", 
                    "APAGAR SERVIDOR" 
                };
                ConsoleValidations.ListOperations($"{"ADMINISTRACIÓN DEL SERVIDOR"}\n\nMENÚ PRINCIPAL:", options, true);

                var option = ConsoleValidations.ReadUntilValid( prompt: "\nIngrese opción",
                    pattern: $"^[1-{options.Length}]$",
                    errorMsg: $"Ingrese un número entre 1 y {options.Length}");

                ExecuteItemMenu(option, server);

                ConsoleValidations.ContinueHandler();
            }
        }

        private static void ExecuteItemMenu(string option, FileServer.Server server)
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
                    Environment.Exit(Environment.ExitCode);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }

        private static void ListSelectedOption(FileServer.Server server, string title, string[] list)
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
        
        private static void CrudClient(FileServer.Server server)
        {
            var menu_options = new []{
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
                    AdminHandler.CreateUser(server);
                    break;
                case "2":
                    AdminHandler.DeleteUser(server);
                    break;
                case "3":
                    AdminHandler.ModifyUser(server);
                    break;
                case "4":
                    Menu(server);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }
        private static void Exit(string input, FileServer.Server server)
        {
            if (input.ToLower() == "q")
            {
                Menu(server);
            }
        }
    }
}