using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryClient.Dto
{
    public class CommentDto
    {
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public long PhotoId { get; set; }
    }
}
