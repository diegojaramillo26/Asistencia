using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Asistencia.Common
{
    public static class EncryptAes
    {
		public static string Encrypt(string text)
		{
			var b = Encoding.UTF8.GetBytes(text);
			var encrypted = GetAes().CreateEncryptor().TransformFinalBlock(b, 0, b.Length);
			return Convert.ToBase64String(encrypted);
		}

		public static string Decrypt(string encrypted)
		{
			var b = Convert.FromBase64String(encrypted);
			var decrypted = GetAes().CreateDecryptor().TransformFinalBlock(b, 0, b.Length);
			return Encoding.UTF8.GetString(decrypted);
		}

		private static Aes GetAes()
		{
			var keyBytes = new byte[16];
			var skeyBytes = Encoding.UTF8.GetBytes("cQfTjWnZr4t7w!z%");
			Array.Copy(skeyBytes, keyBytes, Math.Min(keyBytes.Length, skeyBytes.Length));

			Aes aes = Aes.Create();
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.KeySize = 128;
			aes.Key = keyBytes;
			aes.IV = keyBytes;

			return aes;
		}
	}
}