using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.Questionary
{
    public class QuestionaryTypeResponseTypeModel
    {
        public int QuestionTypeId { get; set; }

        public string QuestionType { get; set; }

        public int OrderBy { get; set; }

        public int IsActive { get; set; }
    }
}
