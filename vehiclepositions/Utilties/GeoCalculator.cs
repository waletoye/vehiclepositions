using System;
using System.Collections.Generic;
using System.Diagnostics;
using vehiclepositions.Models;

namespace vehiclepositions.Utilties
{
    public class GeoCalculator
    {
        /// <summary>
        /// Linear Search
        /// </summary>
        /// <param name="sourcePositions">entire source list</param>
        /// <param name="sampleVechicle">sampleVechicle location to compare</param>
        internal static void LinearSingleSearch(List<VehiclePosition> sourcePositions, VehicleLocation sampleVechicle)
        {
            // Time Complexity: O(n)
            // Space Complexity: O(1)
            //Processing Time: 484 ms

            var sw = new Stopwatch();
            sw.Start();

            double minDistance = double.MaxValue;
            VehiclePosition nearestVehicle = null;

            foreach (var sourcePosition in sourcePositions)
            {
                var dist = CalculateDistanceInMeters(sourcePosition.Location, sampleVechicle);

                if(dist < minDistance)
                {
                    minDistance = Math.Min(minDistance, dist);
                    nearestVehicle = sourcePosition;
                }
            }

            sw.Stop();
            Console.WriteLine("{0} => Vechicle: {1} distance: {2} meters geo: {3}", sampleVechicle, nearestVehicle.VehicleRegistraton, minDistance, nearestVehicle);
            //Console.WriteLine(minDistance + " " + sourcePositions.Count + " =>  " + sw.ElapsedMilliseconds);
        }

        internal static double CalculateDistanceInMeters(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            var result = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));

            return result;
        }
    }
}
