using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Service_Center.Models
{
    class Service
    {      
        [Key]
        public int ServiceId { get; set; }
        public string ImageSourse { get; set; }  
        public string Title { get; set; }       
        public string Info { get; set; }        
        public decimal Price { get; set; }        
    }
}
