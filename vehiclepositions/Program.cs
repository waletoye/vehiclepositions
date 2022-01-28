using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using vehiclepositions.Models;
using vehiclepositions.Utilties;

namespace vehiclepositions
{
    class Program
    {
        static void Main()
        {
            const string fileLocation = @"Data/VehiclePositions.dat";

            //the 10 vehicles
            List<VehicleLocation> sampleVehicles = Context.LoadSampleVechicles();

            //vehicle 2M positions from binary data
            List<VehiclePosition> vehiclePositions = Context.ReadAllRecords(fileLocation);



            //try 1: linear and parallel search approach
            //Utilties.LinearSearch.Linear_ParallelSearch(sampleVehicles, vehiclePositions);

            //try 2: KDTree approach
            NearestNeighbourSearch(vehiclePositions, sampleVehicles);


            Console.ReadKey();
        }

        /// <summary>
        /// Nearest Neighbour Search using KDTree
        /// </summary>
        /// <param name="vehiclePositions"></param>
        /// <param name="sampleVehicles"></param>
        static void NearestNeighbourSearch(List<VehiclePosition> vehiclePositions, List<VehicleLocation> sampleVehicles)
        {
            // Time Complexity: (n log n)
            // Space Complexity: O(1), all threads access the same in-memory data
            // Processing Time: 6060ms to build tree 
            //                  43ms to pefrom the 10 searches 



            var sw = new Stopwatch();
            sw.Start();
            KDTree kdt = new KDTree(vehiclePositions.Count);
            foreach (var point in vehiclePositions)
            {
                kdt.Add(new double[] { point.Latitude, point.Longitude });
            }



            sw.Stop();
            Logger.ConsoleLogger(msg: $"===== KDTree {sw.ElapsedMilliseconds} ms ===== \n\t Time Complexity: O(n log n)");


            for (int i = 0; i < sampleVehicles.Count; i++)
            {
                Node kdn = kdt.Find_Nearest(new double[] { sampleVehicles[i].Latitude, sampleVehicles[i].Longitude });
                Console.WriteLine("lat:{0}, long:{1}  ==> lat:{2}, long:{3}", sampleVehicles[i].Latitude, sampleVehicles[i].Longitude, kdn.x[0], kdn.x[1]);
            }

        }
    }
}