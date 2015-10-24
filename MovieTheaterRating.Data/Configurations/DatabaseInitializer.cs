using MovieTheaterRating.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Data.Configurations
{
    public class DatabaseInitializer : 
        //DropCreateDatabaseIfModelChanges<MovieTheaterRatingContext>
    CreateDatabaseIfNotExists<MovieTheaterRatingContext>
    //DropCreateDatabaseIfModelChanges<MovieTheaterRatingContext>
    //CreateDatabaseIfNotExists<MovieTheaterRatingContext>
    //DropCreateDatabaseIfModelChanges<MovieTheaterRatingContext>
    //DropCreateDatabaseAlways<MovieTheaterRatingContext> //use this one for unit testing purpose
    {
        const string TOMATO_URL = @"YOUR_ROTTEN_TOMATOR_URL";

        private static string GetResult(string url)
        {
            string details = CallRestMethod(url);
            return details;
        }

        private static string CallRestMethod(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            Encoding enc = Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webResponse.Close();
            return result;
        }

        protected override void Seed(MovieTheaterRatingContext context)
        {
            string jsonString = GetResult(TOMATO_URL);
            JObject rss = JObject.Parse(jsonString);
            JArray movies = (JArray)rss["movies"];
            IList<JObject> moviesText = movies.Select(m => (JObject)m).ToList();
            foreach (dynamic m in moviesText)
            {
                int id = m.id;
                string title = m.title;
                string year = m.year;
                string mpaa_rating = m.mpaa_rating;
                DateTime releaseDate = m.release_dates.theater;
                int audienceRating = m.ratings.audience_score;
                string synopsis = m.synopsis;
                string posterImg = m.posters.original;
                string thumbnail = m.posters.thumbnail;
                string runtime = m.runtime;
                Movie movie = new Movie();
                movie.Id = id;
                movie.Title = title;
                movie.Year = year;
                movie.Runtime = Int32.Parse(runtime);
                movie.MPAARating = mpaa_rating;
                movie.ReleaseDate = releaseDate;
                movie.AudienceRating = audienceRating;
                movie.Synopsis = synopsis;
                movie.PosterImg = posterImg;
                movie.Thumbnail = thumbnail;
                context.Movies.Add(movie);

                List<Actor> actors = new List<Actor>();
                JArray casts = (JArray)m.abridged_cast;
                foreach (dynamic c in casts)
                {
                    int aid;
                    string name;
                    dynamic cast = null;
                    string character = string.Empty;
                    aid = c.id;
                    name = c.name;
                    Actor actor = new Actor();
                    actor.Id = aid;
                    var names = name.Split(' ');
                    if (names.Count() > 1)
                    {
                        actor.FirstName = names[0];
                        actor.LastName = names[1];
                    }
                    else
                    {
                        actor.FirstName = names[0];
                        actor.LastName = "NA";
                    }

                    context.Actors.Add(actor);

                    if (c.characters != null)
                    {
                        cast = c.characters[0];
                        character = (string)cast;
                    }
                    else
                    {
                        character = "Undefined";
                    }
                    ActorMovie am = new ActorMovie();
                    am.Movie = movie;
                    am.Actor = actor;
                    am.CharacterInMovie = character;
                    context.ActorMovies.Add(am);

                }
            }
            base.Seed(context);
        }
    }
}
