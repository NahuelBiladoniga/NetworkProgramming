using System;
using Domain;
using TCPComm.Protocol;

namespace FileClient
{
    public static class ServerHandler
    {
        public static void HandleLogin(StreamCommunication communication)
        {
            Console.Clear();
            Console.WriteLine("InstaPhoto" + "\nLogin");

            Console.WriteLine("\nIngrese email (<<q>> para cancelar) >>");
            var name = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());
            // Exit(name, server);

            Console.WriteLine("\nIngrese contrasenia (<<q>> para cancelar) >>");
            var email = ClientUtils.ReadUntilIsNotEmpty(Console.ReadLine());
            // Exit(email, server);

            
            
            // Console.WriteLine("\nPresione cualquier tecla para volver al Menú Principal >>");
            // Console.ReadLine();
            //
            // Menu(server);
        }
    }
}