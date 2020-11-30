using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using TCPComm.Protocol;

namespace Server
{
    public static class UserHandler
    {
        public static void CreateUser(Server server)
        {
            Console.Clear();
            Console.WriteLine("ADMINISTRACIÓN DEL SERVIDOR" + "\n\nCREACIÓN DE CLIENTE");

            Console.WriteLine("\nIngrese el nombre del nuevo cliente (<<q>> para cancelar) >>");
            string name = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());
            // Exit(name, server);

            Console.WriteLine("\nIngrese el email del nuevo cliente (<<q>> para cancelar) >>");
            string email = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());
            // Exit(email, server);

            var client = new User
            {
                Name = name,
                Email = email
            };
            server.Service.AddUser(client);

            // Console.WriteLine("\nPresione cualquier tecla para volver al Menú Principal >>");
            // Console.ReadLine();
            //
            // Menu(server);
        }
        
                
        public static List<User> ListUsers(string server_clients)
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
        
        public static void DeleteUser(Server server)
        {
            var clients = server.Service.GetAllClients();
            string[] options = clients.Select(g => g.ToString()).ToArray();

            ConsoleValidations.ListOperations("\nClientes disponibles:", options, false); ;
            string option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese número de cliente",
                pattern: $"^[1-9]|10$",
                errorMsg: $"Ingrese un número entre 1 y {options.Length}");

            
            var clientToDelete = clients.ElementAt(int.Parse(option) - 1);
            server.Service.DeleteUser(clientToDelete);

            // Console.WriteLine("Presione cualqier tecla para volver al menu");
            // Console.ReadLine();
            //
            // Menu(server);
        }

        public static void ModifyUser(Server server)
        {
            var clients = server.Service.GetAllClients();
            string[] options = clients.Select(g => g.ToString()).ToArray();

            ConsoleValidations.ListOperations("\nClientes disponibles:", options, false); ;
            string option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese número de cliente",
                pattern: $"^[1-9]|10$",
                errorMsg: $"Ingrese un número entre 1 y {options.Length}");

            
            var userToUpdate = clients.ElementAt(int.Parse(option) - 1);
            
            Console.WriteLine("\nIngrese el nuevo nombre del cliente (<<q>> para cancelar) >>");
            string name = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());
            // Exit(name, server);
            
            userToUpdate.Name = name;
            
            server.Service.UpdateUser(userToUpdate);

            // Console.WriteLine("Presione cualqier tecla para volver al menu");
            // Console.ReadLine();
            //
            // Menu(server);
        }
    }
}