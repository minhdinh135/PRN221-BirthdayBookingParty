using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public List<Service> GetAllService()
        {
            return serviceRepository.GetAll().ToList();
        }

        public void CreateService(Service service)
        {
            serviceRepository.Add(service);
        }

        public Service UpdateService(int id, Service service)
        {
            Service existingservice = serviceRepository.GetAll().FirstOrDefault(p => p.ServiceId == id);

            existingservice.ServiceName = service.ServiceName;
            existingservice.ServiceStatus = service.ServiceStatus;

            serviceRepository.Update(existingservice);

            return existingservice;
        }

        public Service DeleteService(int id)
        {
            Service existingservice = serviceRepository.GetAll().FirstOrDefault(p => p.ServiceId == id);
            
            serviceRepository.Delete(existingservice);

            return existingservice;
        }
    }
}
