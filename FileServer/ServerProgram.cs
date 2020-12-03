﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TCPComm.Constants;
using Utils;

namespace FileServer
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

            var server = new Server(listener);

            //var log = new Log();
            //Console.WriteLine("Enter Log Level:");
            //log.Level = Console.ReadLine();
            //Console.WriteLine("Enter message:");
            //log.Message = Console.ReadLine();
            //var stringLog = JsonSerializer.Serialize(log);
            //var result = await SendMessage(channel, stringLog);

            await Menu(server);
        }
        
        private static async Task Menu(FileServer.Server server)
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

                await ExecuteItemMenu(option, server);

                ConsoleValidations.ContinueHandler();
            }
        }

        private static async Task ExecuteItemMenu(string option, FileServer.Server server)
        {
            switch (option)
            {
                case "1":
                    await ListSelectedOption(server,"CLIENTES CONECTADOS", await server.GetConnectedClients());
                    break;
                case "2":
                    await CrudClient(server);
                    break;
                case "3":
                    Environment.Exit(Environment.ExitCode);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }

        private static async Task ListSelectedOption(FileServer.Server server, string title, string[] list)
        {
            ConsoleValidations.ListOperations($"{"ADMINISTRACIÓN DEL SERVIDOR"}\n\n{title}:", list, true);
            string option = ConsoleValidations.ReadUntilValid(prompt: "\nPresione <<q>> para volver al menú principal",
                pattern: $"^[q|Q]$",
                errorMsg: $"Opción no válida");

            if (option.ToLower() == "q")
            {
                await Menu(server);
            }
        }
        
        private static async Task CrudClient(FileServer.Server server)
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
                    await Menu(server);
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