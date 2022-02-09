using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.ProjectType
{
    public  class UpdateProjectTypeResponseModel : CreateProjectTypeResponseModel
    {
        [Required(ErrorMessage = "ProjectTypeId is required")]
        public int ProjectTypeId { get; set; }
    
    }
}
