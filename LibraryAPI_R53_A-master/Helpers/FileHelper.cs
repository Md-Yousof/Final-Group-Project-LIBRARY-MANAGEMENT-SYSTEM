namespace LibraryAPI_R53_A.Helpers
{
    public class FileHelper
    {
        private readonly string _uploadsFolder;

        public FileHelper(string uploadsFolderPath)
        {
            _uploadsFolder = uploadsFolderPath;
        }

        public string SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var fileUrl = "http://localhost:5154/" + "uploads/" + uniqueFileName;

            return fileUrl;
        }
    }
}
