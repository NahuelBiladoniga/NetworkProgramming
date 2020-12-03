// using System;
// using System.Collections.Generic;
// using Domain;
// using Repositories.Interfaces;
// using Services.Interfaces;
// using TCPComm;
//
// namespace FileServer
// {
//     public class Service : IService
//     {
//         private IRepository _repository;
//         
//         public Service(IRepository repository)
//         {
//             _repository = repository;
//         }
//
//         public List<User> GetAllClients()
//         {
//             return _repository.GetUsers();
//         }
//
//         public List<User> GetConnectedClients()
//         {
//             throw new NotImplementedException();
//         }
//
//         public List<Photo> GetPhotosFromUser(User user)
//         {
//             return _repository.GetPhotosFromUser(user);
//         }
//
//         public List<Comment> GetCommentsFromPhoto(Photo id)
//         {
//             return _repository.GetCommentsFromPhoto(id);
//         }
//
//         public void AddUser(User user)
//         {
//             _repository.AddUser(user);
//         }
//
//         public void DeleteUser(User user)
//         {
//             _repository.DeleteUser(user);
//         }
//
//         public void UpdateUser(User user)
//         {
//             _repository.UpdateUser(user);
//         }
//
//         public void DisconnectUser(User user)
//         {
//             _repository.DisconnectUser(user);
//         }
//
//         public int GetClientSize(){
//             return _repository.GetUsers().Count;
//         }
//
//         public void AddClient(CommunicationClient client)
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool ContainsUser(User user)
//         {
//             return _repository.GetUsers().Contains(user);
//         }
//
//         public bool AutenticateUser(User user)
//         {
//             var savedUser = _repository.GetUser(user);
//             return savedUser.IsUserValid(user);
//         }
//         
//         public void CommentPhoto(Comment comment)
//         {
//             _repository.CommentPhoto(comment);
//         }
//
//         public void UploadPhoto(Photo photo)
//         {
//             _repository.UploadPhoto(photo);
//         }
//
//         public List<Photo> GetPhotos()
//         {
//             return _repository.GetPhotos();
//         }
//     }
// }
