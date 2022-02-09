using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    class TicketMaster
    {

      public  string id { get; set; }
      public  string keyword { get; set; }
      public  string attractionId { get; set; }
      public  string venueId { get; set; }
      public  string postalCode { get; set; }
      public  string latlong { get; set; }
      public  string radius { get; set; }
      public  string unit { get; set; }
      public  string source { get; set; }
      public  string locale { get; set; }
      public  string marketId { get; set; }
      public  string startDateTime { get; set; }
      public  string endDateTime { get; set; }
      public  string includeTBA { get; set; }
      public  string includeTBD { get; set; }
      public  string includeTest { get; set; }
      public  string size { get; set; }
      public  string page { get; set; }
      public  string sort { get; set; }
      public  string onsaleStartDateTime { get; set; }
      public  string onsaleEndDateTime { get; set; }
      public  Array city { get; set; }
      public  string countryCode { get; set; }
      public  string stateCode { get; set; }
      public  Array classificationName { get; set; }
      public  Array classificationId { get; set; }
      public  string dmaId { get; set; }
      public  Array localStartDateTime { get; set; }
      public  Array localStartEndDateTime { get; set; }
      public  Array startEndDateTime { get; set; }
      public  Array publicVisibilityStartDateTime { get; set; }
      public  Array preSaleDateTime { get; set; }
      public  string onsaleOnStartDate { get; set; }
      public  string onsaleOnAfterStartDate { get; set; }
      public  Array collectionId { get; set; }
      public  Array segmentId { get; set; }
      public  Array segmentName { get; set; }
      public  string includeFamily { get; set; }
      public  string promoterId { get; set; }
      public  Array genreId { get; set; }
      public  Array subGenreId { get; set; }
      public  Array typeId { get; set; }
      public  Array subTypeId { get; set; }
      public  string geoPoint { get; set; }
      public  string preferredCountry { get; set; }
      public  string includeSpellcheck { get; set; }
      public  Array domain { get; set; }

    }
}
