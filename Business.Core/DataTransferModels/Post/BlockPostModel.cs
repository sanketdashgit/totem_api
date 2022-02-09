using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class BlockPostModel
    {
        public long Id { get; set; }
        public long BlockId { get; set; }
        public long PostuserId { get; set; }
        public long PostId { get; set; }
        public string Reason { get; set; }        
    }

    public class BlockPostResponce
    {
        public long Id { get; set; }
        public long BlockId { get; set; }
        public long PostuserId { get; set; }
        public long PostId { get; set; }
        public string Reason { get; set; }
        public int PostBlockCount { get; set; }
    }
}
