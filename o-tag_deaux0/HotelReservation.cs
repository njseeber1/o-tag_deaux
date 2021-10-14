using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o_tag_deaux0
{
    public class HotelReservation
    {
        public string confirmationid { get; set; }
        public string result { get; set; }
        public string msg { get; set; }
        public string status { get; set; }
        public string indate { get; set; }
        public string outdate { get; set; }
        public string roomnum { get; set; }
        public int pax { get; set; }
        public string[] names { get; set; }
        public string primaryname { get; set; }

        public HotelReservation(Reservation reservation)
        {
            this.confirmationid = reservation.confirmationid;
            this.result = reservation.result;
            this.msg = reservation.msg;
            this.status = reservation.status;
            this.indate = reservation.indate;
            this.outdate = reservation.outdate;
            this.roomnum = reservation.roomnum;
            this.pax = reservation.paxcount;
            getNamesArray(reservation);
        }

        private void getNamesArray(Reservation reservation)
        {
            this.names = new string[pax];

            for (int i = 0; i < pax; i++)
            {
                try
                {
                    names[i] = reservation.pax[i].name;
                }
                catch (Exception ioore)
                {
                    names[i] = "Guest";
                }

            }
            primaryname = names[0];
        }
    }
}
