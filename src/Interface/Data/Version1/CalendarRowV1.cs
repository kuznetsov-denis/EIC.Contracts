using System;
using System.Runtime.Serialization;

namespace EIC.Contracts.Data.Version1
{
    [DataContract]
    public class CalendarRow
    {
        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "days")]
        public int Days { get; set; }

        [DataMember(Name = "body")]
        public double Body { get; set; }

        [DataMember(Name = "percents")]
        public double Percens { get; set; }

        [DataMember(Name = "rest")]
        public double Rest { get; set; }
    }

}
