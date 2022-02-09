using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Database.StoreProcedure
{
    public partial class Sp_GetUserDetails
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string Image { get; set; }
        public bool? BussinessUser { get; set; }
        public bool ProfileVerified { get; set; }
        public string IsBusinessRequestSend { get; set; }
        public string IsProfileVarificationRequestSend { get; set; }
        public bool? IsPrivate { get; set; }
    }
}
