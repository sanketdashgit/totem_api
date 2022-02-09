using System.Collections.Generic;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class RemovePostMediaModel
    {
        public long PostId {get;set;}
        public List<long> PostFileId { get; set; }
    }

   
}
