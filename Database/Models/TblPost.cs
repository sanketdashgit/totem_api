using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblPost
    {
        public TblPost()
        {
            TblPostComments = new HashSet<TblPostComment>();
            TblPostFileLikes = new HashSet<TblPostFileLike>();
            TblPostFiles = new HashSet<TblPostFile>();
            TblPostLikes = new HashSet<TblPostLike>();
            TblPostShareds = new HashSet<TblPostShared>();
            TblPostThumbs = new HashSet<TblPostThumb>();
        }

        public long PostId { get; set; }
        public string Caption { get; set; }
        public long? EventId { get; set; }
        public long Id { get; set; }
        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsPrivate { get; set; }

        public virtual TblEvent Event { get; set; }
        public virtual Usermanagement IdNavigation { get; set; }
        public virtual ICollection<TblPostComment> TblPostComments { get; set; }
        public virtual ICollection<TblPostFileLike> TblPostFileLikes { get; set; }
        public virtual ICollection<TblPostFile> TblPostFiles { get; set; }
        public virtual ICollection<TblPostLike> TblPostLikes { get; set; }
        public virtual ICollection<TblPostShared> TblPostShareds { get; set; }
        public virtual ICollection<TblPostThumb> TblPostThumbs { get; set; }
    }
}
