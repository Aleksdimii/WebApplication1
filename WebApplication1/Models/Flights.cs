public class Flight
{
    public int Id { get; set; }
    public string FlightNumber { get; set; }
    public string Airline {  get; set; }
    public IEnumerable<Flight>? Departures { get; set; }
    public IEnumerable<Flight>? Arrivals { get; set; }
    public int DepartureAirportId { get; set; }
    public virtual Airport? DepartureAirport { get; set; }

    public int ArrivalAirportId { get; set; }
    public virtual Airport? ArrivalAirport { get; set; }

    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }

    public string Status { get; set; }

    public int AircraftId { get; set; }
    public virtual Aircraft? Aircraft { get; set; }

    public virtual ICollection<Booking>? Bookings { get; set; }
    public virtual ICollection<CrewMember>? CrewMembers { get; set; }
}
