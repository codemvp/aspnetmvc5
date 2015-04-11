using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Transactions;

namespace MatchByteMovieApp.Models
{
    public class DbInitializer<TContext> : IDatabaseInitializer<TContext> where TContext : MovieDbContext, new()
    {
        protected MovieDbContext _context;
        protected const string Sql =
            "if (select DB_ID('{0}')) is not null "
            + "begin "
            + "alter database [{0}] set offline with rollback immediate; "
            + "alter database [{0}] set online; "
            + "drop database [{0}]; "
            + "end";

        public virtual void InitializeDatabase(TContext context)
        {
            if (DbExists(context))
            {
                if (context.Database.CompatibleWithModel(false))
                    return;
                DropDatabase(context);
            }
            context.Database.Create();
            Seed(context);
            context.SaveChanges();
        }

        protected virtual bool DbExists(TContext context)
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                return context.Database.Exists();
            }
        }

        protected virtual void DropDatabase(TContext context)
        {
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(Sql, context.Database.Connection.Database));
        }

        protected virtual void Seed(TContext context)
        {
            _context = context;

            Movie movie = new Movie()
            {
                Title = "MatchByte the movie",
                YearReleased = System.DateTime.Now,
                Rating = EnumRatings.G
            };
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }
    }

    public partial class MovieDbContext : DbContext        
    {
        public MovieDbContext()
            : base("name=DefaultConnection")
        {
        }
        public virtual DbSet<Movie> Movies { get; set; }
    }
}