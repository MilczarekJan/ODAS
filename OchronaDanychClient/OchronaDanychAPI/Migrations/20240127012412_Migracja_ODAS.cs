using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OchronaDanychAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracja_ODAS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float(2)", precision: 2, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sender_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipient_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipient_Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    LettersHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    LettersSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.InsertData(
                table: "BankTransfers",
                columns: new[] { "Id", "Amount", "Recipient_Email", "Recipient_Name", "Sender_Email", "Title" },
                values: new object[,]
                {
                    { 1, 427.0, "Briana88@gmail.com", "Gia Emmerich", "Flossie.Braun@hotmail.com", "Licensed Granite Bacon hacking port Denmark" },
                    { 2, 3567.0, "Olaf.Abernathy@gmail.com", "Joseph Kuhic", "Merle28@yahoo.com", "Plains monitor ROI Djibouti Franc" },
                    { 3, 3213.0, "Merle.Bergstrom9@yahoo.com", "Jerrod Bartell", "Eusebio.Armstrong86@yahoo.com", "framework hacking quantifying Investment Account" },
                    { 4, 3680.0, "Edgardo_Fritsch54@gmail.com", "Destiny Blanda", "Antone45@yahoo.com", "navigate monitor Isle payment" },
                    { 5, 3607.0, "Randall.Bruen6@gmail.com", "Hermann Nikolaus", "Gay_Gottlieb@hotmail.com", "Unbranded Rubber Ball empower Concrete Plains" },
                    { 6, 3260.0, "Providenci98@gmail.com", "Dortha Bechtelar", "Kane.Abshire@yahoo.com", "Dynamic synthesizing deliverables static" },
                    { 7, 4317.0, "Tristian.Upton@yahoo.com", "Ariane Kessler", "Emerson.Spencer57@yahoo.com", "Auto Loan Account index invoice Colombian Peso" },
                    { 8, 1882.0, "Nayeli_Steuber@yahoo.com", "Heber Bogan", "Lester.Thompson82@gmail.com", "Home & Jewelery Health compressing Jewelery & Home" },
                    { 9, 4051.0, "Mariam40@yahoo.com", "Simone Torp", "Bradford.Bartell94@yahoo.com", "Dynamic Unbranded Wooden Ball Right-sized bandwidth-monitored" },
                    { 10, 563.0, "Leonor44@yahoo.com", "Christopher Gottlieb", "Anjali.OKon85@yahoo.com", "SAS Corporate systems Mountain" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransfers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
