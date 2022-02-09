using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblMemorieFile
    {
        public long MemorieFileId { get; set; }
        public long MemorieId { get; set; }
        public string Downloadlink { get; set; }
        public string MediaType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Videolink { get; set; }
        public bool? IsPrivate { get; set; }

        public virtual TblMemorie Memorie { get; set; }
    }
}
