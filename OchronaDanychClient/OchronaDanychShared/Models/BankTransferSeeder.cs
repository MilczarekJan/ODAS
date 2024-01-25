using Bogus;
namespace OchronaDanychShared.Models
{
    public class BankTransferSeeder
    {
        public static List<BankTransfer> GenerateTransferData()
        {
            int transferId = 1;
            var transferFaker = new Faker<BankTransfer>()
                .UseSeed(12345)
                .RuleFor(x => x.Recipient_Id, x => x.IndexFaker)
                .RuleFor(x => x.Sender_Id, x => x.IndexFaker)
                .RuleFor(x => x.Amount, x => x.Commerce.Random.Int(100, 5000))
                .RuleFor(x => x.Title, x => x.Random.Words(4))
                .RuleFor(x => x.Recipient_Name, x => x.Name.FullName())
                .RuleFor(x => x.Recipient_Email, x => x.Internet.Email())
                .RuleFor(x => x.Sender_Email, x => x.Internet.Email())
                .RuleFor(x => x.Id, x => transferId++);

            return transferFaker.Generate(10).ToList();

        }
    }
}
