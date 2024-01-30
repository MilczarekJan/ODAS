using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Models
{
    public class BankTransferDTO
    {
		[Required]
		public double Amount { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Sender_Email { get; set; }

		[Required]
		public string Recipient_Name { get; set; }

		[Required]
		public string Recipient_Email { get; set; }
        public BankTransferDTO(double amount, string title, string senderEmail, string recipientEmail, string recipientName) {
            this.Amount = amount;
            this.Title = title;
            this.Sender_Email = senderEmail;
            this.Recipient_Name = recipientName;
            this.Recipient_Email = recipientEmail;
        }

		public BankTransferDTO() { }
    }
}
