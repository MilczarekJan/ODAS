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
using OchronaDanychShared.Models;

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
            var combination = await _context.Combinations.FindAsync(userMail);

            if (user == null || combination == null)
            {
                  return new ServiceResponse<bool>
                  {
                    Success = false,
                    Message = "User not found."
                };
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            CreateLettersHash(newPassword, out CombinationDTO combinations);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Combo1Hash = combinations.Combo1Hash;
            user.Combo2Hash = combinations.Combo2Hash;
            user.Combo3Hash = combinations.Combo3Hash;
            user.Combo4Hash = combinations.Combo4Hash;
            user.Combo5Hash = combinations.Combo5Hash;
            user.Combo1Salt = combinations.Combo1Salt;
            user.Combo2Salt = combinations.Combo2Salt;
            user.Combo3Salt = combinations.Combo3Salt;
            user.Combo4Salt = combinations.Combo4Salt;
            user.Combo5Salt = combinations.Combo5Salt;

            combination.Combo1 = combinations.Combo1;
            combination.Combo2 = combinations.Combo2;
            combination.Combo3 = combinations.Combo3;
            combination.Combo4 = combinations.Combo4;
            combination.Combo5 = combinations.Combo5;
            combination.Email = user.Email;
            combination.Choice = 1;

            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "success",
                Success = true
            };
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            email = SanitizeInput(email);
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            var loginAttempt = await _context.LoginAttempts.FirstOrDefaultAsync(x => x.UserEmail.ToLower() == email.ToLower());
            var choice = await _context.Combinations.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());


            if (user == null || loginAttempt == null || choice == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            else if (loginAttempt.Attempts >= 5 && (DateTime.Now - loginAttempt.LastAttempt).TotalMinutes <= 5)
            {
                response.Success = false;
                response.Message = "Too many failed attempts. Try again in 5 minutes.";
                return response;
            }

            if (loginAttempt.Attempts >= 5 && (DateTime.Now - loginAttempt.LastAttempt).TotalMinutes > 5)
            {
                loginAttempt.Attempts = 0;
                loginAttempt.LastAttempt = DateTime.Now;
            }

            if (!VerifyPasswordHash(user, password, choice.Choice))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
                loginAttempt.Attempts++;
                loginAttempt.LastAttempt = DateTime.Now;
                await _context.SaveChangesAsync();
                return response;
            }
            else
            {
                response.Data = CreateToken(user);
                response.Success = true;
                response.Message = "Login successful.";
                loginAttempt.Attempts = 0;
                loginAttempt.LastAttempt = DateTime.Now;
                await _context.SaveChangesAsync();
                return response;
            }
        }

        private bool VerifyPasswordHash(User user, string password, int choice)
        {
            var hmac = new System.Security.Cryptography.HMACSHA512();
            switch (choice) 
            {
                case 1:
                    hmac = new System.Security.Cryptography.HMACSHA512(user.Combo1Salt);
                    return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)).SequenceEqual(user.Combo1Hash);
                case 2:
                    hmac = new System.Security.Cryptography.HMACSHA512(user.Combo2Salt);
                    return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)).SequenceEqual(user.Combo2Hash);
                case 3:
                    hmac = new System.Security.Cryptography.HMACSHA512(user.Combo3Salt);
                    return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)).SequenceEqual(user.Combo3Hash);
                case 4:
                    hmac = new System.Security.Cryptography.HMACSHA512(user.Combo4Salt);
                    return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)).SequenceEqual(user.Combo4Hash);
                case 5:
                    hmac = new System.Security.Cryptography.HMACSHA512(user.Combo5Salt);
                    return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)).SequenceEqual(user.Combo5Hash);
                default:
                    return false;
            }
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
            CreateLettersHash(password, out CombinationDTO combinations);
            EncryptStringToBytes_Aes(documentNumber, out byte[] docKey, out byte[] docIV, out byte[] documentHash);

            // assign hash and salt to user
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.DocumentNumber = documentHash;
            user.DocumentIv = docIV;
            user.DocumentKey = docKey;
            user.Combo1Hash = combinations.Combo1Hash;
            user.Combo2Hash = combinations.Combo2Hash;
            user.Combo3Hash = combinations.Combo3Hash;
            user.Combo4Hash = combinations.Combo4Hash;
            user.Combo5Hash = combinations.Combo5Hash;
            user.Combo1Salt = combinations.Combo1Salt;
            user.Combo2Salt = combinations.Combo2Salt;
            user.Combo3Salt = combinations.Combo3Salt;
            user.Combo4Salt = combinations.Combo4Salt;
            user.Combo5Salt = combinations.Combo5Salt;

            LoginAttempt userAttempts = new LoginAttempt();
            userAttempts.UserEmail = user.Email;
            userAttempts.Attempts = 0;
            userAttempts.LastAttempt = DateTime.UtcNow;

            Combination combination = new Combination();
            combination.Combo1 = combinations.Combo1;
            combination.Combo2 = combinations.Combo2;
            combination.Combo3 = combinations.Combo3;
            combination.Combo4 = combinations.Combo4;
            combination.Combo5 = combinations.Combo5;
            combination.Email = user.Email;
            combination.Choice = 1;

            await _context.Users.AddAsync(user);
            await _context.LoginAttempts.AddAsync(userAttempts);
            await _context.Combinations.AddAsync(combination);
            await _context.SaveChangesAsync();

            return new ServiceResponse<string> { Success = true, Data = user.Email, Message = "Registration successful!" };

        }

        public async Task<ServiceResponse<string>> GetDocumentNumber(string email) {
            email = SanitizeInput(email);
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

        public void CreateLettersHash(string password, out CombinationDTO combinations)
        {
            combinations = new CombinationDTO();
            string[] indices = new string[5];
            string currentCombo;
            int[] currentComboIndices = new int[5];
            byte[][] hashes = new byte[5][];
            byte[][] salts = new byte[5][];

            for (int i = 0; i < 5; i++)
            {
                currentCombo = GenerateRandomComboIndices(password.Length);
                while (indices.Take(i).Contains(currentCombo))
                {
                    currentCombo = GenerateRandomComboIndices(password.Length);
                }
                indices[i] = currentCombo;
            }

            combinations.Combo1 = indices[0];
            combinations.Combo2 = indices[1];
            combinations.Combo3 = indices[2];
            combinations.Combo4 = indices[3];
            combinations.Combo5 = indices[4];

            for (int i = 0; i < 5; i++)
            {
                currentComboIndices = indices[i].Split(',').Select(int.Parse).ToArray();
                currentCombo = password[currentComboIndices[0]].ToString()
                    + password[currentComboIndices[1]].ToString()
                    + password[currentComboIndices[2]].ToString()
                    + password[currentComboIndices[3]].ToString()
                    + password[currentComboIndices[4]].ToString();
                var hmac = new System.Security.Cryptography.HMACSHA512();
                salts[i] = hmac.Key;
                hashes[i] = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(currentCombo));
            }
            combinations.Combo1Hash = hashes[0];
            combinations.Combo2Hash = hashes[1];
            combinations.Combo3Hash = hashes[2];
            combinations.Combo4Hash = hashes[3];
            combinations.Combo5Hash = hashes[4];
            combinations.Combo1Salt = salts[0];
            combinations.Combo2Salt = salts[1];
            combinations.Combo3Salt = salts[2];
            combinations.Combo4Salt = salts[3];
            combinations.Combo5Salt = salts[4];
        }


        private string GenerateRandomComboIndices(int passwordLength)
        {
            Random random = new Random();
            int[] indices = new int[5];

            for (int j = 0; j < 5; j++)
            {
                int index;
                do
                {
                    index = random.Next(0, passwordLength);
                } while (Array.IndexOf(indices, index) != -1);

                indices[j] = index;
            }

            Array.Sort(indices);
            return string.Join(",", indices);
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

        public async Task<ServiceResponse<string>> CheckUser(string email) 
        {
            email = SanitizeInput(email);
            var combo = await _context.Combinations.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            string chosenCombo;
            if (combo == null) 
            {
                return new ServiceResponse<string>
                {
                    Data = "User does not exist.",
                    Message = "User does not exist.",
                    Success = false
                };
            }
            Random randomizer = new Random();
            int choice = randomizer.Next(1, 6);
            combo.Choice = choice;
            await _context.SaveChangesAsync();
            switch (choice) 
            {
                case 1:
                    chosenCombo = combo.Combo1;
                    break;
                case 2:
                    chosenCombo = combo.Combo2;
                    break;
                case 3:
                    chosenCombo = combo.Combo3;
                    break;
                case 4:
                    chosenCombo = combo.Combo4;
                    break;
                case 5:
                    chosenCombo = combo.Combo5;
                    break;
                default:
                    chosenCombo = "Randomizer error";
                    break;
            }
            chosenCombo = IncrementNumbers(chosenCombo);
            return new ServiceResponse<string>
            {
                Data = chosenCombo,
                Message = "User exists.",
                Success = true
            };
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
        private string IncrementNumbers(string input)
        {
            string[] numbers = input.Split(',');
            var incrementedNumbers = numbers.Select(num => (int.Parse(num) + 1).ToString());
            string result = string.Join(",", incrementedNumbers);
            return result;
        }
    }
}
