using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.Questionary
{
    public class QuestionResponseModel
    {
        public long QuestionID { get; set; }
        public string Question { get; set; }
        public int OrderNo { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionType { get; set; }
        public int NoOfControls { get; set; }
        public bool? IsClinicTrailBase { get; set; }
        public bool? IsPredefined { get; set; }
        public bool? IsMultipleGroup { get; set; }
        public bool? IsActive { get; set; }

        public long OptionId { get; set; }
        public string Option { get; set; }
        public int? CategoryId { get; set; }
        public string PlaceHolderText { get; set; }

    }
}
