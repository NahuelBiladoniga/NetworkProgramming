using System;
using System.Linq;
using Domain;
using TCPComm.Protocol;

namespace Server
{
    public static class ImageHandler
    {
        public static void UploadPhoto(Server server)
        {
            throw new NotImplementedException();
        }
        
        public static void CommentPhoto(Server server)
        {
            // var photos = server.Service.GetPhotosFromUser();
            // var options = photos.Select(g => g.ToString()).ToArray();
            //
            // ConsoleValidations.ListOperations("\nFotos disponibles:", options, false); ;
            // var option = ConsoleValidations.ReadUntilValid(prompt: "\nIngrese id de foto",
            //     pattern: $"^[1-9]|10$",
            //     errorMsg: $"Ingrese un número entre 1 y {options.Length}");
            //
            // Console.WriteLine("\nIngrese el comentario para la foto (<<q>> para cancelar) >>");
            //
            // string comment = ServerUtils.ReadUntilIsNotEmpty(Console.ReadLine());
            //
            // var photoSelected = photos.ElementAt(int.Parse(option) - 1);
            //
            // var commentEntity = new Comment
            // {
            //     Photo = photoSelected,
            //     Commentor = server.UserLogged,
            //     Message = comment
            // };
            //
            // server.Service.CommentPhoto(commentEntity);

            // Console.WriteLine("\nPresione cualquier tecla para volver al Menú Principal >>");
            // Console.ReadLine();
            //
            // Menu(server);
        }

        public static void ListPhotos(Server server)
        {
            // var photos = server.Service.GetPhotosFromUser();
            // string[] options = photos.Select(g => g.ToString()).ToArray();
            //
            // ConsoleValidations.ListOperations("\nFotos disponibles:", options, false); ;

            // Console.WriteLine("\nPresione cualquier tecla para volver al Menú Principal >>");
            // Console.ReadLine();
            //
            // Menu(server);
        }
    }
}