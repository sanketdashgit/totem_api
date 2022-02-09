namespace Totem.Business.Core.DataTransferModels.Admin.Questionary
{
    public class UpdateQuestionToInnovationMappingModel : CreateQuestionToInnovationMappingModel
    {
        public int QuestionInnovationMappingId { get; set; }
        public string Question { get; set; }
        public string InnovationName { get; set; }
    }
}
