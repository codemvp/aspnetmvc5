using System;
namespace MatchByteMovieApp.Models
{
    public partial class Movie
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual DateTime YearReleased { get; set; }
        public virtual EnumRatings Rating { get; set; }
    }
}