using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblBusiness
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string ComumuunicationEmailId { get; set; }
        public string ComumuunicationPhone { get; set; }
        public string Designation { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string LegalName { get; set; }
    }
}
