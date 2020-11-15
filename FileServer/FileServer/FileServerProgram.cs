using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using TCPComm.Protocol;
using System.Linq;
using System.Configuration;

namespace Controller.FileServer
{
    public class FileServerProgram
    {
        static TcpListener server;

        static async Task Main(string[] args)
        {
            Console.Clear();
            Console.Title = "ADMINISTRACION DEL SERVIDOR";
            Console.WriteLine($"ADMINISTRACION DEL SERVIDOR\n\nPresione una tecla para continuar...\n");

            Console.ReadLine();

            string ip_server = AvailableIp("ADMINISTRACION DEL SERVIDOR");
            Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket conection;
            IPEndPoint connect = new IPEndPoint(IPAddress.Parse(ip_server),GetPort("ServerPort"));

            listen.Bind(connect);
            listen.Listen(10);

            conection = listen.Accept();
            FileServer file_server = new FileServer(serverSocket, new Service(new Repository()));

           // Console.ReadKey();
        }


        public static string AvailableIp(string title)
        {
            Console.Clear();
            Console.WriteLine($"{title}\n\nIPS DISPONIBLES:\n");
            int i = 0;

            string[] available_ips = GetIPs().ToArray();
            Array.ForEach(available_ips,
                          ip => Console.Write("{0} - {1}\n", ++i, ip));

            var options = ReadValid(
                            prompt: "\nSeleccione una IP",
                            pattern: $"^[1-{i}]$",
                            err: $"Ingrese un número entre 1 y {i}.");

            return available_ips[int.Parse(options) - 1];
        }

        private static IEnumerable<string> GetIPs()
        {
            IPHostEntry ip_info = Dns.GetHostEntry(Dns.GetHostName());
            IEnumerable<string> ip_addresses = ip_info.AddressList
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

        public static string ReadValid(string prompt, string pattern, string err)
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
                Console.WriteLine(err);
                return ReadValid(prompt, pattern, err);
            }
        }

        public static string ReadNonEmpty(string input)
        {
            while (input.Trim() == string.Empty)
            {
                Console.WriteLine("No se permite ingresar valores vacíos, ingrese nuevamente >>");
                input = Console.ReadLine();
            }

            return input;
        }

        private static int GetPort(string key)
        {
            string value = ConfigurationManager.AppSettings[key].ToString();

            int port;
            bool is_integer = int.TryParse(value, out port);

            return is_integer ? port : 0;
        }
    }
}
