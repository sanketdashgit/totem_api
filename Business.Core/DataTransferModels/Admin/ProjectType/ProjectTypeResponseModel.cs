using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.ProjectType
{
    public class ProjectTypeResponseModel
    {
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
