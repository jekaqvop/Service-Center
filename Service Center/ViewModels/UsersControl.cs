using Service_Center.Contexts;
using Service_Center.Models;
using Service_Center.Resources;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.ViewModels
{
    class UsersControlVM : PropertysChanged
    {
        public UsersControlVM() 
        {
            
        }
        

        public User User { get; set; }
        
    }
}
