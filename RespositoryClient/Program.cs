using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using RepositoryServer;

namespace RespositoryClient
{
    class Program
    {
        static void Main(string[] args)
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
            Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

            Console.WriteLine("Type name to greet or type exit to stop sending greetings");
            string nameToGreet = string.Empty;

            var helloRequest = new CreateUserInput()
            {
                Name = "nameToGreet",
                Email = "test",
                Password = "testt123"
            };

            var response = client.CreateUser(helloRequest);
            Console.WriteLine(response.Code);
        }
    }
}
