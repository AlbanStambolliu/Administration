using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> SaveFile(byte[] fileBytes, string fileName);
        Task<byte[]> GetFile(string fileName);
    }
}
