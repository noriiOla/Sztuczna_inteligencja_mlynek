﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlynekV2.Models
{
    public class Punkt
    {
        public double x { get; set; }
        public double y { get; set; }

        public Punkt(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}