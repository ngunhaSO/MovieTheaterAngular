using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using MovieTheaterRating.Data;
using MovieTheaterRating.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MovieTheaterRating.Test.MovieTheaterRating.Data
{
    [TestClass]
    public class Movie_ActorMovieCRUD
    {
        private DbConnection connection;
        private MovieTheaterRatingContext _context;
        private Movie movie;
        private ActorMovie actorMovie1;
        private ActorMovie actorMovie2;
        private ActorMovie actorMovie3;
        private Actor actor1;
        private Actor actor2;


        private ActorMovie updateActorMovie1;
        private ActorMovie insertNewActorMovie4;
        private Movie updateMovie1;

        [TestInitialize]
        public void Setup()
        {
            connection = Effort.DbConnectionFactory.CreateTransient();
            using (var context = new MovieTheaterRatingContext(connection))
            {
                actorMovie1 = new ActorMovie
                {
                    ID = 1,
                    ActorID = 1,
                    CharacterInMovie = "Actor 1: First character",
                    MovieID = 1
                };

                actorMovie2 = new ActorMovie
                {
                    ID = 2,
                    ActorID = 1,
                    CharacterInMovie = "Actor 1: Second character",
                    MovieID = 1
                };

                actorMovie3 = new ActorMovie
                {
                    ID = 3,
                    ActorID = 2,
                    CharacterInMovie = "Actor 2: First character",
                    MovieID = 1
                };


                context.ActorMovies.Add(actorMovie1);
                context.ActorMovies.Add(actorMovie2);

                List<ActorMovie> moviePlayedByActor1 = new List<ActorMovie>();
                moviePlayedByActor1.Add(actorMovie1);
                moviePlayedByActor1.Add(actorMovie2);

                List<ActorMovie> moviePlayedByActor2 = new List<ActorMovie>();
                moviePlayedByActor2.Add(actorMovie3);

                actor1 = new Actor
                {
                    Id = 1,
                    FirstName = "First1",
                    LastName = "Last1",
                    ActorMovies = moviePlayedByActor1
                };

                actor2 = new Actor
                {
                    Id = 2,
                    FirstName ="First2",
                    LastName = "Last2",
                    ActorMovies = moviePlayedByActor2
                };

                context.Actors.Add(actor1);
                context.Actors.Add(actor2);

                List<ActorMovie> cast = new List<ActorMovie>();
                cast.Add(actorMovie1);
                cast.Add(actorMovie2);
                cast.Add(actorMovie3);
                movie = new Movie
                {
                    Id = 1,
                    Title = "Movie 1",
                    ActorMovies = cast
                };
                context.Movies.Add(movie);
                context.SaveChanges();
            }

            _context = new MovieTheaterRatingContext(connection);
            _context.Database.Initialize(true);
        }

        [TestMethod]
        public void TestGetAMovie()
        {
            using (_context)
            {
                var movie = _context.Movies.Where(m => m.Id == 1).FirstOrDefault();
                Assert.IsNotNull(movie);
                Assert.AreEqual("Movie 1", movie.Title);
                Assert.AreEqual(3, movie.ActorMovies.Count);
            }
        }

        [TestMethod]
        public void TestGetAnActor()
        {
            using (_context)
            {
                var actor = _context.Actors.Where(a => a.Id == 1).FirstOrDefault();
                Assert.AreEqual("First1", actor.FirstName);
            }
        }

        [TestMethod]
        public void TestCRUDParentChild()
        {
            setupDataForParentChildCRUD();
            performCRUDParentChild(updateMovie1);

            var mov = _context.Movies.Where(m => m.Id == 1).FirstOrDefault();

            Assert.AreEqual("Update movie 1", mov.Title);
            Assert.AreEqual(2, mov.ActorMovies.Count);
        }

        private void setupDataForParentChildCRUD()
        {
            updateActorMovie1 = new ActorMovie
            {
                ID = 1,
                CharacterInMovie = "update actor movie1",
                ActorID = 1,
                MovieID = 1
            };

            insertNewActorMovie4 = new ActorMovie
            {
                ID = 4,
                CharacterInMovie = "insert actor movie 4 for actor 1",
                ActorID = 1,
                MovieID = 1
            };

            List<ActorMovie> cast = new List<ActorMovie>();
            cast.Add(updateActorMovie1);
            cast.Add(insertNewActorMovie4);
            updateMovie1 = new Movie
            {
                Id = 1,
                Title = "Update movie 1",
                ActorMovies = cast
            };
        }

        private void performCRUDParentChild(Movie movie)
        {
            var existingParent = _context.Movies.Where(m => m.Id == 1)
                                            .Include(m => m.ActorMovies).FirstOrDefault();

            if (existingParent != null)
            {
                _context.Entry(existingParent).State = EntityState.Modified;
                _context.Entry(existingParent).CurrentValues.SetValues(movie);

                foreach (var existingChild in existingParent.ActorMovies.ToList())
                {
                    if(!movie.ActorMovies.Any(am => am.ID == existingChild.ID))
                    {
                        _context.ActorMovies.Remove(existingChild);
                    }
                }

                foreach(var child in movie.ActorMovies)
                {
                    var existingChild = existingParent.ActorMovies.Where(am => am.ID == child.ID).FirstOrDefault();
                    if (existingChild != null)
                    {
                        _context.Entry(existingChild).CurrentValues.SetValues(child);
                    }
                    else
                    {
                        existingParent.ActorMovies.Add(child);
                    }
                }
            }
            _context.SaveChanges();
        }
    }
}
