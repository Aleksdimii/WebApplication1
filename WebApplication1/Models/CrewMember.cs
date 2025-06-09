public class CrewMember
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }

    public int FlightId { get; set; }
    public virtual Flight Flight { get; set; }
}
