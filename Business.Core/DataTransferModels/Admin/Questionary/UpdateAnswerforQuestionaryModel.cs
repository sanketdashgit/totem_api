using DTC.Business.Core.DataTransferModels.Admin.Questionary;

namespace DTC.Business.Core.DataTransferModels.Admin.Questionary
{
    public class UpdateAnswerforQuestionaryModel : CreateAnswerforQuestionaryModel
    {
        public long ReferenceDataEntityValueId { get; set; }
    }
}
