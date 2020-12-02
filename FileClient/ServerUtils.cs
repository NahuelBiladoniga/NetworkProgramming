using System;

namespace FileClient
{
    public static class ClientUtils
    {
        public static string ReadUntilIsNotEmpty(string input)
        {
            while (input != null && input.Trim() == string.Empty)
            {
                Console.WriteLine("No puede ingresar valores vacíos, ingrese nuevamente >>");
                input = Console.ReadLine();
            }

            return input;
        }
    }
}