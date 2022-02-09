using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblEvent
    {
        public TblEvent()
        {
            TblEventComments = new HashSet<TblEventComment>();
            TblEventUserFiles = new HashSet<TblEventUserFile>();
            TblMemories = new HashSet<TblMemorie>();
            TblPosts = new HashSet<TblPost>();
        }

        public long Id { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Details { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string VanueId { get; set; }

        public virtual ICollection<TblEventComment> TblEventComments { get; set; }
        public virtual ICollection<TblEventUserFile> TblEventUserFiles { get; set; }
        public virtual ICollection<TblMemorie> TblMemories { get; set; }
        public virtual ICollection<TblPost> TblPosts { get; set; }
    }
}
