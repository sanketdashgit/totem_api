using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class DashbordModel
    {
        public int? TotalUsers { get; set; }
        public int? BusinessUsers { get; set; }
        public int? Users { get; set; }
        public int? AdminUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int? TotalEvents { get; set; }
        public int? LiveEvents { get; set; }
        public int? NotLiveEvents { get; set; }
        public int? BusinessEvents { get; set; }
        public int? CurrentUsers { get; set; }
    }
}
