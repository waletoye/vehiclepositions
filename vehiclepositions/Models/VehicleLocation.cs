using System;
using System.Collections.Generic;
using System.Text;

namespace vehiclepositions.Models
{
    public class VehicleLocation : Location
    {
        public override string ToString() => $"{Latitude},{Longitude}";
    }
}