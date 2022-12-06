using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System;
using System.Linq;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcMovieContext>>()))
            {
                if (!context.Genre.Any())
                {
                    context.Genre.AddRange(
                        new Genre
                        {
                            GenreName = "Drama"
                        },

                        new Genre
                        {
                            GenreName = "Action"
                        },

                        new Genre
                        {
                            GenreName = "Adventure"
                        },

                        new Genre
                        {
                            GenreName = "Comedy"
                        },

                        new Genre
                        {
                            GenreName = "Romance"
                        }
                    );
                    context.SaveChanges();
                }

                // Look for any movies.
                if (!context.Movie.Any())
                {
                    List<Genre> genres = context.Genre.ToList();

                    context.Movie.AddRange(
                        new Movie
                        {
                            Title = "17 Miracles",
                            ReleaseDate = DateTime.Parse("2011-6-3"),
                            GenreId = genres.Where(genre => genre.GenreName == "Adventure").First().GenreId,
                            Price = 11.69M,
                            Rating = "PG",
                            ImageUrl = "https://m.media-amazon.com/images/M/MV5BNTkzZWRmMWQtZDc2YS00MzIzLTljMTUtYTVkM2I5Yzg4OTMzL2ltYWdlXkEyXkFqcGdeQXVyNDcxMTk4Mzc@._V1_.jpg"
                        },

                        new Movie
                        {
                            Title = "Ephraim's Rescue",
                            ReleaseDate = DateTime.Parse("2013-5-31"),
                            GenreId = genres.Where(genre => genre.GenreName == "Drama").First().GenreId,
                            Price = 12.36M,
                            Rating = "PG",
                            ImageUrl = "https://m.media-amazon.com/images/I/81Cx7qsKdeL._RI_.jpg"
                        },

                        new Movie
                        {
                            Title = "The Best Two Years",
                            ReleaseDate = DateTime.Parse("2003-10-10"),
                            GenreId = genres.Where(genre => genre.GenreName == "Comedy").First().GenreId,
                            Price = 9.99M,
                            Rating = "PG",
                            ImageUrl = "https://m.media-amazon.com/images/M/MV5BMjEzMzc5MjQ4NV5BMl5BanBnXkFtZTYwNjY1MjE3._V1_.jpg"
                        },

                        new Movie
                        {
                            Title = "The Other Side of Heaven",
                            ReleaseDate = DateTime.Parse("2001-12-14"),
                            GenreId = genres.Where(genre => genre.GenreName == "Adventure").First().GenreId,
                            Price = 9.99M,
                            Rating = "PG",
                            ImageUrl = "https://m.media-amazon.com/images/M/MV5BNjA0NTY1NzM2MV5BMl5BanBnXkFtZTgwMzgyMzYzNTE@._V1_.jpg"
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}