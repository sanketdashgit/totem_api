using System;
using System.Collections.Generic;
using Totem.Business.Core.DataTransferModels.Event;

#nullable disable

namespace Totem.Business.Core.DataTransferModels.Spotify
{
    public partial class FavouriteEvents
    {

        //public long Favourite { get; set; }
        public long Id { get; set; }
        public long EventId { get; set; }

    }
}
