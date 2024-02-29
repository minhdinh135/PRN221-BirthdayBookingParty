

using Models;

namespace Services.Interfaces
{
	public interface IPackageService
	{
		List<Package> GetAllPackages();

		void CreatePackage(Package package);

		Package UpdatePackage(int id, Package package);

		Package DeletePackage(int id);
	}
}
