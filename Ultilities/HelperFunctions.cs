using System.IO;

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
	}
}
