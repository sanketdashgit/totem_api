using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class BlockUserModel
    {

        public long Id { get; set; }
        public long BlockId { get; set; }
        // public bool isFollow { get; set; }


    }

    public class PresentLiveStatusModel
    {
        public long Id { get; set; }
        public int PresentLiveStatus { get; set; }
    }
}
