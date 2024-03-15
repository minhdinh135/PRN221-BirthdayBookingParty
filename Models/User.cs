using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Models;

public partial class User
{
    public int UserId {  get; set; }
	[StringLength(40, MinimumLength = 2, ErrorMessage = "FullName must be between 2 and 40 characters")]
	[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "CustomerFullName can only contain alphabetic characters and spaces")]
	
	public string FullName { get; set; }
	[RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telephone must be a valid phone number")]
	
	public string Phone {  get; set; }
	[StringLength(50, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 50 characters")]
	
	public string Address { get; set; }
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
	
	public DateTime Birthday { get; set; }
    
	public byte? Status { get; set; }
	[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email format")]
	
	public string Email { get; set; }
	[StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
	
	public string Password { get; set; }
    
	public int RoleId { get; set; }
    
	public Role Role { get; set; }
    
	public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}
