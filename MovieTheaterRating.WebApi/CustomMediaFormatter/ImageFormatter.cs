using MovieTheaterRating.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace MovieTheaterRating.WebApi.CustomMediaFormatter
{
    public class ImageFormatter : MediaTypeFormatter
    {
        public ImageFormatter()
        {
            SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("image/png")); //the value "image/png" must match the Accept type in Fiddler
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Movie); //this should be a generic Entity
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() => WriteToStream(type, value, writeStream, content));
        }

        public void WriteToStream(Type type, object value, Stream stream, HttpContent content)
        {
            Movie movie = (Movie)value;
            Image image = Image.FromFile(@".\Photos\" + movie.Title + ".png");
            image.Save(stream, ImageFormat.Png);
            image.Dispose();
        }
    }
}