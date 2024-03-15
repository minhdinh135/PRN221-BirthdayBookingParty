using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;

namespace PRN221_BirthdayBookingParty.Pages
{
	[Authorize("LoginSessionPolicy")]
    public class ServiceListModel : PageModel
    {
		private IRepositoryBase<Service> serviceRepository;
		private IRepositoryBase<Package> packageRepository;

		public List<Service> Services { get; set; }
		public List<Package> Packages { get; set; }

		public Dictionary<int, string> PackageTypes { get; private set; }

		public void OnGet()
		{
			packageRepository = new PackageRepository();
			serviceRepository = new ServiceRepository();
			Services = serviceRepository.GetAll();

			PackageTypes = new Dictionary<int, string>();
			foreach (var service in Services)
			{
				int packageId = GetPackageId(service.ServiceId);
				string packageType = GetPackageType(packageId);
				PackageTypes.Add(service.ServiceId, packageType);
			}
		}

		public int GetPackageId(int serviceId)
		{
			var service = Services.FirstOrDefault(s => s.ServiceId == serviceId);
			if (service != null)
			{
				return service.PackageId;
			}
			return -1;
		}

		public string GetPackageType(int packageId)
		{
			var package = packageRepository.GetAll().FirstOrDefault(p => p.PackageId == packageId);
			if (package != null)
			{
				return package.PackageType;
			}
			return null;
		}
	}
}
