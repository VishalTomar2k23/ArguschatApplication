using ChatApp.Context.EntityClasses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Context
{
    public class ChatAppContext : DbContext
    {
        public ChatAppContext(DbContextOptions<ChatAppContext> options): base(options) 
        {
        }
       
             protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map stored procedure
            modelBuilder.Entity<ConversationResult>().HasNoKey().ToView("GetAllConversationByUserId");
            modelBuilder.Entity<ConversationResult>().HasNoKey().ToView("GetAllConversationByUserIdsBoth");
        }
        public virtual DbSet<Profile> Profiles { get; set; }

        public virtual DbSet<MessageText> Messages { get; set; }

        public virtual DbSet<ConversationResult> ConversationResults { get; set; }

        
    }
}
