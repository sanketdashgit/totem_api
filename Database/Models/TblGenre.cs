using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblGenre
    {
        public long Id { get; set; }
        public long GenreId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string SpotifyId { get; set; }
    }
}
