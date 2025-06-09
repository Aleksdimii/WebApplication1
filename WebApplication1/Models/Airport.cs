public class Airport
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public virtual ICollection<Flight> Departures { get; set; } = new List<Flight>();
    public virtual ICollection<Flight> Arrivals { get; set; } = new List<Flight>();
}
