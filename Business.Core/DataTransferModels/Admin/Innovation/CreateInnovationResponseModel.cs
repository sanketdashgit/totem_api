using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.Innovation
{
    public class CreateInnovationResponseModel
    {
      
        [StringLength(250, ErrorMessage = "Innovation cannot be longer than 250 characters")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter Innovation")]
        public string InnovationName { get; set; }

        [Required(ErrorMessage = "Please enter procedure type")]
        public int ProcedureTypeId { get; set; }

        [Required(ErrorMessage = "Please enter project type")]
        public int ProjectTypeId { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
