using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TCPComm.Constants;
using TCPComm.Protocol;
using Utils;

namespace FileClient
{
    public class ClientProgram
    {
        public static async Task Main(string[] args)
        {
            Console.Clear();
            Console.Title = "InstaPhoto";
            Console.WriteLine("InstaPhoto\n\nPresione una tecla para continuar...\n");

            Console.ReadLine();

            var ipServer = ConsoleValidations.PromptIPsAvailablesOnPC("InstaPhoto");
            
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ipServer), int.Parse(ServerConstants.SERVER_PORT.ToString()));
            var client = new TcpClient(ipEndPoint);
            var stream = client.GetStream();
            var communication = new StreamCommunication(stream);
            
            EntryMenu(communication);
        }
        
        private static void EntryMenu(StreamCommunication communication)
        {
            while (true)
            {
                var options = new string[] { 
                    "Registrase", 
                    "Login", 
                    "Desconectarse" 
                };
                ConsoleValidations.ListOperations($"{"InstaPhoto"}\n\nMENÚ PRINCIPAL:", options, true);

                var option = ConsoleValidations.ReadUntilValid( prompt: "\nIngrese opción",
                    pattern: $"^[1-{options.Length}]$",
                    errorMsg: $"Ingrese un número entre 1 y {options.Length}");

                ExecuteItemEntryMenu(option, communication);
                Console.ReadLine();
            }
        }

        private static void ExecuteItemEntryMenu(string option, StreamCommunication communication)
        {
            switch (option)
            {
                case "1":
                    CreateUser(communication);
                    break;
                case "2":
                    LoginUser(communication);
                    break;
                case "3":
                    Environment.Exit(Environment.ExitCode);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }
        
        private static void CreateUser(StreamCommunication communication)
        {
            throw new NotImplementedException();
        }

        private static void LoginUser(StreamCommunication communication)
        {
            var result = ClientHandler.HandleLogin(communication);
            
        }

        private static void MainMenu(StreamCommunication communication)
        {
            var options = new string[] { 
                "Cargar Foto", 
                "Listado de Usuarios", 
                "Ver Comentarios" ,
                "Agregar Comentarios",
                "Cerrar Sesion"
            };
            
            ConsoleValidations.ListOperations($"{"InstaPhoto"}\n\nMENÚ PRINCIPAL:", options, true);

            var option = ConsoleValidations.ReadUntilValid( prompt: "\nIngrese opción",
                pattern: $"^[1-{options.Length}]$",
                errorMsg: $"Ingrese un número entre 1 y {options.Length}");

            ExecuteItemMainMenu(option, communication);
            Console.ReadLine();
        }

        private static void ExecuteItemMainMenu(string option, StreamCommunication communication)
        {
            switch (option)
            {
                case "1":
                    UploadPhoto(communication);
                    break;
                case "2":
                    ListUsers(communication);
                    break;
                case "3":
                    ViewComments(communication);
                    break;
                case "4":
                    AddComments(communication);
                    break;
                case "5":
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }

        private static void AddComments(StreamCommunication communication)
        {
            throw new NotImplementedException();
        }

        private static void ViewComments(StreamCommunication communication)
        {
            throw new NotImplementedException();
        }

        private static void ListUsers(StreamCommunication communication)
        {
            throw new NotImplementedException();
        }

        private static void UploadPhoto(StreamCommunication communication)
        {
            throw new NotImplementedException();
        }
    }
}