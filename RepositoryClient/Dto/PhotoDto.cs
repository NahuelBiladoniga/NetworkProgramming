using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryClient.Dto
{
    public class PhotoDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }
        public string UserEmail { get; set; }

    }
}
