using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Services
{
	public class PasswordStrengthService
	{
		public PasswordStrengthService() { }

		public bool CheckPasswordStrength(string password) {
			double minStrength = 47.63;
			int charactersCount = 0;
			double strength;

			if (password.Any(char.IsUpper)) { 
				charactersCount += 26;
			}

			if (password.Any(char.IsLower))
			{
				charactersCount += 26;
			}

			if (password.Any(char.IsDigit))
			{
				charactersCount += 10;
			}

			strength = password.Length * Math.Log2(charactersCount);

			if (strength >= minStrength)
				return true;
			return false;
		}
	}
}
