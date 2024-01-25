using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Models
{
    public class BankTransfer
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Title { get; set; }
        public int Sender_Id { get; set; }
        public string Sender_Email { get; set; }
        public int Recipient_Id { get; set; }
        public string Recipient_Name { get; set; }
        public string Recipient_Email { get; set; }
    }
}
