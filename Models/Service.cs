namespace Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; }

    public string ServiceStatus { get; set; }

    public decimal Price { get; set; }

    public int PackageId { get; set; }

    public virtual Package Package { get; set; }

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}
