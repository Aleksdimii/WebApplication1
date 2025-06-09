public class Booking
{
    public int Id { get; set; }

    public int PassengerId { get; set; }
    public virtual Passenger Passenger { get; set; }

    public int FlightId { get; set; }
    public virtual Flight Flight { get; set; }

    public DateTime BookingDate { get; set; }
    public string SeatNumber { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }

}
