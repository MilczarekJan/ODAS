using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Auth
{
    public class UserLoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public PasswordPair[] Password { get; set; }

        public UserLoginDTO(PasswordPair[] password) { 
            this.Password = password;
        }
    }
}
