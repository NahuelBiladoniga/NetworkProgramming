using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace TCPComm.Protocol
{
    public class ConsoleValidations
    {
        public static string PromptIPsAvailablesOnPC(string title)
        {
            Console.Clear();
            Console.WriteLine($"{title}\n\nIPS DISPONIBLES EN ESTE EQUIPO:\n");
            int i = 0;

            string[] availableIPs = GetIPs().ToArray();
            Array.ForEach(availableIPs,
                          ip => Console.Write("{0} - {1}\n", ++i, ip));

            var option = ReadUntilValid(
                            prompt: "\nSeleccione una IP",
                            pattern: $"^[1-{i}]$",
                            errorMsg: $"Ingrese un número entre 1 y {i}.");
            return availableIPs[int.Parse(option) - 1];
        }

        public static void ListOperations(string title, string[] options, bool clear)
        {
            if (clear) { Console.Clear(); };
            Console.WriteLine("{0}\n", title);
            int i = 0;
            Array.ForEach(options,
                          op => Console.Write("{0} - {1}\n", ++i, op));
        }

        public static string ReadUntilValid(string prompt, string pattern, string errorMsg)
        {
            var regex = new Regex(pattern);

            Console.Write($"{prompt} >> ");
            var ret = Console.ReadLine();

            if (regex.IsMatch(ret))
            {
                return ret;
            }
            else
            {
                Console.WriteLine(errorMsg);
                return ReadUntilValid(prompt, pattern, errorMsg);
            }
        }

        private static IEnumerable<string> GetIPs()
        {
            IPHostEntry ip_host_info = Dns.GetHostEntry(Dns.GetHostName());
            IEnumerable<string> ip_addresses = ip_host_info.AddressList
                .Where(i => i.AddressFamily == AddressFamily.InterNetwork)
                .Select(i => i.ToString());

            if (ip_addresses.Count() == 0)
            {
                return new List<string>() { "127.0.0.1" };
            }
            else
            {
                return ip_addresses;
            }
        }

    }
}
