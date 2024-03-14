namespace Models;

public partial class Booking
{
    public int BookingId { get; set; }
    public DateTime BookingDate { get; set; }

    public DateTime PartyDateTime { get; set; }

    public string BookingStatus { get; set; }
    public string Feedback { get; set; }

    public Payment Payment { get; set; } 

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();

    public int UserId { get; set; }
    public virtual User User { get; set; }

    public int RoomId { get; set; }
    public virtual Room Room { get; set; }
}
