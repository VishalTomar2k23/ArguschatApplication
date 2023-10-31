using System;
using System.Collections.Generic;
using ChatApp.Context.EntityClasses;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Context;

public partial class ArgusChatContext : DbContext
{
    public ArgusChatContext()
    {
    }

    public ArgusChatContext(DbContextOptions<ArgusChatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupMember> GroupMembers { get; set; }

    public virtual DbSet<GroupMessage> GroupMessages { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ConversationResult> ConversationResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Map stored procedure
        modelBuilder.Entity<ConversationResult>().HasNoKey().ToView("GetAllConversationByUserId");
        modelBuilder.Entity<ConversationResult>().HasNoKey().ToView("GetAllConversationByUserIdsBoth");

        

        modelBuilder.Entity<Notification>()
                .HasOne(n => n.Receiver)
                .WithMany(p => p.NotificationReceivers)
                .HasForeignKey(n => n.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Sender)
            .WithMany(p => p.NotificationSenders)
            .HasForeignKey(n => n.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Connection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Connecti__3214EC0752B5B5B3");

            entity.HasIndex(e => e.ProfileId, "UQ__Connecti__290C88E566218098").IsUnique();

            entity.Property(e => e.DateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SignalId).HasMaxLength(22);

            entity.HasOne(d => d.Profile).WithOne(p => p.Connection)
                .HasForeignKey<Connection>(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_ProfileId_To_Profiles");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Groups__3214EC070D064E64");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.GroupName).HasMaxLength(50);
            entity.Property(e => e.ImagePath).HasMaxLength(1000);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Groups_CreatedBy_To_Profiles");
        });

        modelBuilder.Entity<GroupMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupMem__3214EC07C964097E");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupMembers_GrpId_To_Groups");

            entity.HasOne(d => d.Profile).WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupMembers_ProfileId_To_Profiles");
        });

        modelBuilder.Entity<GroupMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupMes__3214EC0774F16260");

            entity.Property(e => e.Content).HasMaxLength(1000);
            entity.Property(e => e.RepliedContent).HasMaxLength(1000);
            entity.Property(e => e.Type).HasMaxLength(30);

            entity.HasOne(d => d.Group).WithMany(p => p.GroupMessages)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupMessages_GrpId_To_Groups");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0745632285");

            entity.Property(e => e.Content).HasMaxLength(1000);
            entity.Property(e => e.RepliedContent).HasMaxLength(1000);
            entity.Property(e => e.Type).HasMaxLength(30);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0783D43A3E");

            entity.Property(e => e.Content).HasMaxLength(50);

            entity.HasOne(d => d.Group).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Notifications_GroupID_To_Groups");

            entity.HasOne(d => d.Receiver).WithMany(p => p.NotificationReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_ToId_To_Profiles");

            entity.HasOne(d => d.Sender).WithMany(p => p.NotificationSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_FromId_To_Profiles");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC076B648D03");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Designation).HasDefaultValueSql("((1))");
            entity.Property(e => e.Email)
                .HasMaxLength(1000)
                .HasColumnName("Email ");
            entity.Property(e => e.FirstName).HasMaxLength(1000);
            entity.Property(e => e.ImagePath).HasMaxLength(1000);
            entity.Property(e => e.LastName).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(40);
            entity.Property(e => e.UserName).HasMaxLength(1000);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    
}
