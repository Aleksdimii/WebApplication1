public class FlightIndexViewModel
{
    public List<Flight> Departures { get; set; }
    public List<Flight> Arrivals { get; set; }
    public List<Airport> Airports { get; set; }
    public int? SelectedAirportId { get; set; }
}
