using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OchronaDanychShared;
using OchronaDanychShared.Auth;
using OchronaDanychAPI.Models;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using Ganss.Xss;

namespace OchronaDanychAPI.Services.AuthService
{

    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        public AuthService(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(string userMail, string newPassword)
        {
            userMail = SanitizeInput(userMail);

            var user = await _context.Users.FindAsync(userMail);
            if (user == null)
            {
                  return new ServiceResponse<bool>
                  {
                    Success = false,
                    Message = "User not found."
                };
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            CreateLettersHash(newPassword, out byte[] lettersHash, out byte[] lettersSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.LettersHash = lettersHash;
            user.LettersSalt = lettersSalt;

            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "success",
                Success = true
            };
        }

        public async Task<ServiceResponse<string>> Login(string email, PasswordPair[] password)
        {
            email = SanitizeInput(email);
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.LettersHash, user.LettersSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
            }
            else
            {
                response.Data = CreateToken(user);
                response.Success = true;
                response.Message = "Login successful.";
            }

           
            return response;
        }

        private bool VerifyPasswordHash(PasswordPair[] password, byte[] lettersHash, byte[] lettersSalt)
        {
            foreach( var pair in password)
            {
                byte[] hashPart = new byte[64];
                byte[] saltPart = new byte[128];
                Array.Copy(lettersHash, pair.Order * 64,hashPart, 0, 64);
                Array.Copy(lettersSalt, pair.Order * 128, saltPart, 0, 128);
                var hmac = new System.Security.Cryptography.HMACSHA512(saltPart);
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pair.Letter.ToString()));
                if (!computedHash.SequenceEqual(hashPart)) { 
                    return false;
                }
            }
            return true;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
             {
                 new Claim(ClaimTypes.Name, user.Username),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.UserData, user.Balance.ToString())
             };

            SymmetricSecurityKey key =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                               claims: claims,
                               expires: DateTime.Now.AddMinutes(30),
                               signingCredentials: creds
                  );

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenHandler;
        }

        public async Task<ServiceResponse<string>> Register(User user, string password, string documentNumber)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }

            // create password hash and salt
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            CreateLettersHash(password, out byte[] lettersHash, out byte[] lettersSalt);
            EncryptStringToBytes_Aes(documentNumber, out byte[] docKey, out byte[] docIV, out byte[] documentHash);

            // assign hash and salt to user
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.LettersHash = lettersHash;
            user.LettersSalt = lettersSalt;
            user.DocumentNumber = documentHash;
            user.DocumentIv = docIV;
            user.DocumentKey = docKey;

            // add user to db
            await _context.Users.AddAsync(user);
            // save changes
            await _context.SaveChangesAsync();

            return new ServiceResponse<string> { Success = true, Data = user.Email, Message = "Registration successful!" };

        }

        public async Task<ServiceResponse<string>> GetDocumentNumber(string email) {
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
            string document = DecryptStringFromBytes_Aes(user.DocumentNumber, user.DocumentKey, user.DocumentIv);
			return new ServiceResponse<string>
			{
				Data = document,
				Message = "Balance accessed successfully.",
				Success = true
			};
		}

        public async Task<ServiceResponse<string>> GetBalance(string email)
        {
            email = SanitizeInput(email);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
            return new ServiceResponse<string>
            {
                Data = user.Balance.ToString(),
                Message = "Balance accessed successfully.",
                Success = true
            };
        }

        public async Task<ServiceResponse<bool>> CheckPassword(string email, string password) 
        {
            email = SanitizeInput(email);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
			var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            if (!computedHash.SequenceEqual(user.PasswordHash))
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Wrong password",
                    Success = false
                };
            }
            else return new ServiceResponse<bool>
            {
                Data = true,
                Message = "success",
                Success = true
            };
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // using statement to dispose of IDisposable objects
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) // using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                // generate random salt
                passwordSalt = hmac.Key;
                // generate hash
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public void CreateLettersHash(string password, out byte[] lettersHash, out byte[] lettersSalt)
        {
            List<byte> hashList = new List<byte>();
            List<byte> saltList = new List<byte>();
            for (int i = 0; i < 8; i++)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    char letter = password[i];
                    byte[] letterBytes = System.Text.Encoding.UTF8.GetBytes(letter.ToString());
                    saltList.AddRange(hmac.Key);
                    hashList.AddRange(hmac.ComputeHash(letterBytes));
                }
            }
            lettersHash = hashList.ToArray();
            lettersSalt = saltList.ToArray();
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }

		public void EncryptStringToBytes_Aes(string docNumber, out byte[] Key, out byte[] IV, out byte[] encrypted)
		{
			// Check arguments.
			if (docNumber == null || docNumber.Length <= 0)
				throw new ArgumentNullException("docNumber");

			// Create an Aes object
			// with the specified key and IV.
			using (Aes aesAlg = Aes.Create())
			{
                Key = aesAlg.Key; 
                IV = aesAlg.IV;

				// Create an encryptor to perform the stream transform.
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for encryption.
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							//Write all data to the stream.
							swEncrypt.Write(docNumber);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}
		}

		public string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
		{
			// Check arguments.
			if (cipherText == null || cipherText.Length <= 0)
				throw new ArgumentNullException("cipherText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException("IV");

			// Declare the string used to hold
			// the decrypted text.
			string plaintext = null;

			// Create an Aes object
			// with the specified key and IV.
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Key;
				aesAlg.IV = IV;

				// Create a decryptor to perform the stream transform.
				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for decryption.
				using (MemoryStream msDecrypt = new MemoryStream(cipherText))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{

							// Read the decrypted bytes from the decrypting stream
							// and place them in a string.
							plaintext = srDecrypt.ReadToEnd();
						}
					}
				}
			}
			return plaintext;
		}

        private string SanitizeInput(string input) {
            HtmlSanitizerOptions options = new HtmlSanitizerOptions();
            options.AllowedTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { };
            options.AllowedAttributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { };
            options.AllowedCssProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { };
            HtmlSanitizer sanitizer = new HtmlSanitizer(options);
            input = sanitizer.Sanitize(input);
            return input;
        }
	}
}
