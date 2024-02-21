using System;
using System.Collections.Generic;

namespace Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int RoomId { get; set; }

    public DateTime BookingDate { get; set; }

    public string BookingStatus { get; set; }

    public string Feedback { get; set; }

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Package Package { get; set; }

    public virtual User User { get; set; }

    public virtual Room Room { get; set; }
}
