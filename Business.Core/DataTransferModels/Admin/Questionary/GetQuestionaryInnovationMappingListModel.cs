using System;
using System.Collections.Generic;

namespace Totem.Business.Core.DataTransferModels.Admin.Questionary
{
    public class GetQuestionaryInnovationMappingListModel
    {
        public int QuestionInnovationMappingId { get; set; }
        public long ReferenceDataEntityId { get; set; }
        public string Question { get; set; }
        public int QuestionId { get; set; }
        public List<string> InnovationName { get; set; }
        public List<int> InnovationId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
