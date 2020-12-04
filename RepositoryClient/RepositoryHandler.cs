using Grpc.Net.Client;
using RepositoryClient.Dto;
using RepositoryService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace RepositoryClient
{
    public class RepositoryHandler
    {
        private static UserHandler.UserHandlerClient _client;

        public RepositoryHandler()
        {
            var connectionAddress = ConfigurationManager.AppSettings["SERVICE_URL"];

            var channel = GrpcChannel.ForAddress(connectionAddress);
            _client = new UserHandler.UserHandlerClient(channel);

        }

        public async Task<ResponseDto> AddCommentAsync(CommentDto comment)
        {
            var input = new AddCommentInput() { Comment = comment.Message, PhotoId = comment.PhotoId };
            var response = await _client.AddCommentAsync(input);

            return new ResponseDto()
            {
                Message = response.Message,
                Status = response.Status,
            };
        }

        public async Task<ResponseDto> AddUserAsync(UserDto user)
        {
            var input = new AddUserInput() { 
                Email = user.Email,
                Name = user.Name,
                Password = user.Password
            };

            var response = await _client.AddUserAsync(input);

            return new ResponseDto()
            {
                Message = response.Message,
                Status = response.Status,
            };
        }

        public async Task<ResponseDto> RemoveUserAsync(UserDto user)
        {
            var input = new RemoveUserInput()
            {
                Email = user.Email,
            };

            var response = await _client.RemoveUserAsync(input);

            return new ResponseDto()
            {
                Message = response.Message,
                Status = response.Status,
            };
        }

        public async Task<ResponseDto> ModifyUserAsync(UserDto user)
        {
            var input = new ModifyUserInput()
            {
                Email = user.Email,
                Name = user.Name,
                Password = user.Password
            };

            var response = await _client.ModifyUserAsync(input);

            return new ResponseDto()
            {
                Message = response.Message,
                Status = response.Status,
            };
        }

        public async Task<ResponseDto> UploadPhotoAsync(PhotoDto photo)
        {
            var input = new UploadPhotoInput()
            {
                Extension = photo.Extension,
                Name = photo.Name,
                Size = photo.FileSize,
                UserEmail = photo.UserEmail
            };

            var response = await _client.UploadPhotoAsync(input);

            return new ResponseDto()
            {
                Message = response.Message,
                Status = response.Status,
            };
        }

        public async Task<List<PhotoDto>> GetPhotosAsync()
        {          
            var response = await _client.ViewPhotosAsync(new EmptyInput());
            var result = new List<PhotoDto>();

            foreach(var a in response.Results)
            {
                result.Add(new PhotoDto
                {
                    Id = a.IdPhoto,
                    Name = a.NamePhoto,
                    FileSize = a.FileSize,
                    Extension = a.Extension,
                    UserEmail = a.Email
                });
            }

            return result;
        }
        public async Task<List<CommentDto>> GetCommentsAsync(PhotoDto photoDto)
        {
            var response = await _client.ViewCommentsAsync(new ViewCommentInput() { PhotoId = photoDto.Id});
            var result = new List<CommentDto>();

            foreach (var a in response.Results)
            {
                result.Add(new CommentDto
                {
                    Message = a.Comment,
                    UserEmail = a.Email,
                    UserName = a.Name
                });
            }

            return result;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var response = await _client.ViewUsersAsync(new EmptyInput());
            var result = new List<UserDto>();

            foreach (var a in response.Results)
            {
                result.Add(new UserDto
                {
                    Email = a.Email,
                    Name = a.Name,
                    //LastConnection = a.LastConnected
                });
            }

            return result;
        }
        
        public async Task<bool> AutenticateUserAsync(UserDto user)
        {
            var input = new AutenticateUserInput() { 
                Email = user.Email,
                Password = user.Password
            };

            var response = await _client.AutenticateUserAsync(input);

            return response.Status.Equals("OK");
        }

        public async Task<ResponseDto> LogoutUser(UserDto user)
        {
            var input = new LogoutUserInput()
            {
                Email = user.Email
            };

            var response = await _client.LogoutUserAsync(input);

            return new ResponseDto()
            {
                Message = response.Message,
                Status = response.Status,
            };
        }
    }
}