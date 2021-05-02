using Service_Center.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Models
{
    class Rapair
    {
        static int count = 0;
        public Rapair()
        {
            count++;
            RapairID = count;
        }
        [Key]
        public int RapairID { get; set; }
        public int UserID { get; set; }
        public string Device { get; set; }
        public string SerialNumber { get; set; }
        public string Malfunction { get; set; }
        public string Status { get; set; }
        public decimal SumMoney { get; set; }
        public DateTime DateOfRaceipt { get; set; }
    }
}
