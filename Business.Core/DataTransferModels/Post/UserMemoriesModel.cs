using System.Collections.Generic;
using Totem.Business.Core.DataTransferModels.Event;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class UserMemoriesModel
    {
        public long PostId { get; set; }
        public long Id { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Details { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool IsPrivate { get; set; }
       // public List<TblEventFile> EventImages { get; set; }        
    }
}
