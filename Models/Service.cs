namespace Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; }

    public int PackageId { get; set; }
    public virtual Package Package { get; set; }
}
