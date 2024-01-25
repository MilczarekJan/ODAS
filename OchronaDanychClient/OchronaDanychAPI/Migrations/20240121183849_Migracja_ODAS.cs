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
                    Sender_Id = table.Column<int>(type: "int", nullable: false),
                    Sender_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipient_Id = table.Column<int>(type: "int", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    LettersHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    LettersSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BankTransfers",
                columns: new[] { "Id", "Amount", "Recipient_Email", "Recipient_Id", "Recipient_Name", "Sender_Email", "Sender_Id", "Title" },
                values: new object[,]
                {
                    { 1, 427.0, "Briana88@gmail.com", 0, "Gia Emmerich", "Flossie.Braun@hotmail.com", 0, "Licensed Granite Bacon hacking port Denmark" },
                    { 2, 3567.0, "Olaf.Abernathy@gmail.com", 1, "Joseph Kuhic", "Merle28@yahoo.com", 1, "Plains monitor ROI Djibouti Franc" },
                    { 3, 3213.0, "Merle.Bergstrom9@yahoo.com", 2, "Jerrod Bartell", "Eusebio.Armstrong86@yahoo.com", 2, "framework hacking quantifying Investment Account" },
                    { 4, 3680.0, "Edgardo_Fritsch54@gmail.com", 3, "Destiny Blanda", "Antone45@yahoo.com", 3, "navigate monitor Isle payment" },
                    { 5, 3607.0, "Randall.Bruen6@gmail.com", 4, "Hermann Nikolaus", "Gay_Gottlieb@hotmail.com", 4, "Unbranded Rubber Ball empower Concrete Plains" },
                    { 6, 3260.0, "Providenci98@gmail.com", 5, "Dortha Bechtelar", "Kane.Abshire@yahoo.com", 5, "Dynamic synthesizing deliverables static" },
                    { 7, 4317.0, "Tristian.Upton@yahoo.com", 6, "Ariane Kessler", "Emerson.Spencer57@yahoo.com", 6, "Auto Loan Account index invoice Colombian Peso" },
                    { 8, 1882.0, "Nayeli_Steuber@yahoo.com", 7, "Heber Bogan", "Lester.Thompson82@gmail.com", 7, "Home & Jewelery Health compressing Jewelery & Home" },
                    { 9, 4051.0, "Mariam40@yahoo.com", 8, "Simone Torp", "Bradford.Bartell94@yahoo.com", 8, "Dynamic Unbranded Wooden Ball Right-sized bandwidth-monitored" },
                    { 10, 563.0, "Leonor44@yahoo.com", 9, "Christopher Gottlieb", "Anjali.OKon85@yahoo.com", 9, "SAS Corporate systems Mountain" }
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
