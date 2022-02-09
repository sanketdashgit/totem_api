using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Business.Core.DataTransferModels.Spotify
{
    public partial class Songs
    {
        public long Id { get; set; }
        //public long SongId { get; set; }
        public string SpotifyId { get; set; }
        //public long SongId { get; set; }
        public string TrackName { get; set; }
        public string Image { get; set; }
        public string Songlink { get; set; }
        public string Mp3Song { get; set; }
        public string CreatedBy { get; set; }
        public string AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }

    }

    public partial class Spotifysongs
    {
        public long Id { get; set; }
        public List<Songs> Songs { get; set; }
       

    }
}
