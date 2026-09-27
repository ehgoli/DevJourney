using DevJourney.Domain.Entities.Articles;
using DevJourney.Domain.Entities.Courses;
using DevJourney.Domain.Entities.Identity;
using DevJourney.Domain.Entities.Portfolio;
using DevJourney.Domain.Entities.Profile;
using DevJourney.Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DevJourney.Infrastructure.Persistence.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region Tables

    #region Profile

    public DbSet<Profile> Profiles => Set<Profile>();

    public DbSet<ProfileTranslation> ProfileTranslations =>
        Set<ProfileTranslation>();

    public DbSet<Skill> Skills => Set<Skill>();

    public DbSet<Timeline> Timelines => Set<Timeline>();

    public DbSet<TimelineTranslation> TimelineTranslations =>
        Set<TimelineTranslation>();

    public DbSet<SocialMedia> SocialMedias => Set<SocialMedia>();

    #endregion

    #region Portfolio

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<ProjectTranslation> ProjectTranslations =>
        Set<ProjectTranslation>();

    public DbSet<ProjectImage> ProjectImages =>
        Set<ProjectImage>();

    public DbSet<Contributor> Contributors =>
        Set<Contributor>();

    public DbSet<Achievement> Achievements =>
        Set<Achievement>();

    public DbSet<AchievementTranslation> AchievementTranslations =>
        Set<AchievementTranslation>();

    #endregion

    #region Articles

    public DbSet<Article> Articles =>
        Set<Article>();

    public DbSet<ArticleTranslation> ArticleTranslations =>
        Set<ArticleTranslation>();

    public DbSet<Category> Categories =>
        Set<Category>();

    public DbSet<CategoryTranslation> CategoryTranslations =>
        Set<CategoryTranslation>();

    public DbSet<Comment> Comments =>
        Set<Comment>();

    #endregion

    #region Courses

    public DbSet<Course> Courses =>
        Set<Course>();

    public DbSet<Episode> Episodes =>
        Set<Episode>();

    #endregion

    #region Identity

    public DbSet<User> Users =>
        Set<User>();

    public DbSet<Role> Roles =>
        Set<Role>();

    public DbSet<UserRole> UserRoles =>
        Set<UserRole>();

    #endregion
    
    #endregion

    #region Persistence

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(
                cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new PersistenceException(
                "The data was modified by another operation.",
                PersistenceErrorType.ConcurrencyConflict,
                ex);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlException)
        {
            var errorType = sqlException.Number switch
            {
                2601 or 2627 =>
                    PersistenceErrorType.DuplicateData,

                547 or 8152 or 2628 =>
                    PersistenceErrorType.ConstraintViolation,

                1205 =>
                    PersistenceErrorType.TransientFailure,

                _ =>
                    PersistenceErrorType.Unknown
            };

            var message = errorType switch
            {
                PersistenceErrorType.DuplicateData =>
                    "The provided data already exists in the system.",

                PersistenceErrorType.ConstraintViolation =>
                    "The operation failed because of a database constraint.",

                PersistenceErrorType.TransientFailure =>
                    "The database operation failed temporarily.",

                PersistenceErrorType.ConcurrencyConflict =>
                    "The data was modified by another operation.",

                _ =>
                    "An unexpected database error occurred."
            };

            throw new PersistenceException(
                message,
                errorType,
                ex);
        }
    }

    #endregion

    #region Configuration
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}