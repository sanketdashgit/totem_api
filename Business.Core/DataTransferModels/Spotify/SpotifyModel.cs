using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels.Event;

namespace Totem.Business.Core.DataTransferModels.Spotify
{
    public class SpotifyModel
    {
        public long Id { get; set; }
        public List<Artists> Artists { get; set; }
        public List<Genres> Genres { get; set; }
        public List<FavouriteEvents> FavouriteEvents { get; set; }
        public List<FavouriteEvents> NextEvents { get; set; }
    }

    public class GetSpotifyModel
    {
        public long Id { get; set; }
        public List<Artists> Artists { get; set; }
        public List<Genres> Genres { get; set; }
        public List<EventModel> FavouriteEvents { get; set; }
    }
}
