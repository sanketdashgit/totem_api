using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Business.Core.DataTransferModels.Spotify
{
    public partial class Artists
    {
        public long Id { get; set; }
        public string SpotifyId { get; set; }
        //public long ArtistsId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
      
    }
}
