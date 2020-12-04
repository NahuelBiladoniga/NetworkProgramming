using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Domain;
using TCPComm.Protocol;
using Utils;

namespace FileClient
{
    public static class ClientProgram
    {
        private static Client _client;
        public static async Task Main(string[] args)
        {
            Console.Clear();
            Console.Title = "InstaPhoto";
            Console.WriteLine("InstaPhoto\n\nPresione una tecla para continuar...\n");
            Console.ReadLine();

            var clientIp = ConsoleValidations.PromptIPsAvailablesOnPC("InstaPhoto");
            
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(clientIp), 0);
            var client = new TcpClient(ipEndPoint);

            Console.WriteLine("\nIndique ip del serivdor:\n");
            var serverIp = Console.ReadLine();
            Console.WriteLine("\nIndique puerto del serivdor:\n");
            var port = Console.ReadLine();
            
            var serverIpEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), int.Parse(port));
            client.Connect(serverIpEndPoint);
            var stream = client.GetStream();

            _client = new Client()
            {
                StreamCommunication = new StreamCommunication(stream)
            };
            
            await EntryMenu();
        }
        
        private static async Task EntryMenu()
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

                await ExecuteItemEntryMenu(option);
            }
        }

        private static async Task ExecuteItemEntryMenu(string option)
        {
            switch (option)
            {
                case "1":
                    await CreateUser();
                    break;
                case "2":
                    await LoginUser();
                    break;
                case "3":
                    Environment.Exit(Environment.ExitCode);
                    break;
                default:
                    throw new Exception($"Option: {option} is not a valid option.");
            }
        }
        
        private static async Task CreateUser()
        {
            Console.Clear();
            Console.WriteLine("InstaPhoto" + "\nLogin");

            Console.WriteLine("\nIngrese nombre");
            var name = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            Console.WriteLine("\nIngrese email");
            var email = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            Console.WriteLine("\nIngrese contrasenia");
            var password = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            var user = new User
            {
                Name = name,
                Email = email,
                Password = password
            };
            
            await ServerHandler.HandleRegister(_client,user);
            _client.IsLoggedIn = true;
            await MainMenu();
        }

        private static async Task LoginUser()
        {
            Console.Clear();
            Console.WriteLine("InstaPhoto" + "\nLogin");

            Console.WriteLine("\nIngrese email (<<q>> para cancelar) >>");
            var email = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            Console.WriteLine("\nIngrese contrasenia (<<q>> para cancelar) >>");
            var password = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            var user = new User
            {
                Email = email,
                Password = password
            };
            
            var response = await ServerHandler.HandleLogin(_client,user);

            if (response.responseCommands == ProtocolConstants.ResponseCommands.Ok)
            {
                Console.WriteLine("\nBienvenido!");
                _client.IsLoggedIn = true;
                await MainMenu();
            }
            else
            {
                Console.WriteLine("\nDatos invalidos, revise y pruebe de nuevo");

                ConsoleValidations.ContinueHandler();
            }
        }

        private static async Task MainMenu()
        {
            var options = new string[] {
                "Cargar Foto",
                "Listado de Usuarios",
                "Ver Comentarios" ,
                "Ver Fotos",
                "Agregar Comentarios",
                "Salir"
            };

            while (_client.IsLoggedIn)
            {
                Console.Clear();
                ConsoleValidations.ListOperations($"{"InstaPhoto"}\n\nMENÚ PRINCIPAL:", options, true);

                var option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese opción",
                    pattern: $"^[1-{options.Length}]$",
                    errorMsg: $"Ingrese un número entre 1 y {options.Length}");

                await ExecuteItemMainMenu(option);                
            }

            ConsoleValidations.ContinueHandler();
        }

        private static async Task ExecuteItemMainMenu(string option)
        {
            switch (option)
            {
                case "1":
                    await UploadPhotoAsync();
                    break;
                case "2":
                    await ViewUsers();
                    break;
                case "3":
                    await ViewComments();
                    break; 
                case "4":
                    await ViewPhotos();
                    break;
                case "5":
                    await AddCommentsAsync();
                    break;
                case "6":
                    Environment.Exit(Environment.ExitCode);
                    break;
                default:
                    throw new Exception($"Opcion: {option} no es valida.");
            }
        }

        private static async Task ViewPhotos()
        {
            var response = await ServerHandler.HandleViewPhotos(_client);

            foreach (var photo in response)
            {
                Console.WriteLine(photo.ToString());
            }
            
            ConsoleValidations.ContinueHandler();
        }

        private static async Task ViewUsers()
        {
            var response = await ServerHandler.HandleViewUsers(_client);

            if(response.Count == 0)
            {
                Console.WriteLine("No hay usuarios en el sistema");
            }
            else
            {
                foreach (var user in response)
                {
                    Console.WriteLine(user.ToString());
                }
            }

            ConsoleValidations.ContinueHandler();
        }

        private static async Task ViewComments()
        {
            Console.WriteLine("\nIngrese ID de foto");
            var photoId = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            var photo = new Photo()
            {
                Id = long.Parse(photoId)
            };

            var response = await ServerHandler.HandleViewComments(_client, photo);

            foreach (var comment in response)
            {
                Console.WriteLine(comment.ToString());
            }
            
            ConsoleValidations.ContinueHandler();
        }

        private static async Task AddCommentsAsync()
        {
            Console.Clear();
            Console.WriteLine("InstaPhoto" + "\nSubir Foto");

            await ViewPhotos();

            Console.WriteLine("\nIngrese ID de foto");
            var photoId = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            Console.WriteLine("\nIngrese comentario");
            var message = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            var comment = new Comment
            {
                Photo = new Photo()
                {
                    Id = long.Parse(photoId)
                },
                Message =message,
            };
            var response = await ServerHandler.HandleCommentCreation(_client, comment);

            if (response.responseCommands == ProtocolConstants.ResponseCommands.Ok)
            {
                Console.WriteLine("\nComentario Agregado Correctamente");
            }
            else
            {
                Console.WriteLine("\nDatos invalidos, revise y pruebe de nuevo");
            }

            ConsoleValidations.ContinueHandler();
        }

        private static async Task UploadPhotoAsync()
        {
            Console.Clear();
            Console.WriteLine("InstaPhoto" + "\nSubir Foto");

            Console.WriteLine("\nIngrese ruta de la foto");
            var filePath = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            var response = await ServerHandler.HandleImageUpload(_client, filePath);

            if (response.responseCommands == ProtocolConstants.ResponseCommands.Ok)
            {
                Console.WriteLine("\nImagen Agregada Correctamente");
            }
            else
            {
                Console.WriteLine("\nDatos invalidos, revise y pruebe de nuevo");
            }

            ConsoleValidations.ContinueHandler();
        }
    }
    
}