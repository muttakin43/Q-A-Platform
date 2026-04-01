using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Q_A.Domain;

namespace Q_A.DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<QuestionTag> QuestionTags => Set<QuestionTag>();
        public DbSet<Vote> Votes => Set<Vote>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🔹 Question
            builder.Entity<Question>()
                .HasOne(q => q.User)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Question>()
                .HasOne(q => q.AcceptedAnswer)
                .WithMany()
                .HasForeignKey(q => q.AcceptedAnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Answer
            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Comment
            builder.Entity<Comment>()
                .HasOne(c => c.Question)
                .WithMany(q => q.Comments)
                .HasForeignKey(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Comment>()
                .HasOne(c => c.Answer)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AnswerId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Vote
            builder.Entity<Vote>()
                .HasOne(v => v.Question)
                .WithMany(q => q.Votes)
                .HasForeignKey(v => v.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Vote>()
                .HasOne(v => v.Answer)
                .WithMany(a => a.Votes)
                .HasForeignKey(v => v.AnswerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Vote>()
                .HasOne(v => v.Comment)
                .WithMany(c => c.Votes)
                .HasForeignKey(v => v.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.QuestionId, v.AnswerId, v.CommentId })
                .IsUnique();

            // 🔹 QuestionTag (Many-to-Many)
            builder.Entity<QuestionTag>()
                .HasIndex(qt => new { qt.QuestionId, qt.TagId })
                .IsUnique();

            builder.Entity<QuestionTag>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionTags)
                .HasForeignKey(qt => qt.QuestionId)
                .IsRequired(false);

            builder.Entity<QuestionTag>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionTags)
                .HasForeignKey(qt => qt.TagId)
                .IsRequired(false);

            builder.Entity<QuestionTag>()
                .HasQueryFilter(qt => !qt.Question.IsDeleted && !qt.Tag.IsDeleted);

            // 🔹 Soft Delete Global Filter
            builder.Entity<Question>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Answer>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Comment>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Tag>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Vote>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}