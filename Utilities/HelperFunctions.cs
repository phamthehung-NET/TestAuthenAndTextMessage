using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace TestAuthenAndTextMessage.Utilities
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
		public static string Encrypt(string secretKey, string text)
		{
            try
            {
                var config = GetConfigurationService();
                var key = Encoding.UTF8.GetBytes(secretKey);
                var iv = Encoding.UTF8.GetBytes(config[Constants.AESInitialVector]);

                // Check arguments
                if (string.IsNullOrEmpty(text) || text.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException("key");
                if (iv == null || iv.Length <= 0)
                    throw new ArgumentNullException("iv");

                // Create an Aes object with the specified key and IV
                using Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption
                using MemoryStream ms = new();
                using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
                using (StreamWriter sw = new(cs))
                {
                    // Write all data to the stream
                    sw.Write(text);
                }
                // Return the encrypted bytes from the memory stream
                var arr = ms.ToArray();
                return BitConverter.ToString(arr).Replace("-", "");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

		/// <summary>
		/// Decrypt256
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string Decrypt(string text)
		{
            var config = GetConfigurationService();

			// AesCryptoServiceProvider
			using var aes = Aes.Create();
			aes.BlockSize = 128;
			aes.KeySize = 256;
			aes.IV = Encoding.UTF8.GetBytes(config[Constants.SystemSecretKey]);
            aes.Key = Encoding.UTF8.GetBytes(config[Constants.AESInitialVector]);
            aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;

			// Convert Base64 strings to byte array
			byte[] src = Convert.FromHexString(text);

			// decryption
			using var decrypt = aes.CreateDecryptor();
			byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
			return Encoding.Unicode.GetString(dest);
		}

        /// <summary>
        /// Get configuration
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot GetConfigurationService()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            return config;
        }

        /// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string EncryptAES(string token, object data)
        {
            try
            {
                var secretKey = string.Empty;
                if(!string.IsNullOrWhiteSpace(token))
                {
                    secretKey = token[..32];
                    DefaultContractResolver contractResolver = new()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };

                    string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                    {
                        ContractResolver = contractResolver,
                        Formatting = Formatting.Indented
                    });

                    return Encrypt(secretKey, json);
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
