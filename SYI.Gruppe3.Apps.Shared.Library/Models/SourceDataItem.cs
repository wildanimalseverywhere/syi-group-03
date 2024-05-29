using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace SYI.Gruppe3.Apps.Shared.Library.Models
{
    public class SourceDataItem
    {
        [Name("Borough")]
        public string Borough { get; set; }
        [Name("Contributing Factor Vehicle 1")]
        public string ContributingFactorVehicle1 { get; set; }
        [Name("Contributing Factor Vehicle 2")]
        public string ContributingFactorVehicle2 { get; set; }
        [Name("Crash Date")]
        public DateTime CrashDate { get; set; }
        [Name("Cross Street Name")]
        public string CrossStreetName { get; set; }
        [Name("Location")]
        public string Location { get; set; }
        [Name("On Street Name")]
        public string OnStreetName { get; set; }
        [Name("Vehicle Type Code 1")]
        public string VehicleTypeCode1 { get; set; }
        [Name("Vehicle Type Code 2")]
        public string VehicleTypeCode2 { get; set; }
        [Name("Zip Code")]
        public int ZipCode { get; set; }
        public int Hour { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Minute { get; set; }
        [Name("Number Of Persons Injured")]
        public int NumberOfPersonsInjured { get; set; }
        [Name("Number Of Persons Killed")]
        public int NumberOfPersonsKilled { get; set; }
    }
}
