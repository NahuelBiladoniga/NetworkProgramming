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

        public Task<ResponseMessage> AutenticateUserAsync(AutenticateUserInput request, ServerCallContext context)
        {
            var user = new User()
            {
                Email = request.Email,
                Password = request.Password
            };

            var savedUser = _userRepository.GetUser(user);

            if (savedUser.Password.Equals(user.Password) && savedUser.Email.Equals(user.Email))
            {
                savedUser.IsConnected = true;
                savedUser.LastConnection = DateTime.Now;

                return Task.FromResult(new ResponseMessage
                {
                    Status = "Ok",
                    Message = "Bienvenido!",
                });
            }
            else
            {
                return Task.FromResult(new ResponseMessage
                {
                    Status = "Error",
                    Message = "Credenciales invalidas",
                });
            }

        }

        public override Task<ResponseMessage> AddComment(AddCommentInput request, ServerCallContext context)
        {
            var comment = new Comment()
            {
                Message = request.Comment
            };
            var photo = new Photo
            {
                Id = request.PhotoId,
            };

            _commentsRepository.CommentPhoto(photo, comment);

            return Task.FromResult(new ResponseMessage
            {
                Status = "Ok",
                Message = "Comentario agregado",
            });
        }

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

                _userRepository.DeleteUser(user);

                return Task.FromResult(new ResponseMessage
                {
                    Status = "Ok",
                    Message = "Se ha borrado el usuario",
                });
            }
            else
            {
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
                User = user
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
                    Email = e.User.Email,
                    NamePhoto = e.Name,
                    IdPhoto = e.Id,
                    Extension = e.Extension,
                    FileSize = e.FileSize,
                });
            });
            
            return Task.FromResult(result);
        }
        public override Task<ViewCommentResponse> ViewComments(ViewCommentInput request, ServerCallContext context)
        {
            var photo = _photoRepository.GetPhotos().Find(p => p.Id == request.PhotoId);
            var comments = _commentsRepository.GetCommentsFromPhoto(photo);

            var result = new ViewCommentResponse();
            comments.ForEach((e) =>
            {
                var user = e.Photo.User;

                result.Results.Add(new CommentResponse()
                {
                    Comment = e.Message,
                    Name = user.Name,
                    Email = user.Email
                });
            });

            return Task.FromResult(result);
        }

        public override Task<ViewUserResponse> ViewUsers(EmptyInput request, ServerCallContext context)
        {
            var users = _userRepository.GetUsers().ToList();

            var result = new ViewUserResponse();
            users.ForEach((e) =>
            {
                result.Results.Add(new UserResponse()
                {
                    Email = e.Email,
                    Name = e.Name,
                    LastConnected = e.LastConnection.ToString(),
                });
            });

            return Task.FromResult(result);
        }

    }
}