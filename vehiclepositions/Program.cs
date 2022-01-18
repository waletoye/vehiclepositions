using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using vehiclepositions.Models;
using vehiclepositions.Utilties;

namespace vehiclepositions
{
    class Program
    {
        static void Main()
        {
            const string fileLocation = @"data\VehiclePositions.dat";

            //the 10 vehicles
            List<VehicleLocation> sampleVehicles = Context.LoadSampleVechicles();

            //vehicle positions from binary data
            List<VehiclePosition> vehiclePositions = Context.ReadAllRecords(fileLocation);


            var sw = new Stopwatch();
            sw.Start();
            LinearBulkSearch(vehiclePositions, sampleVehicles);
            Console.WriteLine("===== LinearBulkSearch {0} ms =====", sw.ElapsedMilliseconds);

            Console.WriteLine();
            Console.WriteLine();

            sw.Restart();
            ParallelBulkSearch(vehiclePositions, sampleVehicles);
            Console.WriteLine("===== ParallelBulkSearch {0} ms =====", sw.ElapsedMilliseconds);

            sw.Stop();
            Console.ReadKey();
        }

        /// <summary>
        /// Linear Bulk Search: foreach sample, loop through 2million vehicles linearly
        /// </summary>
        /// <param name="sourcePositions">entire source list</param>
        /// <param name="sampleVehicles">sample vehicle</param>
        internal static void LinearBulkSearch(List<VehiclePosition> sourcePositions, List<VehicleLocation> sampleVehicles)
        {
            // Time Complexity: O(n^2)
            // Space Complexity: O(1) 
            //Processing Time: 4029 ms

            double minDistance = double.MaxValue;
            VehiclePosition nearestVehicle = null;

            foreach (var vehicle in sampleVehicles)
            {
                //reset minDistance for each vehicle
                minDistance = double.MaxValue;

                foreach (var sourcePosition in sourcePositions)
                {
                    var dist = GeoCalculator.CalculateDistanceInMeters(sourcePosition.Location, vehicle);

                    if (dist < minDistance)
                    {
                        minDistance = Math.Min(minDistance, dist);
                        nearestVehicle = sourcePosition;
                    }
                }

                Console.WriteLine("{0} => Vechicle: {1} distance: {2} meters geo: {3}", vehicle, nearestVehicle.VehicleRegistraton, minDistance, nearestVehicle);
            }
        }

        /// <summary>
        /// Parrallel Bulk Search: create 10 threads to concurrently loop through 2million vehicles
        /// </summary>
        /// <param name="sourcePositions">entire source list</param>
        /// <param name="sampleVehicles">sample vehicles</param>
        internal static void ParallelBulkSearch(List<VehiclePosition> sourcePositions, List<VehicleLocation> sampleVehicles)
        {
            // Time Complexity: O(n)
            // Space Complexity: O(1), all threads access the same in-memory data
            //Processing Time: 1342 ms

            int k = 10; //number of threads

            Parallel.For(0, k, count =>
            {
                //Console.WriteLine($"value of count = {count}, thread = {System.Threading.Thread.CurrentThread.ManagedThreadId}");

                GeoCalculator.LinearSingleSearch(sourcePositions, sampleVehicles[count]);
            });
        }
    }
}
