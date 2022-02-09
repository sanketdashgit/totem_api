using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.Questionary
{
    public class QuestionaryResponseTypeModel
    {
        public long ReferenceDataEntityId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public long QuestionTypeId { get; set; }
        public bool? IsPredefined { get; set; }
        public bool? IsClinicTrailBase { get; set; }
        public bool? IsMultipleGroup { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string PlaceHolderText { get; set; }
        public int? NoOfControls { get; set; }
        public int? OrderNo { get; set; }
        public List<childItemRowscls> itemRows { get; set; }
        public List<childItemRowsclsnew> itemRows1 { get; set; }

        public string QuestionId { get; set; }
    }
}
