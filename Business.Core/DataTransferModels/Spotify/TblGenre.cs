using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Business.Core.DataTransferModels.Spotify
{
    public partial class Genres
    {
        public long Id { get; set; }
        public string SpotifyId { get; set; }
       // public long GenreId { get; set; }
        public string Name { get; set; }
        
    }
}
