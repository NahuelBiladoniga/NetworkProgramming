using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repositories.Dtos
{
    [Table("users")]
    public class UserDto{
        public string Name { get; set; }
        [Key]
        public string Email { get; set; }
    }
}
