﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFDBFirstDemo.Data
{
    public class MovieRepository
    {
        private readonly MoviesDBContext _db;

        public MovieRepository(MoviesDBContext db)
        {
            _db = db ?? throw new ArgumentException(nameof(db));
        }

        public IEnumerable<Movie> GetMovies()
        {
            //we don't need to track changes to these
            //so skip the overhead of doing so
            List<Movie> movies = _db.Movie.AsNoTracking().ToList();
            return movies;
        }

        public IEnumerable<Movie> GetMoviesWithGenres()
        {
            List<Movie> movies = _db.Movie.Include(m => m.Genre).AsNoTracking().ToList();
            return movies;
        }

        public void AddMovie(string name, DateTime release, string genrename)
        {
            //LINQ: First fails by throwing exception,
            //FirtsOrDefault fails to just null
            var genre = _db.Genre.FirstOrDefault(g => g.Name == genrename);
            if (genre == null)
            {
                throw new ArgumentException("Genre not found: ", nameof(genrename));
            }
            var movie = new Movie
            {
                Name = name,
                ReleaseDate = release,
                Genre = genre
            };
            _db.Add(movie);
        }

        public void DeleteByID(int id)
        {
            //LINQ -> .FirstOrDefault(m => m.Id == ID)
            var movie = _db.Movie.Find(id);
            if (movie == null)
            {
                throw new ArgumentException("No such movie ID: ", nameof(id));
            }
            _db.Remove(movie);
        }

        public void Edit(Movie movie)
        {
            //would add the movie if it didn't exist
            _db.Update(movie);

            //sometimes we need to do it a different way
            //var trackedMovie = _db.Movie.Find(movie.Id);
            //_db.Entry(trackedMovie).CurrentValues.SetValues(movie);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
