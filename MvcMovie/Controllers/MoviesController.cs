using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using Newtonsoft.Json.Linq;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(int? genreId, string searchString, string sortBy)
        {
            // Use LINQ to get list of genres.
            IQueryable<Genre> genreQuery = from g in _context.Genre
                                            orderby g.GenreName
                                            select g;
            IQueryable<Movie> movies = _context.Movie.Include("Genre");

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(movie => movie.Title!.Contains(searchString));
            }

            if (genreId is not null)
            {
                movies = movies.Where(movie => movie.Genre.GenreId == genreId);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                movies = sortBy switch
                {
                    "title" => movies.OrderBy(movie => movie.Title),
                    "releasedate" => movies.OrderBy(movie => movie.ReleaseDate),
                    "genre" => movies.OrderBy(movie => movie.Genre.GenreName),
                    "price" => movies.OrderBy(movie => movie.Price),
                    "rating" => movies.OrderBy(movie => movie.Rating),
                    _ => movies.OrderBy(movie => movie.Title),
                };
            }

            MovieGenreViewModel movieGenreVM = new()
            {
                Genres = await genreQuery.ToListAsync(),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include("Genre")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            List<Genre> genres = _context.Genre.ToList();
            MovieEditViewModel editModel = new()
            {
                Genres = genres
            };

            return View(editModel);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,GenreId,Price,Rating,ImageUrl")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            Movie movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            List<Genre> genres = await _context.Genre.ToListAsync();
            MovieEditViewModel editModel = new() 
            { 
                Movie = movie,
                Genres = genres
            };

            return View(editModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,GenreId,Price,ImageUrl")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            Movie movie = await _context.Movie
                .Include("Genre")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return _context.Movie.Any(e => e.Id == id);
        }
    }
}
