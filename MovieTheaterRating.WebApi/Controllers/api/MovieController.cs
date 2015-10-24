using MovieTheaterRating.Data;
using MovieTheaterRating.Data.DTOs;
using MovieTheaterRating.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace MovieTheaterRating.WebApi.Controllers.api
{
    [EnableCorsAttribute("http://localhost:54921", "*", "*")]
    [RoutePrefix("api/Movie")]
    public class MovieController : ApiController
    {
        MovieTheaterRatingContext context ;
        public MovieController()
        {
            context = new MovieTheaterRatingContext();
        }

        //GET: api/Movie
        [HttpGet]
        [ResponseType(typeof(MovieDTO))]
        public IHttpActionResult Get()
        {
            try
            {
                var movies = context.Movies.Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Runtime = m.Runtime,
                    Year = m.Year,
                    AudienceRating = m.AudienceRating,
                    MPAARating = m.MPAARating,
                    PosterImg = m.PosterImg,
                    Thumbnail = m.Thumbnail,
                    Synopsis = m.Synopsis,
                    MovieCharacters = m.ActorMovies.Select(am => new MovieCharacter
                    {
                        ActorID = am.ActorID,
                        CharacterInMovie = am.CharacterInMovie,
                        FirstName = am.Actor.FirstName,
                        LastName = am.Actor.LastName
                    }).ToList()
                }).ToList();
                return Ok(movies);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //GET: api/Movie/{movieId}
        [HttpGet]
        [ResponseType(typeof(MovieDTO))]
        //Parameter must be Id if we want a route like this: api/Movie/1
        //if parameter is not Id but something else like movieId, route will be: api/Movie/movieID=value
        //if we want to keep route like api/Movie/1 but use custom name for parameter use Attribute routing
        [Route("{movieId}")]
        public IHttpActionResult Get(int movieId) 
        {
            try
            {
                var movie = context.Movies.Where(m => m.Id == movieId).Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Year = m.Year,
                    Runtime = m.Runtime,
                    AudienceRating = m.AudienceRating,
                    MPAARating = m.MPAARating,
                    PosterImg = m.PosterImg,
                    Thumbnail = m.Thumbnail,
                    Synopsis = m.Synopsis,
                    MovieCharacters = m.ActorMovies.Select(am => new MovieCharacter
                    {
                        ActorID = am.ActorID,
                        CharacterInMovie = am.CharacterInMovie,
                        FirstName = am.Actor.FirstName,
                        LastName = am.Actor.LastName
                    }).ToList()
                }).FirstOrDefault();

                if (movie == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(movie);
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //POST: api/Movie
        [HttpPost]
        [ResponseType(typeof(MovieDTO))]
        public HttpResponseMessage Post([FromBody] MovieDTO movieDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int currMaxId = context.Movies.Max(m => m.Id) + 1;

                    movieDTO.Id = currMaxId;
                    var newMovie = new Movie
                    {
                        Id = movieDTO.Id,
                        Title = movieDTO.Title,
                        ReleaseDate = movieDTO.ReleaseDate,
                        Year = movieDTO.Year,
                        Runtime = movieDTO.Runtime,
                        AudienceRating = movieDTO.AudienceRating,
                        MPAARating = movieDTO.MPAARating,
                        PosterImg = movieDTO.PosterImg,
                        Thumbnail = movieDTO.Thumbnail,
                        Synopsis = movieDTO.Synopsis
                    };
                    context.Movies.Add(newMovie);
                    context.SaveChanges();

                    var movieChars = movieDTO.MovieCharacters;
                    if(movieChars != null)
                    {
                        foreach(var mc in movieChars)
                        {
                            Actor newActor;
                            ActorMovie newActorMovie;
                            if ( (context.Actors.FirstOrDefault(a => a.FirstName == mc.FirstName) == null || 
                                context.Actors.FirstOrDefault(a => a.LastName != mc.LastName) == null)) //non existing Actor, add new actor, then set movie the actor plays role
                            {
                                newActor = new Actor
                                {
                                    FirstName = mc.FirstName,
                                    LastName = mc.LastName                                                                                                       
                                };
                                context.Actors.Add(newActor);
                                context.SaveChanges();
                                newActorMovie = new ActorMovie
                                {
                                    CharacterInMovie = mc.CharacterInMovie,
                                    ActorID = newActor.Id,
                                    MovieID = newMovie.Id
                                };
                                context.ActorMovies.Add(newActorMovie);
                                context.SaveChanges();
                            }
                            else //an existing actor, just set roles the actor plays in new movie
                            {
                                var f = context.Actors.FirstOrDefault(a => a.FirstName == mc.FirstName);
                                var l = context.Actors.FirstOrDefault(a => a.LastName != mc.LastName);
                                newActor = context.Actors.FirstOrDefault(a => a.FirstName == mc.FirstName && a.LastName == mc.LastName);
                                newActorMovie = new ActorMovie
                                {
                                    CharacterInMovie = mc.CharacterInMovie,
                                    ActorID = newActor.Id,
                                    MovieID = newMovie.Id
                                };
                                context.ActorMovies.Add(newActorMovie);
                                context.SaveChanges();
                            }
                        }
                    }
                    HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.Created, newMovie);
                    result.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = newMovie.Id }));
                    return result;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //PUT: api/Movie
        [HttpPut]
        public HttpResponseMessage Put(MovieDTO movieDTO)
        {
            if (ModelState.IsValid)
            {
                var existingParent = context.Movies.Where(m => m.Id == movieDTO.Id).Include(m => m.ActorMovies).FirstOrDefault();
                if (existingParent != null)
                {
                    var movie = new Movie
                    {
                        Id = movieDTO.Id,
                        Title = movieDTO.Title,
                        Year = movieDTO.Year,
                        Runtime = movieDTO.Runtime,
                        ReleaseDate = movieDTO.ReleaseDate,
                        AudienceRating = movieDTO.AudienceRating,
                        MPAARating = movieDTO.MPAARating,
                        PosterImg = movieDTO.PosterImg,
                        Thumbnail = movieDTO.Thumbnail,
                        Synopsis = movieDTO.Synopsis
                    };
                    foreach(var mc in movieDTO.MovieCharacters)
                    {
                        int actorID = context.Actors.Where(a => a.FirstName == mc.FirstName && a.LastName == mc.LastName).FirstOrDefault().Id; //reuse ActorID
                        var am = new ActorMovie
                        {
                            ActorID = actorID,
                            MovieID = movie.Id,
                            CharacterInMovie = mc.CharacterInMovie
                        };
                        movie.ActorMovies.Add(am);
                    }
                    context.Entry(existingParent).State = EntityState.Modified;
                    context.Entry(existingParent).CurrentValues.SetValues(movie); //existing movie with new ActorMovie collection

                    foreach (var existingChild in existingParent.ActorMovies.ToList())
                    {
                        if (!movie.ActorMovies.Any(am => am.ActorID == existingChild.ActorID && am.MovieID == existingChild.MovieID))
                        {
                            Debug.WriteLine("deleting an item: actorid = " + existingChild.ActorID + " | movieid = " + existingChild.MovieID + " | chacater: " + existingChild.CharacterInMovie);
                            context.ActorMovies.Remove(existingChild);
                        }
                    }
                    foreach (var child in movie.ActorMovies)
                    {
                        var existingChild = existingParent.ActorMovies.Where(am => am.ActorID == child.ActorID && am.MovieID == child.MovieID)
                                                                    .FirstOrDefault();
                        if (existingChild != null)
                        {
                            Debug.WriteLine("updating an exsting item: " + child.ActorID + " | movieid = " + existingChild.MovieID + " | chacater: " + existingChild.CharacterInMovie);
                            context.Entry(existingChild).State = EntityState.Modified;
                            existingChild.MovieID = movie.Id;
                            existingChild.CharacterInMovie = child.CharacterInMovie;
                        }
                        else
                        {
                            //get ActorID
                            int newActorID = movie.ActorMovies.Where(am => am.MovieID == child.MovieID && am.CharacterInMovie == child.CharacterInMovie).FirstOrDefault().ActorID;
                            child.ActorID = newActorID;
                            var newChild = child;
                            existingParent.ActorMovies.Add(newChild);
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent,movieDTO);
                }
                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "{success: 'true', verb:'PUT'}");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, ex);
                }
                catch(Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
