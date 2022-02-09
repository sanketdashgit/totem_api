using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.ProcedureType
{
   public class UpdateProcedureTypeResponseModel : CreateProcedureTypeResponseModel
    {
        public int ProcedureTypeId { get; set; }
    }
}
