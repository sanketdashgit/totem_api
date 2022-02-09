using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblEventUserFile
    {
        public long FileId { get; set; }
        public long EventId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Downloadlink { get; set; }
        public string Title { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }

        public virtual TblEvent Event { get; set; }
    }
}
