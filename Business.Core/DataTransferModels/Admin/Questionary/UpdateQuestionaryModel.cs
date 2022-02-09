using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.Questionary
{
   public  class UpdateQuestionaryModel : CreateQuestionaryModel
    {
        public long ReferenceDataEntityId { get; set; }
    }
}
