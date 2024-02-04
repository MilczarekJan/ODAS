using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Models
{
    public class CombinationDTO
    {
        public string Combo1 { get; set; }
        public string Combo2 { get; set; }
        public string Combo3 { get; set; }
        public string Combo4 { get; set; }
        public string Combo5 { get; set; }
        public byte[] Combo1Hash { get; set; }
        public byte[] Combo2Hash { get; set; }
        public byte[] Combo3Hash { get; set; }
        public byte[] Combo4Hash { get; set; }
        public byte[] Combo5Hash { get; set; }
        public byte[] Combo1Salt { get; set; }
        public byte[] Combo2Salt { get; set; }
        public byte[] Combo3Salt { get; set; }
        public byte[] Combo4Salt { get; set; }
        public byte[] Combo5Salt { get; set; }
    }
}
