using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DDD.Application.ILogics
{
    public interface IFileManagerLogic
    {
        Task Upload(IFormFile file, string containerName, string fileName=null);
        Task<byte[]> Get(string FileName, string containerName);
    }
}
