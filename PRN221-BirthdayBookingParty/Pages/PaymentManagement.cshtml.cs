using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    public class PaymentManagementModel : PageModel
    {
        public decimal DepositMoney { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; }
        public List<Service> Services { get; set; }
        private PaymentRepository paymentRepository;
        private ServiceRepository serviceRepository;


        public PaymentManagementModel()
        {
            paymentRepository = new PaymentRepository();
            serviceRepository = new ServiceRepository();
        }

        public void OnGet()
        {
            Services = serviceRepository.GetAll();
            PaymentStatus = "Not yet";

        }
    }
}
