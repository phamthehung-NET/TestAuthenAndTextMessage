using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TestAuthenAndTextMessage.Ultilities
{
    public class HelperFunctions
    {
        public static string UploadBase64File(string file, string fileName, string directory)
        {
            FileInfo rootpath = new(@"wwwroot\" + directory);

            if (!Directory.Exists(rootpath.FullName))
            {
                Directory.CreateDirectory(rootpath.FullName);
            }
            try
            {
                byte[] imageBytes = Convert.FromBase64String(file);
                if (file != null && file.Length > 0)
                {
                    var fileNameWE = Path.GetFileNameWithoutExtension(fileName);
                    var fileExtention = Path.GetExtension(fileName);
                    string newFileName = fileNameWE + DateTime.Now.ToString("ddMMyyyyHms") + fileExtention;
                    var fileSavePath = Path.Combine(rootpath.FullName, newFileName);

                    using (var imageFile = new FileStream(fileSavePath, FileMode.Create))
                    {
                        imageFile.Write(imageBytes, 0, imageBytes.Length);
                        imageFile.Flush();
                    }
                    return "/" + directory + "/" + newFileName;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool RemoveFile(string path)
        {
			FileInfo filePath = new(@"wwwroot\" + path);
            if (filePath.Exists)
            {
                filePath.Delete();
                return true;
            }
            return false;
		}

		/// <summary>
		/// Encrypt256
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string Encrypt(string text)
		{
			// AesCryptoServiceProvider
			Aes aes = Aes.Create();
			aes.BlockSize = 128;
			aes.KeySize = 256;
			aes.IV = Encoding.UTF8.GetBytes(Constants.AESInitalVector);
			aes.Key = Encoding.UTF8.GetBytes(Constants.SystemSecretKey);
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;

			// Convert string to byte array
			byte[] src = Encoding.Unicode.GetBytes(text);

			// encryption
			using var encrypt = aes.CreateEncryptor();
			byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

			// Convert byte array to Base64 strings
			return Convert.ToBase64String(dest);
		}

		/// <summary>
		/// Decrypt256
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string Decrypt(string text)
		{
			// AesCryptoServiceProvider
			Aes aes = Aes.Create();
			aes.BlockSize = 128;
			aes.KeySize = 256;
			aes.IV = Encoding.UTF8.GetBytes(Constants.AESInitalVector);
			aes.Key = Encoding.UTF8.GetBytes(Constants.SystemSecretKey);
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;

			// Convert Base64 strings to byte array
			byte[] src = Convert.FromBase64String(text);

			// decryption
			using var decrypt = aes.CreateDecryptor();
			byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
			return Encoding.Unicode.GetString(dest);
		}
	}
}
