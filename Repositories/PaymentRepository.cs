using DAOs;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PaymentRepository : RepositoryBase<Payment>
    {
        BookingPartyContext context;
        public PaymentRepository()
        {
            context = new BookingPartyContext();
        }
        public List<Payment> GetAll()
        {
            return context.Payments.Include(p => p.Booking).ToList();
        }
    }
}
