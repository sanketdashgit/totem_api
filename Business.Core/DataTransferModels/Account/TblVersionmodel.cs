using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{

    public partial class TblVersionmodel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public partial class GetVersionmodel
    {      
        public string Type { get; set; }
        public string Version { get; set; }
       
    }

    public partial class Versionmodel
    {
        public bool? Ismandatory { get; set; }
        public bool? UpdateRequired { get; set; }
    }
}
