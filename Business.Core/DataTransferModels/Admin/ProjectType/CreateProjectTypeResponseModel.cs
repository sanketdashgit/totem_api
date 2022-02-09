using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.ProjectType
{
    public class CreateProjectTypeResponseModel
    {
        [StringLength(100, ErrorMessage = "Project Type cannot be longer than 100 characters")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter project type")]
        public string ProjectTypeName { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
