using System;
using System.Collections.Generic;

namespace Totem.Business.Core.DataTransferModels.Admin.Questionary
{
    public class CreateQuestionaryModel
    {
        public string Question { get; set; }
        public int QuestionTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPredefined { get; set; }
        public bool IsClinicTrailBase { get; set; }
        public bool IsMultipleGroup { get; set; }

        public int? OrderNo { get; set; }
        public string PlaceHolderText { get; set; }
        public int? NoOfControls { get; set; }
        public List<childItemRowscls> itemRows { get; set; }

    }
    public class childItemRowscls
    {
        public string PlaceHolderTextForMultiDropDown { get; set; }
        public List<Questionoption> childItemRows { get; set; }
    }
    public class Questionoption
    {
        public string option { get; set; }

        public long optionId { get; set; }
    }
    public class childItemRowsclsnew
    {
        public string PlaceHolderTextForMultiDropDown { get; set; }
        public List<QuestionoptionList> childItemRows { get; set; }
    }
    public class QuestionoptionList
    {
        public long key { get; set; }

        public  string value { get; set; }
    }
}
