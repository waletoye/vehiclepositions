using System;
using System.Collections.Generic;
using System.Text;

namespace vehiclepositions.Models
{
    public class VehicleLocation : Location
    {
        public Location Location => new Location { Latitude = Latitude, Longitude = Longitude };
        public double MinDistInKM { get; set; }
    }
}