using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.ProcedureType
{
    public class CreateProcedureTypeResponseModel
    {
        [StringLength(100, ErrorMessage = "Procedure Type cannot be longer than 250 characters")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter procedure type")]
        public string ProcedureTypeName { get; set; }

        [Required(ErrorMessage = "Please enter project type")]
        public int ProjectTypeId { get; set; }

        public bool IsActive { get; set; }
    }
}
