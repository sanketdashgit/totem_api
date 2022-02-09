using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTC.Business.Core.DataTransferModels.Admin.Questionary
{
    public class QuestionaryInnovationSPModel
    {
        public List<QuestionDetails> QuestionDetails { get; set; }
        public List<Questionoption2> Questionoption2 { get; set; }
        public List<childItemRowscls2> childItemRowscls2 { get; set; }
    }

    public class QuestionDetails
    {
        public int ReferenceDataEntityId { get; set; }
        public string Question { get; set; }
        public int QuestionTypeId { get; set; }
        public bool IsPredefined { get; set; }
        public bool IsClinicTrailBase { get; set; }
        public bool IsMultipleGroup { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int NoOfControls { get; set; }
        public int OrderNo { get; set; }
        //public List<childItemRowscls> parentitemRows { get; set; }
    }

    public class Questionoption2
    {
        public string Option { get; set; }
        public int OptionId { get; set; }
    }

    public class childItemRowscls2
    {
        public long ReferenceDataEntityId { get; set; }
        public long CategoryId { get; set; }
        public string PlaceHolderTextForMultiDropDown { get; set; }
        //  public List<Questionoption> childItemRows { get; set; }
    }

}
