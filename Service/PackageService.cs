using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
	public class PackageService : IPackageService
	{
		private readonly IPackageRepository packageRepository;

		public PackageService(IPackageRepository packageRepository)
		{
			this.packageRepository = packageRepository;
		}

		public List<Package> GetAllPackages()
		{
			return packageRepository.GetAll().ToList();
		}

		public void CreatePackage(Package package)
		{
			packageRepository.Add(package);
		}

		public Package UpdatePackage(int id, Package package)
		{
			Package existingPackage = packageRepository.GetAll().FirstOrDefault(p => p.PackageId == id);

			existingPackage.PackageName = package.PackageName;
			existingPackage.PackageType = package.PackageType;

			packageRepository.Update(existingPackage);

			return existingPackage;
		}

		public Package DeletePackage(int id)
		{
			Package existingPackage = packageRepository.GetAll().FirstOrDefault(p => p.PackageId == id);
			packageRepository.Delete(existingPackage);

			return existingPackage;
		}
	}
}
