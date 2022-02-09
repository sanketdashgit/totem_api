using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class ContactU
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
