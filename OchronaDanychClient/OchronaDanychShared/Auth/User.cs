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
        public byte[] Combo1Hash { get; set; }
        public byte[] Combo1Salt { get; set; }
        public byte[] Combo2Hash { get; set; }
        public byte[] Combo2Salt { get; set; }
        public byte[] Combo3Hash { get; set; }
        public byte[] Combo3Salt { get; set; }
        public byte[] Combo4Hash { get; set; }
        public byte[] Combo4Salt { get; set; }
        public byte[] Combo5Hash { get; set; }
        public byte[] Combo5Salt { get; set; }
        public string Email { get; set; }
        public double Balance { get; set; }
    }
}
