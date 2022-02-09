using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class Usermanagement
    {
        public Usermanagement()
        {
            TblFollowerFollowers = new HashSet<TblFollower>();
            TblFollowerUsers = new HashSet<TblFollower>();
            TblMemories = new HashSet<TblMemorie>();
            TblPostComments = new HashSet<TblPostComment>();
            TblPostFileLikes = new HashSet<TblPostFileLike>();
            TblPostLikes = new HashSet<TblPostLike>();
            TblPostShareds = new HashSet<TblPostShared>();
            TblPostThumbs = new HashSet<TblPostThumb>();
            TblPosts = new HashSet<TblPost>();
            TblSupports = new HashSet<TblSupport>();
        }

        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string Password { get; set; }
        public int? Role { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid? EmailToken { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public int? Gender { get; set; }
        public bool? IsMobileVerified { get; set; }
        public int? SignInType { get; set; }
        public int? MobileOtp { get; set; }
        public bool ProfileVerified { get; set; }
        public string Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public bool? IsPrivate { get; set; }
        public bool ByPostBlocked { get; set; }
        public bool? MessageNotification { get; set; }
        public bool? EventNotification { get; set; }
        public bool? FollowNotification { get; set; }
        public bool? InvalidLoginAttempts { get; set; }
        public int PresentLiveStatus { get; set; }
        public int ReferredBy { get; set; }
        public int TotemPoints { get; set; }

        public virtual ICollection<TblFollower> TblFollowerFollowers { get; set; }
        public virtual ICollection<TblFollower> TblFollowerUsers { get; set; }
        public virtual ICollection<TblMemorie> TblMemories { get; set; }
        public virtual ICollection<TblPostComment> TblPostComments { get; set; }
        public virtual ICollection<TblPostFileLike> TblPostFileLikes { get; set; }
        public virtual ICollection<TblPostLike> TblPostLikes { get; set; }
        public virtual ICollection<TblPostShared> TblPostShareds { get; set; }
        public virtual ICollection<TblPostThumb> TblPostThumbs { get; set; }
        public virtual ICollection<TblPost> TblPosts { get; set; }
        public virtual ICollection<TblSupport> TblSupports { get; set; }
    }
}
