using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.Consts.Admin
{
    public static class AdminMessageHelper
    {
        public const string SuccessMessage = "{0} {1} successfully";
        public const string NoFound = "No {0} Found";
        public const string NullMessage = "{0} can't  be null";
        public const string IsExistsMessage = "{0} is already exists!!";
        public const string PassParaMessage = "Please Pass {0}";
        public const string ProjectTypeMappedwithProcedure = "Project Type can not be deleted as {0} type is mapped with procedure";
        public const string ProjectTypeMappedwithInnovation = "Project Type can not be deleted as {0} type is mapped with innovation";
        public const string ProcedureTypeMappedwithInnovation = "Procedure Type can not be deleted as {0} type is mapped with innovation";
        public const string checkOrderExists = "Order is already assigned to another question.Kindly choose another order.";
    }
}
