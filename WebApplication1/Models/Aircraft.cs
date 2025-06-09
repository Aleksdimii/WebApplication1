public class Aircraft
{
    public int Id { get; set; }
    public string Model { get; set; }
    public string RegistrationNumber { get; set; }
    public int Capacity { get; set; }
    public string Manufacturer { get; set; }
    public FlightType? Type { get; set; }


    public virtual ICollection<Flight>? Flights { get; set; }
}
