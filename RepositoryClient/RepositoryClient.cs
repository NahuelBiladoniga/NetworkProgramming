using Grpc.Net.Client;
using RepositoryService;
using System;
using System.Threading.Tasks;

namespace RepositoryClient
{
    public class RepositoryClient
    {
        static async Task Main(string[] args)
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
            UserHandler.UserHandlerClient client = new UserHandler.UserHandlerClient(channel);


            var request = new AddUserInput
            {
                Email = "asda",
                Password = "asda",
                Name = "asdasd",
            };

            var response = await client.AddUserAsync(request);
            Console.WriteLine(response.Message);

        }
    }
}