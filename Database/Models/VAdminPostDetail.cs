using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class VAdminPostDetail
    {
        public long PostId { get; set; }
        public string Caption { get; set; }
        public long? EventId { get; set; }
        public long Id { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string Image { get; set; }
        public int? BlockedCount { get; set; }
        public bool ProfileVerified { get; set; }
    }
}
