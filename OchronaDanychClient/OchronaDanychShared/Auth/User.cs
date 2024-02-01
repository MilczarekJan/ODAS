using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Auth
{
    public class User
    {
        public byte[] DocumentNumber { get; set; }
        public byte[] DocumentIv { get; set; }
        public byte[] DocumentKey { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] LettersHash { get; set; }
        public byte[] LettersSalt { get; set; }
        public string Email { get; set; }
        public double Balance { get; set; }
    }
}
