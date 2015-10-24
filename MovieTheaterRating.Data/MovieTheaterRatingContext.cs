using MovieTheaterRating.Data.Configurations;
using MovieTheaterRating.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Data
{
    public class MovieTheaterRatingContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }

        public MovieTheaterRatingContext()
            : base(nameOrConnectionString: ConnectionStringName) { }

        public MovieTheaterRatingContext(string connectionString) : base(connectionString)
        {   }

        public MovieTheaterRatingContext(DbConnection connection)
            : base(connection, true)
        {   }

        //=====NOTE: do not use this static constructor if to use Effort to mock db ===
        //**** UNCOMMENT THIS STATIC CONSTRUCTOR OUT IN ORDER TO RUN WITH CONSOLE APP TO POPULATE DATABASE ****//
        static MovieTheaterRatingContext()
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        public static string ConnectionStringName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConnectionStringName"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["ConnectionStringName"].ToString();
                }

                return "DefaultConnection";
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new MovieConfiguration());
            modelBuilder.Configurations.Add(new ActorConfiguration());
            modelBuilder.Configurations.Add(new ActorMovieConfiguration());
        }


        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException vex)
            {
                var exception = HandleDbEntityValidationException(vex);
                throw exception;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException vex)
            {
                var exception = HandleDbEntityValidationException(vex);
                throw exception;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
        }

        //=================================================================
        //=                                                               =
        //=      Display detail error while saving                        =
        //=                                                               =
        //=================================================================
        private Exception HandleDbEntityValidationException(DbEntityValidationException vex)
        {
            
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");
            foreach (var eve in vex.EntityValidationErrors)
            {
                builder.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                foreach (var ve in eve.ValidationErrors)
                {
                    builder.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                        ve.PropertyName,
                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                        ve.ErrorMessage));
                }
            }
            builder.AppendLine();

            string message = builder.ToString();
            return new Exception(message, vex);
        }

        private Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");
            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }
            string message = builder.ToString();
            return new Exception(message, dbu);
        }
    }
}
