﻿using System;
using System.Collections.Generic;

namespace Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int Capacity { get; set; }

    public string RoomStatus { get; set; }

    public decimal RoomPrice { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
