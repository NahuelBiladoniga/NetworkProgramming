using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using TCPComm.Protocol;
using Utils;

namespace Server
{
    public static class AdminHandler
    {
        public static void CreateUser(Server server)
        {
            Console.Clear();
            Console.WriteLine("ADMINISTRACIÓN DEL SERVIDOR" + "\n\nCREACIÓN DE CLIENTE");

            Console.WriteLine("\nIngrese el nombre del nuevo cliente (<<q>> para cancelar) >>");
            string name = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            Console.WriteLine("\nIngrese el email del nuevo cliente (<<q>> para cancelar) >>");
            string email = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());

            var client = new User
            {
                Name = name,
                Email = email
            };
            server.Service.AddUser(client);
        }

        public static void DeleteUser(Server server)
        {
            var clients = server.Service.GetAllClients();
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

            
            var clientToDelete = clients.ElementAt(int.Parse(option) - 1);
            server.Service.DeleteUser(clientToDelete);
        }

        public static void ModifyUser(Server server)
        {
            var clients = server.Service.GetAllClients();
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
            
            userToUpdate.Name = name;
            
            server.Service.UpdateUser(userToUpdate);

        }
    }
}