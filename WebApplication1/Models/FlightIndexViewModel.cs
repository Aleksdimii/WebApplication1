using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class FlightIndexViewModel
    {
        public IEnumerable<Flight> Departures { get; set; }
        public IEnumerable<Flight> Arrivals { get; set; }
    }
}