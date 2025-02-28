namespace Auctions.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string foldername)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", foldername);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath = Path.Combine(folderPath,fileName);
            using var filestream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(filestream);

            return fileName;
        }
    }
}
