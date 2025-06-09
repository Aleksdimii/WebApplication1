public class Passenger
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PassportNumber { get; set; }
    public DateTime DateOfBirth { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; }
}
