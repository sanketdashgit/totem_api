using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblPostFile
    {
        public TblPostFile()
        {
            TblPostFileLikes = new HashSet<TblPostFileLike>();
        }

        public long PostFileId { get; set; }
        public long PostId { get; set; }
        public string Downloadlink { get; set; }
        public string MediaType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Videolink { get; set; }

        public virtual TblPost Post { get; set; }
        public virtual ICollection<TblPostFileLike> TblPostFileLikes { get; set; }
    }
}
