using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblSong
    {
        public long Id { get; set; }
        public long SongId { get; set; }
        public string TrackName { get; set; }
        public string Image { get; set; }
        public string Songlink { get; set; }
        public string Mp3Song { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string SpotifyId { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumId { get; set; }
    }
}
