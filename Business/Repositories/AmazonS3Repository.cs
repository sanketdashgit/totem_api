using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Totem.Business.Core.AppSettings;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.RepositoryInterface;
using Totem.Database.Models;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels.Documents;

namespace Totem.Business.Repositories
{
    public class AmazonS3Repository : IAmazonS3Repository
    {
        #region Constructor
        private readonly TotemDBContext _dbContext;
        private readonly AWS _awsSettings;
        private readonly RegionEndpoint _region;
        public AmazonS3Repository(TotemDBContext dbContext, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _awsSettings = appSettings.Value.AWS;
            _region = RegionEndpoint.USEast2;
        }
        #endregion


        public async Task<ExecutionResult<TestResponceModel>> ReadAccessFile(string fileName)
        {
            TestResponceModel path = new TestResponceModel();

            using (AmazonS3Client client = new AmazonS3Client(_awsSettings.AccessKey, _awsSettings.SecretAccessKey, _region))
            {
                // create S3 put object



                PutACLRequest request = new PutACLRequest();
                request.BucketName = _awsSettings.BucketName;
                request.Key = fileName;
                request.CannedACL = S3CannedACL.PublicRead;
                var DownloadURL = await client.PutACLAsync(request);


            }
            return new ExecutionResult<TestResponceModel>(path);
        }
        public async Task<ExecutionResult<TestResponceModel>> TestingFile(string fileName)
        {
            TestResponceModel path = new TestResponceModel();

            using (AmazonS3Client client = new AmazonS3Client(_awsSettings.AccessKey, _awsSettings.SecretAccessKey, _region))
            {
                // create S3 put object



                GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest();


                request1.BucketName = _awsSettings.BucketName;
                request1.Key = fileName;
                request1.Verb = HttpVerb.PUT;
                request1.Expires = System.DateTime.UtcNow.AddHours(2);
                request1.Protocol = Protocol.HTTP;

                path.UploadURL = client.GetPreSignedURL(request1);

                //PutACLRequest request = new PutACLRequest();
                //request.BucketName = _awsSettings.BucketName;
                //request.Key = fileName;
                //request.CannedACL = S3CannedACL.PublicRead;
                //var DownloadURL = await client.PutACLAsync(request);


            }
            return new ExecutionResult<TestResponceModel>(path);
        }
        #region File Upload in AWS
        public async Task<ExecutionResult<bool>> UploadFile(MemoryStream stream, string fileName)
        {
            // create Amazon client
            using (AmazonS3Client client = new AmazonS3Client(_awsSettings.AccessKey, _awsSettings.SecretAccessKey, _region))
            {
                // create S3 put object
                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = _awsSettings.BucketName,
                    Key = fileName,
                    InputStream = stream,
                    CannedACL = S3CannedACL.PublicRead
                };

                try
                {
                    //upload file
                    PutObjectResponse response = client.PutObjectAsync(request).Result;
                    if (response != null && response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return new ExecutionResult<bool>(true);
                    }
                }
                catch (AmazonServiceException) // catch just Amazon exception other exception should be logged and handled by global handler
                {
                    return new ExecutionResult<bool>(false);
                }
            }
            return new ExecutionResult<bool>(false);
        }
        #endregion



        #region Get File
        public ExecutionResult<bool> GetFile(string FileName)
        {
            using (AmazonS3Client client = new AmazonS3Client(_awsSettings.AccessKey, _awsSettings.SecretAccessKey, _region))
            {
                try
                {
                    var name = FileName.Replace("%2F", "/");
                    GetObjectRequest request = new GetObjectRequest
                    {
                        BucketName = _awsSettings.BucketName,
                        Key = name
                    };
                    GetObjectResponse response = client.GetObjectAsync(request).Result;
                    if (response != null && response.HttpStatusCode == HttpStatusCode.NoContent)
                    {
                        return new ExecutionResult<bool>(true);
                    }
                    return new ExecutionResult<bool>(false);
                }
                catch (AmazonServiceException ex)
                {

                    throw;
                }
            }
            return new ExecutionResult<bool>(false);
        }
        #endregion

        #region Delete File
        public async Task<ExecutionResult<bool>> DeleteFile(string FileName)
        {
            // create Amazon client
            using (AmazonS3Client client = new AmazonS3Client(_awsSettings.AccessKey, _awsSettings.SecretAccessKey, _region))
            {
                try
                {
                    var request = new DeleteObjectRequest
                    {
                        BucketName = _awsSettings.BucketName,
                        Key = FileName
                    };

                    var response = client.DeleteObjectAsync(request).Result;
                    if (response != null && response.HttpStatusCode == HttpStatusCode.NoContent)
                    {
                        return new ExecutionResult<bool>(true);
                    }
                }
                catch (AmazonServiceException)
                {
                    return new ExecutionResult<bool>(false);
                }
            }
            return new ExecutionResult<bool>(false);
        }
        #endregion
    }
}
