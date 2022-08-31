using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.InputParam
{
    public class NearestLocationParam
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<int> PolyClinicIds { get; set; }
        public string Clue { get; set; }
        public int Radius { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
