﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Admin.Innovation
{
    public class UpdateInnovationResponseModel : CreateInnovationResponseModel
    {
        public int InnovationId { get; set; }
    }
}
