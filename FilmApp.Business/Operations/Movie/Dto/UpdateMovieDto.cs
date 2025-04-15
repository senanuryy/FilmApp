﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Data.Enums;

namespace FilmApp.Business.Operations.Movie.Dto
{
    public class UpdateMovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? IMDB { get; set; }       
        public string Language { get; set; }        
        public FormatType FormatType { get; set; }
        public List<int> GenreIds { get; set; }
    }
}
