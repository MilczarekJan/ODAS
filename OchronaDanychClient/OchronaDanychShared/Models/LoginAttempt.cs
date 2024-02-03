using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Models
{
    public class LoginAttempt
    {
        public string UserEmail { get; set; }
        public DateTime LastAttempt { get; set; }
        public int Attempts { get; set; }
    }
}
