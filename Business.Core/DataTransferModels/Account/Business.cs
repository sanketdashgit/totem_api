using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class BusinessModel
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string LegalName { get; set; }
        public string ComumuunicationEmailId { get; set; }
        public string ComumuunicationPhone { get; set; }
        public string Designation { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        
    }

    public class GetBusinessModel
    {
        public BusinessModel BusinessModel { get; set; }
        public VerifyProfile VerifyProfile { get; set; }
    }
}
