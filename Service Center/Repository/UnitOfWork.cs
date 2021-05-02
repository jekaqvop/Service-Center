using Service_Center.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Repository
{
    class UnitOfWork : IDisposable
    {
        readonly Context context = new Context();
        private bool disposed = false;
        ServiceRepository serviceRepository;
        RapairRepository rapairRepository;
        UserRepository userRepository;
        public ServiceRepository Services
        {
            get
            {
                if (serviceRepository == null)
                    serviceRepository = new ServiceRepository(context);
                return serviceRepository;
            }
        }
        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(context);
                return userRepository;
            }
        }
        public RapairRepository Repairs
        {
            get
            {
                if (rapairRepository == null)
                    rapairRepository = new RapairRepository(context);
                return rapairRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
