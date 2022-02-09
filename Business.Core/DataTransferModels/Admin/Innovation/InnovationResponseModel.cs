using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.Innovation
{
    public class InnovationResponseModel
    {
        public int InnovationId { get; set; }
        public string InnovationName { get; set; }
        public int ProcedureTypeId { get; set; }
        public string ProcedureTypeName { get; set; }
        public int ProjectTypeId { get; set; }
        public int? AccountId { get; set; }
        public string ProjectTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
