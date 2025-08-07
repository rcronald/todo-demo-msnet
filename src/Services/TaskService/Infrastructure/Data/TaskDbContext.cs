using TodoApp.TaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.TaskService.Infrastructure.Data;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Task> Tasks { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TaskTag> TaskTags { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Domain.Entities.Task>().ToTable("tasks");
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Tag>().ToTable("tags");
        modelBuilder.Entity<TaskTag>().ToTable("task_tags");

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KeycloakUserId).HasColumnName("keycloak_user_id").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(e => e.KeycloakUserId).IsUnique().HasDatabaseName("idx_users_keycloak_user_id");
        });

        // Configure Task entity
        modelBuilder.Entity<Domain.Entities.Task>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("TEXT");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed").HasDefaultValue(false);
            entity.Property(e => e.Priority).HasColumnName("priority").HasMaxLength(10).HasDefaultValue("Medium");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            // Foreign key relationships
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Tasks)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Category)
                  .WithMany()
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.TaskTags)
                  .WithOne(tt => tt.Task)
                  .HasForeignKey(tt => tt.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Indexes matching SQL schema
            entity.HasIndex(e => e.UserId).HasDatabaseName("idx_tasks_user_id");
            entity.HasIndex(e => e.DueDate).HasDatabaseName("idx_tasks_due_date");
            entity.HasIndex(e => e.IsCompleted).HasDatabaseName("idx_tasks_is_completed");
            entity.HasIndex(e => e.CategoryId).HasDatabaseName("idx_tasks_category_id");
            
            // Priority constraint
            entity.HasCheckConstraint("CK_tasks_priority", "priority IN ('High', 'Medium', 'Low')");
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Color).HasColumnName("color").IsRequired().HasMaxLength(7);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure Tag entity
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Tags)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint and indexes matching SQL schema
            entity.HasIndex(e => new { e.UserId, e.Name }).IsUnique();
            entity.HasIndex(e => e.UserId).HasDatabaseName("idx_tags_user_id");
        });

        // Configure TaskTag entity (Many-to-Many junction)
        modelBuilder.Entity<TaskTag>(entity =>
        {
            entity.HasKey(e => new { e.TaskId, e.TagId });
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.Task)
                  .WithMany(t => t.TaskTags)
                  .HasForeignKey(e => e.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tag)
                  .WithMany(t => t.TaskTags)
                  .HasForeignKey(e => e.TagId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Indexes matching SQL schema
            entity.HasIndex(e => e.TaskId).HasDatabaseName("idx_task_tags_task_id");
            entity.HasIndex(e => e.TagId).HasDatabaseName("idx_task_tags_tag_id");
        });
    }
}