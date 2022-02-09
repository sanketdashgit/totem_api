using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class VerifyProfile
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AdditionalInformation { get; set; }
        //public string Category { get; set; }       
       
    }
}
