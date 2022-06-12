using System;
using System.Security.Cryptography;
using System.Text;

namespace CapstoneOnGoing.Utils
{
	public class GenerateUtil
	{
		public static string GenerateJoinString()
		{
			const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			StringBuilder res = new StringBuilder();
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				byte[] uintBuffer = new byte[sizeof(uint)];
				int stringLength = 6;
				while (res.Length < stringLength)
				{
					rng.GetBytes(uintBuffer);
					uint num = BitConverter.ToUInt32(uintBuffer, 0);
					res.Append(valid[(int)(num % (uint)valid.Length)]);
				}
			}
			return res.ToString();
        }
	}
}
