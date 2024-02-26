using System;
using System.Collections.Generic;

namespace Models;

public partial class User
{
    //public string? ProfilePicture { get; set; }

    public int UserId {  get; set; }
    public string FullName { get; set; } 
    public string Phone {  get; set; }
    public string Address { get; set; }
    public DateTime Birthday { get; set; }
    public byte? Status { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }

    public Role Role { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}
