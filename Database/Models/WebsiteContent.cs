using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class WebsiteContent
    {
        public WebsiteContent()
        {
            TblUserFiles = new HashSet<TblUserFile>();
        }

        public long Id { get; set; }
        public string Page { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Section { get; set; }

        public virtual ICollection<TblUserFile> TblUserFiles { get; set; }
    }
}
