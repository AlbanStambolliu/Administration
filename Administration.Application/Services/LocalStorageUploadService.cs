using Administration.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Application.Services
{
    public class LocalStorageUploadService : IUploadService
    {
        private readonly string _outputPath = "out";

        public async Task<byte[]> GetFile(string path)
        {
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    await fs.CopyToAsync(ms);
                    result = ms.ToArray();
                }
            }

            return result;
        }

        public async Task<string> SaveFile(byte[] fileBytes, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            //var newFileName = fileName.Replace(fileExtension, $"_{DateTime.UtcNow.Ticks.ToString()}_{fileExtension}");
            var newFileName = $"file_{DateTime.UtcNow.Ticks.ToString()}{fileExtension}";
            var fullFileName = Path.Join(_outputPath, newFileName);
            if (!Directory.Exists(_outputPath))
                Directory.CreateDirectory(_outputPath);
            await File.WriteAllBytesAsync(fullFileName, fileBytes);

            return fullFileName;
        }
    }
}
