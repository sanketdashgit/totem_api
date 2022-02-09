using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblUserFile
    {
        public long FileId { get; set; }
        public long? Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Downloadlink { get; set; }
        public string Title { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }

        public virtual WebsiteContent IdNavigation { get; set; }
    }
}
