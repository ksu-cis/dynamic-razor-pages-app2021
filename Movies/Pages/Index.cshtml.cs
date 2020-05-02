using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {

        /// <summary>
        /// The movies to display on the index page 
        /// </summary>
        public IEnumerable<Movie> Movies { get; protected set; }

        /// <summary>
        /// The current search terms 
        /// </summary>
        [BindProperty]
        public string SearchTerms { get; set; } 

        /// <summary>
        /// The filtered MPAA Ratings
        /// </summary>
        [BindProperty]
        public string[] MPAARatings { get; set; } 

        /// <summary>
        /// The filtered genres
        /// </summary>
        [BindProperty]
        public string[] Genres { get; set; }

        /// <summary>
        /// The minimum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? IMDBMin { get; set; }

        /// <summary>
        /// The maximum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? IMDBMax { get; set; }


        public double? RottenMin { get; set; }
        public double? RottenMax { get; set; }


        /// <summary>
        /// Gets the search results for display on the page
        /// </summary>
    public void OnGet(double? IMDBMin, double? IMDBMax, double? RottenMin, double? RottenMax)
        {
            
            this.IMDBMin = IMDBMin;
            this.IMDBMax = IMDBMax;
            this.RottenMin = RottenMin;
            this.RottenMax = RottenMax;

            SearchTerms = Request.Query["SearchTerms"];
            MPAARatings = Request.Query["MPAARatings"];
            Genres = Request.Query["Genres"];

            /*
            Movies = MovieDatabase.Search(SearchTerms);
            Movies = MovieDatabase.FilterByMPAARating(Movies, MPAARatings);
            Movies = MovieDatabase.FilterByGenre(Movies, Genres);
            Movies = MovieDatabase.FilterByIMDBRating(Movies, IMDBMin, IMDBMax);
            Movies = MovieDatabase.FilterByRottenRating(Movies, RottenMin, RottenMax);
            */

            Movies = MovieDatabase.All;

            if(SearchTerms != null)
            {
                  Movies = Movies.Where(movie => 
                  movie.Title != null && 
                  movie.Title.Contains(SearchTerms, StringComparison.CurrentCultureIgnoreCase)
                  );
            //    Movies = from movie in Movies
            //             where movie.Title != null && movie.Title.Contains(SearchTerms, StringComparison.CurrentCultureIgnoreCase)
            //             select movie;
            }

            if(MPAARatings != null && MPAARatings.Length != 0)
            {
                Movies = Movies.Where(movie => 
                movie.MPAARating != null && 
                MPAARatings.Contains(movie.MPAARating)
                );
            }

            if(Genres != null && Genres.Count() != 0)
            {
                Movies = Movies.Where(movie =>
                movie.MajorGenre != null &&
                Genres.Contains(movie.MajorGenre));
            }
            
            if(IMDBMin != null || IMDBMax != null)
            {

                if(IMDBMin == null)
                {
                    Movies = Movies.Where(movie =>
                    movie.IMDBRating <= IMDBMax
                    );
                }

                else if(IMDBMax == null)
                {
                    Movies = Movies.Where(movie =>
                    movie.IMDBRating >= IMDBMin
                    );
                }

                else
                {
                    Movies = Movies.Where(movie =>
                    movie.IMDBRating >= IMDBMin && movie.IMDBRating <= IMDBMax
                    );
                }
                
            }

            if(RottenMin != null || RottenMax != null)
            {
                if(RottenMin == null)
                {
                    Movies = Movies.Where(movie =>
                    movie.RottenTomatoesRating <= RottenMax
                    );
                }
                else if(RottenMax == null)
                {
                    Movies = Movies.Where(movie =>
                    movie.RottenTomatoesRating >= RottenMin
                    );
                }
                else
                {
                    Movies = Movies.Where(movie =>
                    movie.RottenTomatoesRating >= RottenMin &&
                    movie.RottenTomatoesRating <= RottenMax
                    );
                }
            }


        }






    }
}
