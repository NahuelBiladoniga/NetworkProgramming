using System;

namespace Server
{
    public static class ServerUtils
    {
        public static string ReadUntilIsNotEmpty(string input)
        {
            while (input.Trim() == string.Empty)
            {
                Console.WriteLine("No puede ingresar valores vacíos, ingrese nuevamente >>");
                input = Console.ReadLine();
            }

            return input;
        }
    }
}