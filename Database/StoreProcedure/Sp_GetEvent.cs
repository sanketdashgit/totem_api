using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Database.StoreProcedure
{
    public class Sp_GetEvent
    {

        public long Id { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Details { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool? Status { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public bool Golive { get; set; }
        public bool Interest { get; set; }
        public bool Favorite { get; set; }
        public bool? ProfileVerified { get; set; }


    }
}
