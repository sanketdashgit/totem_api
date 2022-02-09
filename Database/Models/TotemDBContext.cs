using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TotemDBContext : DbContext
    {
        public TotemDBContext()
        {
        }

        public TotemDBContext(DbContextOptions<TotemDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContactU> ContactUs { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<TblArtist> TblArtists { get; set; }
        public virtual DbSet<TblBlockPost> TblBlockPosts { get; set; }
        public virtual DbSet<TblBlockUser> TblBlockUsers { get; set; }
        public virtual DbSet<TblBusiness> TblBusinesses { get; set; }
        public virtual DbSet<TblCategory> TblCategories { get; set; }
        public virtual DbSet<TblDeleteUser> TblDeleteUsers { get; set; }
        public virtual DbSet<TblEarlyToParty> TblEarlyToParties { get; set; }
        public virtual DbSet<TblEvent> TblEvents { get; set; }
        public virtual DbSet<TblEventAboutfeed> TblEventAboutfeeds { get; set; }
        public virtual DbSet<TblEventComment> TblEventComments { get; set; }
        public virtual DbSet<TblEventCommentsReply> TblEventCommentsReplies { get; set; }
        public virtual DbSet<TblEventUserFile> TblEventUserFiles { get; set; }
        public virtual DbSet<TblEventfeed> TblEventfeeds { get; set; }
        public virtual DbSet<TblFavouriteEvent> TblFavouriteEvents { get; set; }
        public virtual DbSet<TblFollower> TblFollowers { get; set; }
        public virtual DbSet<TblGenre> TblGenres { get; set; }
        public virtual DbSet<TblMemorie> TblMemories { get; set; }
        public virtual DbSet<TblMemorieFile> TblMemorieFiles { get; set; }
        public virtual DbSet<TblNotification> TblNotifications { get; set; }
        public virtual DbSet<TblPost> TblPosts { get; set; }
        public virtual DbSet<TblPostComment> TblPostComments { get; set; }
        public virtual DbSet<TblPostCommentReply> TblPostCommentReplies { get; set; }
        public virtual DbSet<TblPostCommentfeed> TblPostCommentfeeds { get; set; }
        public virtual DbSet<TblPostFile> TblPostFiles { get; set; }
        public virtual DbSet<TblPostFileLike> TblPostFileLikes { get; set; }
        public virtual DbSet<TblPostLike> TblPostLikes { get; set; }
        public virtual DbSet<TblPostShared> TblPostShareds { get; set; }
        public virtual DbSet<TblPostThumb> TblPostThumbs { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }
        public virtual DbSet<TblProfile> TblProfiles { get; set; }
        public virtual DbSet<TblSong> TblSongs { get; set; }
        public virtual DbSet<TblSubcategory> TblSubcategories { get; set; }
        public virtual DbSet<TblSupport> TblSupports { get; set; }
        public virtual DbSet<TblTagPost> TblTagPosts { get; set; }
        public virtual DbSet<TblUpdateEmail> TblUpdateEmails { get; set; }
        public virtual DbSet<TblUserFcm> TblUserFcms { get; set; }
        public virtual DbSet<TblUserFile> TblUserFiles { get; set; }
        public virtual DbSet<TblVersion> TblVersions { get; set; }
        public virtual DbSet<Usermanagement> Usermanagements { get; set; }
        public virtual DbSet<VAdminPostDetail> VAdminPostDetails { get; set; }
        public virtual DbSet<VDashbord> VDashbords { get; set; }
        public virtual DbSet<VEventDetail> VEventDetails { get; set; }
        public virtual DbSet<VGetUserdetail> VGetUserdetails { get; set; }
        public virtual DbSet<VPostDetail> VPostDetails { get; set; }
        public virtual DbSet<VSupport> VSupports { get; set; }
        public virtual DbSet<WebsiteContent> WebsiteContents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=18.191.237.171;Initial Catalog=TotemDB;User ID=sa;Password=Admin@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ContactU>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Message).HasMaxLength(500);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("FAQ");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Question).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Role1)
                    .IsRequired()
                    .HasColumnName("role");
            });

            modelBuilder.Entity<TblArtist>(entity =>
            {
                entity.HasKey(e => e.ArtistsId)
                    .HasName("PK__Tbl_Arti__60D5597F7E473F25");

                entity.ToTable("Tbl_Artists");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(300);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SpotifyId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SpotifyID");
            });

            modelBuilder.Entity<TblBlockPost>(entity =>
            {
                entity.HasKey(e => e.BlockId)
                    .HasName("PK__Tbl_Bloc__A84895A60CC7CEB1");

                entity.ToTable("Tbl_BlockPost");

                entity.Property(e => e.BlockId).HasColumnName("Block_Id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostuserId).HasColumnName("PostuserID");

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBlockUser>(entity =>
            {
                entity.HasKey(e => e.Blockuserid)
                    .HasName("PK__Tbl_Bloc__FCA6BC28A2B433E4");

                entity.ToTable("Tbl_BlockUser");

                entity.Property(e => e.BlockId).HasColumnName("Block_Id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblBusiness>(entity =>
            {
                entity.HasKey(e => e.BusinessId)
                    .HasName("PK__Tbl_Busi__F1EAA36ECCD41347");

                entity.ToTable("Tbl_Business");

                entity.Property(e => e.ComumuunicationEmailId)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ComumuunicationPhone)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Designation)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LegalName)
                    .HasMaxLength(200)
                    .HasColumnName("legalName");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrganizationAddress)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tbl_Category");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TblDeleteUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Tbl_DeleteUser");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeleteUserId).HasColumnName("DeleteUser_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<TblEarlyToParty>(entity =>
            {
                entity.ToTable("Tbl_EarlyToParty");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK_Event");

                entity.ToTable("Tbl_Event");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Latitude).HasMaxLength(100);

                entity.Property(e => e.Longitude).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VanueId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("vanueId");
            });

            modelBuilder.Entity<TblEventAboutfeed>(entity =>
            {
                entity.HasKey(e => e.FeedId)
                    .HasName("PK__Tbl_Even__A0A7D53F451FAB1D");

                entity.ToTable("Tbl_EventAboutfeeds");

                entity.Property(e => e.FeedId).HasColumnName("feedID");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TblEventAboutfeeds)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("FK__Tbl_Event__Comme__489AC854");
            });

            modelBuilder.Entity<TblEventComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK__Tbl_Even__C3B4DFAAA8D31006");

                entity.ToTable("Tbl_EventComments");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.CommentBody).HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReplyBody).HasMaxLength(500);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.TblEventComments)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tbl_EventComments_Tbl_Event");
            });

            modelBuilder.Entity<TblEventCommentsReply>(entity =>
            {
                entity.HasKey(e => e.ReplyId)
                    .HasName("PK__Tbl_Even__C25E46294897EA7A");

                entity.ToTable("Tbl_EventCommentsReply");

                entity.Property(e => e.ReplyId).HasColumnName("ReplyID");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReplyBody).HasMaxLength(500);

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TblEventCommentsReplies)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tbl_EventCommentsReply_Tbl_EventComments");
            });

            modelBuilder.Entity<TblEventUserFile>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK__Tbl_Even__6F0F98BF0AFFF994");

                entity.ToTable("Tbl_EventUserFile");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Downloadlink).HasMaxLength(500);

                entity.Property(e => e.FileName).HasMaxLength(100);

                entity.Property(e => e.FileType).HasMaxLength(30);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.TblEventUserFiles)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_Event__Event__4B7734FF");
            });

            modelBuilder.Entity<TblEventfeed>(entity =>
            {
                entity.HasKey(e => e.FeedId)
                    .HasName("PK__Tbl_Even__A0A7D53F4B00C5E3");

                entity.ToTable("Tbl_Eventfeeds");

                entity.Property(e => e.FeedId).HasColumnName("feedID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblFavouriteEvent>(entity =>
            {
                entity.HasKey(e => e.Favourite)
                    .HasName("PK__Tbl_favo__CE4A6E0C2BEBAB1E");

                entity.ToTable("Tbl_favouriteEvent");

                entity.Property(e => e.Favourite).HasColumnName("favourite");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblFollower>(entity =>
            {
                entity.HasKey(e => e.FId)
                    .HasName("pk_Usermanagement_Tbl_follower");

                entity.ToTable("Tbl_follower");

                entity.Property(e => e.FId).HasColumnName("F_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FollowerId).HasColumnName("follower_id");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Follower)
                    .WithMany(p => p.TblFollowerFollowers)
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tbl_follower_Usermanagement1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblFollowerUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tbl_follower_Usermanagement");
            });

            modelBuilder.Entity<TblGenre>(entity =>
            {
                entity.HasKey(e => e.GenreId)
                    .HasName("PK__Tbl_Genr__0385057EA753AE5A");

                entity.ToTable("Tbl_Genre");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(300);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SpotifyId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SpotifyID");
            });

            modelBuilder.Entity<TblMemorie>(entity =>
            {
                entity.HasKey(e => e.MemorieId)
                    .HasName("PK_Tbl_Memorie_MemorieId");

                entity.ToTable("Tbl_Memorie");

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.TblMemories)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Tbl_Memor__Event__13F1F5EB");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblMemories)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_Memorie__Id__14E61A24");
            });

            modelBuilder.Entity<TblMemorieFile>(entity =>
            {
                entity.HasKey(e => e.MemorieFileId)
                    .HasName("PK_Tbl_MemorieFile_MemorieFileId");

                entity.ToTable("Tbl_MemorieFile");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Downloadlink)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsPrivate).HasDefaultValueSql("((0))");

                entity.Property(e => e.MediaType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Videolink)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Memorie)
                    .WithMany(p => p.TblMemorieFiles)
                    .HasForeignKey(d => d.MemorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_Memor__Memor__19AACF41");
            });

            modelBuilder.Entity<TblNotification>(entity =>
            {
                entity.HasKey(e => e.Ssrno)
                    .HasName("PK__Tbl_noti__613045CC885E38A0");

                entity.ToTable("Tbl_notifications");

                entity.Property(e => e.Ssrno).HasColumnName("ssrno");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Descp)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("descp")
                    .IsFixedLength(true);

                entity.Property(e => e.Image).HasMaxLength(500);

                entity.Property(e => e.NotificationType)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationTypeId).HasColumnName("NotificationTypeID");

                entity.Property(e => e.NuserName)
                    .HasMaxLength(200)
                    .HasColumnName("Nuser_name");

                entity.Property(e => e.Readflag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("readflag")
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPost>(entity =>
            {
                entity.HasKey(e => e.PostId)
                    .HasName("PK_Tbl_Post_PostId");

                entity.ToTable("Tbl_Post");

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.TblPosts)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Tbl_Post__EventI__4E53A1AA");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblPosts)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_Post__Id__4F47C5E3");
            });

            modelBuilder.Entity<TblPostComment>(entity =>
            {
                entity.HasKey(e => e.PostCommentId)
                    .HasName("PK_Tbl_PostComment_PostCommentId");

                entity.ToTable("Tbl_PostComment");

                entity.Property(e => e.Comment).HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblPostComments)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostComm__Id__51300E55");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.TblPostComments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostC__PostI__503BEA1C");
            });

            modelBuilder.Entity<TblPostCommentReply>(entity =>
            {
                entity.HasKey(e => e.ReplyId)
                    .HasName("PK__Tbl_Post__C25E46290F241772");

                entity.ToTable("Tbl_PostCommentReply");

                entity.Property(e => e.ReplyId).HasColumnName("ReplyID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReplyBody).HasMaxLength(500);

                entity.HasOne(d => d.PostComment)
                    .WithMany(p => p.TblPostCommentReplies)
                    .HasForeignKey(d => d.PostCommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tbl_PostCommentReply_Tbl_PostComment");
            });

            modelBuilder.Entity<TblPostCommentfeed>(entity =>
            {
                entity.HasKey(e => e.FeedId)
                    .HasName("PK__Tbl_Post__A0A7D53F86DA8762");

                entity.ToTable("Tbl_PostCommentfeeds");

                entity.Property(e => e.FeedId).HasColumnName("feedID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PostComment)
                    .WithMany(p => p.TblPostCommentfeeds)
                    .HasForeignKey(d => d.PostCommentId)
                    .HasConstraintName("FK__Tbl_PostC__PostC__5224328E");
            });

            modelBuilder.Entity<TblPostFile>(entity =>
            {
                entity.HasKey(e => e.PostFileId)
                    .HasName("PK_Tbl_PostFile_PostFileId");

                entity.ToTable("Tbl_PostFile");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Downloadlink)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.MediaType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Videolink)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.TblPostFiles)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostF__PostI__540C7B00");
            });

            modelBuilder.Entity<TblPostFileLike>(entity =>
            {
                entity.HasKey(e => e.PostLikeId)
                    .HasName("PK_Tbl_PostFileLikes_PostLikeId");

                entity.ToTable("Tbl_PostFileLikes");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblPostFileLikes)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostFile__Id__3BFFE745");

                entity.HasOne(d => d.PostFile)
                    .WithMany(p => p.TblPostFileLikes)
                    .HasForeignKey(d => d.PostFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostF__PostF__3A179ED3");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.TblPostFileLikes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostF__PostI__3B0BC30C");
            });

            modelBuilder.Entity<TblPostLike>(entity =>
            {
                entity.HasKey(e => e.PostLikeId)
                    .HasName("PK_Tbl_PostLikes_PostLikeId");

                entity.ToTable("Tbl_PostLikes");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblPostLikes)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostLike__Id__55F4C372");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.TblPostLikes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostL__PostI__55009F39");
            });

            modelBuilder.Entity<TblPostShared>(entity =>
            {
                entity.HasKey(e => e.PostSharedId)
                    .HasName("PK_Tbl_PostShared_PostSharedId");

                entity.ToTable("Tbl_PostShared");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblPostShareds)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostShar__Id__57DD0BE4");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.TblPostShareds)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostS__PostI__56E8E7AB");
            });

            modelBuilder.Entity<TblPostThumb>(entity =>
            {
                entity.HasKey(e => e.PostThumbId)
                    .HasName("PK_Tbl_PostThumb_PostThumbId");

                entity.ToTable("Tbl_PostThumb");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblPostThumbs)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostThum__Id__59C55456");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.TblPostThumbs)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_PostT__PostI__58D1301D");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.ToTable("Tbl_Product");

                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblProfile>(entity =>
            {
                entity.HasKey(e => e.ProfileId)
                    .HasName("PK__Tbl_Prof__290C88E419423C13");

                entity.ToTable("Tbl_Profile");

                entity.Property(e => e.AdditionalInformation)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FullName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblSong>(entity =>
            {
                entity.HasKey(e => e.SongId)
                    .HasName("PK__Tbl_Song__12E3D697B5313605");

                entity.ToTable("Tbl_Songs");

                entity.Property(e => e.AlbumId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("albumId")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AlbumName).HasColumnName("albumName");

                entity.Property(e => e.ArtistName).HasColumnName("artistName");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(300);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Mp3Song).HasColumnName("mp3Song");

                entity.Property(e => e.Songlink).HasMaxLength(300);

                entity.Property(e => e.SpotifyId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SpotifyID");

                entity.Property(e => e.TrackName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("trackName");
            });

            modelBuilder.Entity<TblSubcategory>(entity =>
            {
                entity.ToTable("tbl_Subcategory");

                entity.Property(e => e.SubcategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblSupport>(entity =>
            {
                entity.HasKey(e => e.SupportId)
                    .HasName("PK__Tbl_Supp__D82DBC6C80277FB0");

                entity.ToTable("Tbl_Support");

                entity.Property(e => e.SupportId).HasColumnName("SupportID");

                entity.Property(e => e.Body).HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblSupports)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tbl_Support_Usermanagement");
            });

            modelBuilder.Entity<TblTagPost>(entity =>
            {
                entity.HasKey(e => e.TagId)
                    .HasName("PK__Tbl_tagP__50FC0157C8A9484E");

                entity.ToTable("Tbl_tagPost");

                entity.Property(e => e.TagId).HasColumnName("tagId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TagUserId).HasColumnName("TagUserID");
            });

            modelBuilder.Entity<TblUpdateEmail>(entity =>
            {
                entity.ToTable("Tbl_UpdateEmails");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUserFcm>(entity =>
            {
                entity.HasKey(e => e.FcmId)
                    .HasName("PK__Tbl_User__3C58EEF020474F76");

                entity.ToTable("Tbl_UserFCM");

                entity.Property(e => e.FcmId).HasColumnName("FCM_Id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Fcm)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("FCM");

                entity.Property(e => e.Login).HasColumnName("login");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TblUserFile>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK__Tbl_User__6F0F98BF93AB7AF7");

                entity.ToTable("Tbl_UserFile");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Downloadlink).HasMaxLength(500);

                entity.Property(e => e.FileName).HasMaxLength(100);

                entity.Property(e => e.FileType).HasMaxLength(30);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TblUserFiles)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK__Tbl_UserFile__Id__5BAD9CC8");
            });

            modelBuilder.Entity<TblVersion>(entity =>
            {
                entity.ToTable("Tbl_Version");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Ismandatory)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Usermanagement>(entity =>
            {
                entity.ToTable("Usermanagement");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bio)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BirthDate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EventNotification).HasDefaultValueSql("((1))");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FollowNotification).HasDefaultValueSql("((1))");

                entity.Property(e => e.Gender).HasDefaultValueSql("((0))");

                entity.Property(e => e.Image).HasDefaultValueSql("('')");

                entity.Property(e => e.InvalidLoginAttempts)
                    .HasColumnName("invalidLoginAttempts")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsMobileVerified).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsPrivate).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.MessageNotification).HasDefaultValueSql("((1))");

                entity.Property(e => e.MobileOtp).HasColumnName("MobileOTP");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PresentLiveStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.SignInType).HasDefaultValueSql("((0))");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VAdminPostDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_adminPostDetails");

                entity.Property(e => e.BirthDate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BlockedCount).HasColumnName("blockedCount");

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VDashbord>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Dashbord");
            });

            modelBuilder.Entity<VEventDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_EventDetails");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.BirthDate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasMaxLength(100);

                entity.Property(e => e.Longitude).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VGetUserdetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_GetUserdetails");

                entity.Property(e => e.BirthDate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BussinessUser).HasColumnName("Bussiness user");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VPostDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_PostDetails");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Details).IsRequired();

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EventCreatdate).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VSupport>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Support");

                entity.Property(e => e.Body).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SupportId).HasColumnName("SupportID");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WebsiteContent>(entity =>
            {
                entity.ToTable("WebsiteContent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Page).HasMaxLength(255);

                entity.Property(e => e.Section)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
