using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OchronaDanychShared;
using OchronaDanychShared.Auth;
using OchronaDanychAPI.Models;

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
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Password updated successfully.",
                Success = true
            };
        }

        public async Task<ServiceResponse<string>> Login(string email, PasswordPair[] password)
        {
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

        /*
        public async Task<ServiceResponse<bool>> ChangeBalance(string email, string amount) {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            user.Balance += Double.Parse(amount);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Password updated successfully.",
                Success = true
            };
        }
        */

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
                               expires: DateTime.Now.AddDays(1),
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
            CreatePasswordHash(documentNumber, out byte[] documentHash, out byte[] documentSalt);
            List<byte> documentList = documentHash.ToList();
            documentList.AddRange(documentSalt);
            documentHash = documentList.ToArray();

            // assign hash and salt to user
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.LettersHash = lettersHash;
            user.LettersSalt = lettersSalt;
            user.DocumentNumber = documentHash;

            // add user to db
            await _context.Users.AddAsync(user);
            // save changes
            await _context.SaveChangesAsync();

            return new ServiceResponse<string> { Success = true, Data = user.Email, Message = "Registration successful!" };

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
    }
}
