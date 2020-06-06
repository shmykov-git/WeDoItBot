using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.Extensions
{
	public static class HashHelper
	{
		public static byte[] GetHash(this string input)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				var data = sha256Hash.ComputeHash(Encoding.UTF32.GetBytes(input));

				return data;
			}
		}

		public static bool VerifyHash(this string password, byte[] passwordFromDb)
		{
			var bytes = GetHash(password);

			return bytes.SequenceEqual(passwordFromDb);
		}
	}
}