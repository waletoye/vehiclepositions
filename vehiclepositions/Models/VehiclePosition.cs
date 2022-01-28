using System;
using System.Collections.Generic;
using System.Text;

namespace vehiclepositions.Models
{
    public class VehiclePosition
    {
        public int Position { get; set; }
        public string VehicleRegistraton { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public ulong RecordedTimeUTC { get; set; }
        public double DistanceToOrigin { get; internal set; }

        internal Location Location => new Location() { Latitude = Latitude, Longitude = Longitude };

        public override string ToString() => $"{Latitude},{Longitude}";
        //internal DateTimeOffset Date => DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(RecordedTimeUTC.ToString()));
    }
}
