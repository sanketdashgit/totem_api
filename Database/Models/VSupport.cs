using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class VSupport
    {
        public long SupportId { get; set; }
        public long Id { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string Username { get; set; }
        public bool? ProfileVerified { get; set; }
    }
}
