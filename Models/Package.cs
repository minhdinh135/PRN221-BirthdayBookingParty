namespace Models;

public partial class Package
{
    public int PackageId { get; set; }

    public string PackageName { get; set; }

    public string PackageType { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
