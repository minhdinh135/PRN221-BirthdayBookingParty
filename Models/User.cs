using System;
using System.Collections.Generic;

namespace Models;

public partial class User
{
    //public string? ProfilePicture { get; set; }

    public int UserId {  get; set; }
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}
