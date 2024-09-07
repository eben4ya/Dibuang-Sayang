using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appview.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Status { get; set; } // Values: Confirmed, Pending, Canceled

        public void ConfirmReservation()
        {
            this.Status = "Confirmed";
            Console.WriteLine("Reservation confirmed");
        }

        public void CancelReservation()
        {
            this.Status = "Canceled";
            Console.WriteLine("Reservation canceled");
        }
    }

}
