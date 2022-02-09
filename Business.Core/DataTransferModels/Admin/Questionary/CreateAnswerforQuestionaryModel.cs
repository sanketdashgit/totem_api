namespace DTC.Business.Core.DataTransferModels.Admin.Questionary
{
    public class CreateAnswerforQuestionaryModel
    {
        public long? ReferenceDataEntityId { get; set; }
        public long? CategoryId { get; set; }
        public string Answer { get; set; }
    }
}
