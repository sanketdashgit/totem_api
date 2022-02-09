using Totem.Business.Core.DataTransferModels;
using System.IO;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels.Documents;

namespace Totem.Business.RepositoryInterface
{
    public interface IAmazonS3Repository
    {

        Task<ExecutionResult<TestResponceModel>> ReadAccessFile(string fileName);
        Task<ExecutionResult<TestResponceModel>> TestingFile(string fileName);
        //UploadFile
        Task<ExecutionResult<bool>> UploadFile(MemoryStream stream, string fileName);
        //GetFile
        ExecutionResult<bool> GetFile(string FileName);
        //DeleteFile
        Task<ExecutionResult<bool>> DeleteFile(string FileName);
    }
}
