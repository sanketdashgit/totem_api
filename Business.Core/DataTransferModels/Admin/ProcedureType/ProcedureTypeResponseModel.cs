using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.ProcedureType
{
    public class ProcedureTypeResponseModel
    {
        public int ProcedureTypeId { get; set; }
        public string ProcedureTypeName { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
