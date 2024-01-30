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
        public string Sender_Email { get; set; }
        public string Recipient_Name { get; set; }
        public string Recipient_Email { get; set; }
        public BankTransfer(BankTransferDTO transfer) { 
            this.Amount = transfer.Amount;
            this.Title = transfer.Title;
            this.Sender_Email = transfer.Sender_Email;
            this.Recipient_Email = transfer.Recipient_Email;
            this.Recipient_Name = transfer.Recipient_Name;
        }
        public BankTransfer() { }
    }
}
