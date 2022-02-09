using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class VDashbord
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
