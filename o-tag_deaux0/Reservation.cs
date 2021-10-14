using System;
using System.Collections.Generic;

namespace o_tag_deaux0

{
    public class Reservation
    {
        public string confirmationid { get; set; }
        public string result { get; set; }
        public string msg { get; set; }
        public string nopost { get; set; }
        public string status { get; set; }
        public DateTime dateIndate { get; set; }
        public DateTime dateOutdate { get; set; }
        public string indate { get; set; }
        public string outdate { get; set; }
        public string roomnum { get; set; }
        public int paxcount { get; set; }
        public List<Pax> pax { get; set; }
        public string[] names { get;  set; }
        public string primaryname { get; set; }
    }
}