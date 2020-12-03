using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using RepositoryClient.Dto;
using Utils;

namespace FileServer
{
    public static class AdminHandler
    {
        public static async Task CreateUser(FileServer.Server server)
        {
            Console.Clear();
            Console.WriteLine("ADMINISTRACIÓN DEL SERVIDOR" + "\n\nCREACIÓN DE CLIENTE");

            Console.WriteLine("\nIngrese el nombre del nuevo cliente (<<q>> para cancelar) >>");
            var name = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            Console.WriteLine("\nIngrese el email del nuevo cliente (<<q>> para cancelar) >>");
            var email = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            var client = new UserDto
            {
                Name = name,
                Email = email
            };
            
            await server.Service.AddUserAsync(client);
        }

        public static async Task DeleteUser(FileServer.Server server)
        {
            var clients = await server.Service.GetUsersAsync();
            if (!clients.Any())
            {
                Console.WriteLine("\nNo hay usuarios en el sistema");
                return;
            }

            var options = clients.Select(g => g.ToString()).ToArray();

            ConsoleValidations.ListOperations("\nClientes disponibles:", options, false); ;
            var option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese número de cliente",
                pattern: $"^[1-9]|10$",
                errorMsg: $"Ingrese un número entre 1 y {options.Length}");

            
            var clientToDelete = clients.ElementAt(int.Parse(option) - 1);
            server.Service.RemoveUserAsync(clientToDelete);
        }

        public static async Task ModifyUser(FileServer.Server server)
        {
            var clients = (await server.Service.GetUsersAsync()).ToList();
            if (!clients.Any())
            {
                Console.WriteLine("\nNo hay usuarios en el sistema");
                return;
            }
            string[] options = clients.Select(g => g.ToString()).ToArray();

            ConsoleValidations.ListOperations("\nClientes disponibles:", options, false); ;
            string option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese número de cliente",
                pattern: $"^[1-9]|10$",
                errorMsg: $"Ingrese un número entre 1 y {options.Length}");

            
            var userToUpdate = clients.ElementAt(int.Parse(option) - 1);
            
            Console.WriteLine("\nIngrese el nuevo nombre del cliente (<<q>> para cancelar) >>");
            string name = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());
            
            Console.WriteLine("\nIngrese el nuevo contraseia del cliente (<<q>> para cancelar) >>");
            string password = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            userToUpdate.Name = name;
            userToUpdate.Password = password;

            var dto = new UserDto()
            {
                Name = userToUpdate.Name,
                Password = userToUpdate.Password,
                Email = userToUpdate.Email
            };
            
            await server.Service.ModifyUserAsync(userToUpdate);
        }
    }
}