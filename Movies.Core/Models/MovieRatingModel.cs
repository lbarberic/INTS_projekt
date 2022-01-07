﻿using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core.Models
{
    public class MovieRatingModel
    {
        [LoadColumn(0)] public float userId;
        [LoadColumn(1)] public float movieId;
        [LoadColumn(2)] public float Label;
    }
}
