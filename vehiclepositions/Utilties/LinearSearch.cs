using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using vehiclepositions.Models;

namespace vehiclepositions.Utilties
{
    public class LinearSearch
    {

        /// <summary>
        /// Linear Search
        /// </summary>
        /// <param name="sampleVehicles"></param>
        /// <param name="vehiclePositions"></param>
        internal static void Linear_ParallelSearch(List<VehicleLocation> sampleVehicles, List<VehiclePosition> vehiclePositions)
        {
            var sw = new Stopwatch();
            sw.Start();
            LinearBulkSearch(vehiclePositions, sampleVehicles);
            Logger.ConsoleLogger(msg: $"===== Linear BulkSearch {sw.ElapsedMilliseconds} ms ===== \n\t Time Complexity: O(n^2) \n\n");

            sw.Restart();
            ParallelBulkSearch(vehiclePositions, sampleVehicles);
            Logger.ConsoleLogger(msg: $"===== Parallel BulkSearch {sw.ElapsedMilliseconds} ms ===== \n\t Time Complexity: \n\n");

            sw.Stop();
        }

        /// <summary>
        /// Linear Bulk Search: foreach sample, loop through 2million vehicles linearly
        /// </summary>
        /// <param name="sourcePositions">entire source list</param>
        /// <param name="sampleVehicles">sample vehicle</param>
        private static void LinearBulkSearch(List<VehiclePosition> sourcePositions, List<VehicleLocation> sampleVehicles)
        {
            // Time Complexity: O(n^2)
            // Space Complexity: O(1) 
            // Processing Time: 4029 ms

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

                Logger.ConsoleLogger(vehicle, nearestVehicle.VehicleRegistraton, minDistance, nearestVehicle);
            }
        }

        /// <summary>
        /// Parrallel Bulk Search: create 10 threads to concurrently search through 2million vehicles
        /// </summary>
        /// <param name="sourcePositions">entire source list</param>
        /// <param name="sampleVehicles">sample vehicles</param>
        private static void ParallelBulkSearch(List<VehiclePosition> sourcePositions, List<VehicleLocation> sampleVehicles)
        {
            // Time Complexity: 
            // Space Complexity: O(1), all threads access the same in-memory data
            // Processing Time: 1342 ms

            int p = 10; //number of threads

            Parallel.For(0, p, threadId =>
            {
                //Console.WriteLine($"value of p = {threadId}, thread = {System.Threading.Thread.CurrentThread.ManagedThreadId}");

                GeoCalculator.LinearSingleSearch(sourcePositions, sampleVehicles[threadId]);
            });
        }

    }
}
