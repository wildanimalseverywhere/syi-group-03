using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYI.Gruppe3.Apps.Shared.Library.Models
{
    public class SourceDataItem
    {
        public string Borough { get; set; }
        public string ContributingFactorVehicle1 { get; set; }
        public string ContributingFactorVehicle2 { get; set; }
        public DateTime CrashDate { get; set; }
        public string CrossStreetName { get; set; }
        public string Location { get; set; }
        public string OnStreetName { get; set; }
        public string VehicleTypeCode1 { get; set; }
        public string VehicleTypeCode2 { get; set; }
        public int ZipCode { get; set; }
        public int Hour { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Minute { get; set; }
        public int NumberOfPersonsInjured { get; set; }
        public int NumberOfPersonsKilled { get; set; }
    }
}
