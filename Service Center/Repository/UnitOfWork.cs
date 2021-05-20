using Service_Center.Contexts;
using Service_Center.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Service_Center.Repository
{
    class UnitOfWork : IDisposable
    {
        readonly Context context = new Context();
        private bool disposed = false;
        ServiceRepository serviceRepository;
        RapairRepository rapairRepository;
        UserRepository userRepository;
        ThemesRepository ThemesRepository;
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
        public ThemesRepository Themes
        {
            get
            {
                if (ThemesRepository == null)
                    ThemesRepository = new ThemesRepository(context);
                return ThemesRepository;
            }
        }
        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Сохранение не удалось!");
            }
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
