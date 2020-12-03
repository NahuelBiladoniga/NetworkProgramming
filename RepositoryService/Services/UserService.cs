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
        private readonly ICommentsRepository commentsRepository;
        private readonly IUserRepository userRepository;
        private readonly IPhotoRepository photoRepository;

        public UserService(ILogger<UserService> logger)
        {

            _logger = logger;
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

            return Task.FromResult(new ResponseMessage
            {
                Status = "asdads",
                Message = "test",
            });
        }

        public override Task<ResponseMessage> RemoveUser(RemoveUserInput request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseMessage
            {
                Status = "asdads",
                Message = "test",
            });
        }
        public override Task<ResponseMessage> ModifyUser(ModifyUserInput request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseMessage
            {
                Status = "asdads",
                Message = "test",
            });
        }
        public override Task<ResponseMessage> UploadPhoto(UploadPhotoInput request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseMessage
            {
                Status = "asdads",
                Message = "test",
            });
        }
        public override Task<ViewPhotoResponse> ViewPhotos(EmptyInput request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseMessage
            {
                Status = "asdads",
                Message = "test",
            });
        }
        public override Task<ViewCommentResponse> ViewComments(ViewCommentInput request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseMessage
            {
                Status = "asdads",
                Message = "test",
            });
        }
        public override Task<ViewUserResponse> ViewUsers(EmptyInput request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseMessage
            {
                Status = "asdads",
                Message = "test",
            });
        }

    }
}