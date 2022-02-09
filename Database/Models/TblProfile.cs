using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblProfile
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AdditionalInformation { get; set; }
        public string Category { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
