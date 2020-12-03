using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RepositoryService.Interfaces;

namespace RepositoryService
{
    public class UserService : UserHandler.UserHandlerBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhotoRepository _photoRepository;

        public UserService(ILogger<UserService> logger, ICommentsRepository commentsRepository, IUserRepository userRepository, IPhotoRepository photoRepository)
        {
            _logger = logger;
            _commentsRepository = commentsRepository;
            _userRepository = userRepository;
            _photoRepository = photoRepository;
        }

        // rpc AddUser (AddUserInput) returns (ResponseMessage);
        // rpc RemoveUser (RemoveUserInput) returns (ResponseMessage);
        // rpc ModifyUser (ModifyUserInput) returns (ResponseMessage);
        // rpc UploadPhoto (UploadPhotoInput) returns (ResponseMessage);
        // rpc AddComment (AddCommentInput) returns (ResponseMessage);
        // rpc ViewPhotos (EmptyInput) returns (ViewPhotoResponse);
        // rpc ViewComments (ViewCommentInput) returns (ViewComments);
        // rpc ViewUsers (EmptyInput) returns (ViewUserResponse);

        public override Task<ResponseMessage> AddUser(AddUserInput request, ServerCallContext context)
        {
            var user = new User()
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password
            };

            if (_userRepository.ContainsUser(user))
            {

                return Task.FromResult(new ResponseMessage
                {
                    Status = "Error",
                    Message = "No se puede crear un usuario con el mismo mail",
                });
            }
            else
            {
                _userRepository.SaveUser(user);

                return Task.FromResult(new ResponseMessage
                {
                    Status = "Ok",
                    Message = "Usuario agregado correctamente",
                });
            }
        }

        public override Task<ResponseMessage> RemoveUser(RemoveUserInput request, ServerCallContext context)
        {
            var user = new User()
            {
                Email = request.Email,
            };

            if (_userRepository.ContainsUser(user))
            {

                return Task.FromResult(new ResponseMessage
                {
                    Status = "Ok",
                    Message = "Se ha borrado el usuario",
                });
            }
            else
            {
                _userRepository.SaveUser(user);

                return Task.FromResult(new ResponseMessage
                {
                    Status = "Error",
                    Message = "Usuario no existe",
                });
            }
        }

        public override Task<ResponseMessage> ModifyUser(ModifyUserInput request, ServerCallContext context)
        {
            var user = new User()
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password
            };

            if (_userRepository.ContainsUser(user))
            {
                _userRepository.UpdateUser(user);

                return Task.FromResult(new ResponseMessage
                {
                    Status = "Ok",
                    Message = "Datos modificados correctamente",
                });
            }
            else
            {

                return Task.FromResult(new ResponseMessage
                {
                    Status = "Error",
                    Message = "Usuario no existente",
                });
            }
        }
        public override Task<ResponseMessage> UploadPhoto(UploadPhotoInput request, ServerCallContext context)
        {
            var user = new User()
            {
                Email = request.UserEmail,
            };
            var photo = new Photo()
            {
                Name = request.Name,
                Extension = request.Extension,
                FileSize = request.Size,
                UserEmail = request.UserEmail
            };

            _photoRepository.UploadPhoto(user, photo);

            return Task.FromResult(new ResponseMessage
            {
                Status = "Ok",
                Message = "Imagen agregada satisfactoriamente",
            });
        }
        public override Task<ViewPhotoResponse> ViewPhotos(EmptyInput request, ServerCallContext context)
        {
            var photos = _photoRepository.GetPhotos();

            var result = new ViewPhotoResponse();
            photos.ForEach((e) =>
            {
                result.Results.Add(new PhotoResponse()
                {
                    Email = e.UserEmail,
                    NamePhoto = e.Name,
                    IdPhoto = e.Id.ToString(),
                    Extension = e.Extension,
                });
            });
            
            return Task.FromResult(result);
        }
        public override Task<ViewCommentResponse> ViewComments(ViewCommentInput request, ServerCallContext context)
        {
/*            var photos = _userRepository.GetTotalUsers();

            var result = new ViewPhotoResponse();
            photos.ForEach((e) =>
            {
                result.Results.Add(new PhotoResponse()
                {
                    Email = e.UserEmail,
                    NamePhoto = e.Name,
                    IdPhoto = e.Id.ToString(),
                    Extension = e.Extension,
                });
            });

            return Task.FromResult(result);
*/        }
        public override Task<ViewUserResponse> ViewUsers(EmptyInput request, ServerCallContext context)
        {
            var users = _userRepository.GetTotalUsers();

            var result = new View();
            users.ForEach((e) =>
            {
                result.Results.Add(new UserResponse()
                {
                    Email = e.UserEmail,
                    Na
                });
            });

            return Task.FromResult(result);
        }

    }
}