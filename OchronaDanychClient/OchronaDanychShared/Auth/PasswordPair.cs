using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Auth
{
    public class PasswordPair
    {
        public char Letter {  get; set; } //litera hasla
        public int Order { get; set; } //kolejnosc litery w hasle
    }
}
