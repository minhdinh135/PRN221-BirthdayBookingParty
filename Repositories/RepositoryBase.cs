﻿using DAOs;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        BookingPartyContext context;
        DbSet<T> set;

        public RepositoryBase()
        {
            context = new BookingPartyContext();
            set = context.Set<T>();
        }

        public List<T> GetAll()
        {
            return set.ToList();
        }

        public void Add(T item)
        {
            set.Add(item);
            context.SaveChanges();
        }

        public void Update(T item)
        {
            set.Update(item);
            context.SaveChanges();
        }

        public void Delete(T item)
        {
            set.Remove(item);
            context.SaveChanges();
        }
    }
}
