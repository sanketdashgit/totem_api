using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblEventfeed
    {
        public long FeedId { get; set; }
        public long Id { get; set; }
        public long EventId { get; set; }
        public bool Golive { get; set; }
        public bool Interest { get; set; }
        public bool Favorite { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RequestAccepted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
