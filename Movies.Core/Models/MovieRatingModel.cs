using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core.Models
{
    public class MovieRatingModel
    {
        [LoadColumn(0)] public string userId;
        [LoadColumn(1)] public string movieId;
        [LoadColumn(2)] public bool Label;
    }
}
