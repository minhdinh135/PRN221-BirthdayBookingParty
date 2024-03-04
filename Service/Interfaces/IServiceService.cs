using Models;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        List<Service> GetAllService();

        void CreateService(Service service);

        Service UpdateService(int id, Service service);

        Service DeleteService(int id);
    }
}
