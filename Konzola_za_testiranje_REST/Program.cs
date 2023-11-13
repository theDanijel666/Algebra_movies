using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace Konzola_za_testiranje_REST
{
    class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7223/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("api/Movies").Result;
            if(response.IsSuccessStatusCode)
            {
                var data=response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;
                foreach(var m in data)
                {
                    Console.WriteLine(m.Title);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            var put_result=client.PostAsync("api/Movies", 
                    new Movie { Id = "5", Title = "The Godfather", Genre = "Crime", Year = "1972" }, 
                    new JsonMediaTypeFormatter()).Result;
            if(put_result.StatusCode==System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("Movie added");
            }
            else
            {
                Console.WriteLine("Movie not added");
            }
            var delete_result=client.DeleteAsync("api/Movies/6").Result;
            switch (delete_result.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    Console.WriteLine("Movie deleted");
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    Console.WriteLine("Movie not found");
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    Console.WriteLine("Internal server error");
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    Console.WriteLine("Bad request");
                    break;
            }

            if ((int)delete_result.StatusCode == 417)
            {

            }

            client.PutAsync("api/Movies/1", 
                            new Movie { Id = "1", Title = "The Godfather", Genre = "Crime", Year = "1972" }, 
                            new JsonMediaTypeFormatter()).Wait();
        }
    }
}
