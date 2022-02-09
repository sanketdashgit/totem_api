using System.Collections.Generic;

namespace Totem.Business.Core.DataTransferModels.Admin.Questionary
{
    public class CreateQuestionToInnovationMappingModel
    {
        public int QuestionId { get; set; }
        public List<int> InnovationId { get; set; }
    }
}
