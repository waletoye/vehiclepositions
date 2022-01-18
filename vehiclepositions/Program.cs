using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using vehiclepositions.Models;

namespace vehiclepositions
{
    class Program
    {
        static void Main()
        {
            var sampleVehicles = new List<VehicleLocation>();
            sampleVehicles.Add(new VehicleLocation { Latitude = 34.544909, Longitude = -102.100843 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 32.345544, Longitude = -99.123124 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 33.234235, Longitude = -100.214124 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 35.195739, Longitude = -95.348899 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 31.895839, Longitude = -97.789573 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 32.895839, Longitude = -101.789573 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 34.115839, Longitude = -100.225732 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 32.335839, Longitude = -99.992232 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 33.535339, Longitude = -94.792232 });
            sampleVehicles.Add(new VehicleLocation { Latitude = 32.234235, Longitude = -100.222222 });

            string pth = @"C:\Users\aadetoye\Downloads\mixtele\VehiclePositions\VehiclePositions.dat";
            pth = @"data\VehiclePositions.dat";

            var positions = ReadAllRecords(pth);



            var sw = new Stopwatch();
            sw.Start();
            LinearBulkSearch(positions, sampleVehicles);
            Console.WriteLine("LinearBulkSearch =>  " + sw.ElapsedMilliseconds);

            sw.Restart();
            ParallelBulkSearch(positions, sampleVehicles);
            Console.WriteLine("ParallelBulkSearch =>  " + sw.ElapsedMilliseconds);



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
            VehiclePosition p;

            foreach (var vehicle in sampleVehicles)
            {
                foreach (var sourcePosition in sourcePositions)
                {
                    var dist = CalculateDistanceInMeters(sourcePosition.Location, vehicle);

                    if (dist < minDistance)
                        p = sourcePosition;

                    minDistance = Math.Min(minDistance, dist);
                }
                vehicle.MinDistInKM = minDistance;
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
            // Space Complexity: O(k), k is number of threads
            //Processing Time: 1342 ms

            int k = 10; //number of threads

            Parallel.For(0, k, count =>
            {
                //Console.WriteLine($"value of count = {count}, thread = {System.Threading.Thread.CurrentThread.ManagedThreadId}");

                LinearSingleSearch(sourcePositions, sampleVehicles[count]);
            });
        }


        /// <summary>
        /// Linear Search
        /// </summary>
        /// <param name="sourcePositions">entire source list</param>
        /// <param name="location">location to compare</param>
        internal static void LinearSingleSearch(List<VehiclePosition> sourcePositions, VehicleLocation location)
        {
            // Time Complexity: O(n)
            // Space Complexity: O(1)
            //Processing Time: 484 ms

            var sw = new Stopwatch();
            sw.Start();

            double minDistance = double.MaxValue;

            foreach (var sourcePosition in sourcePositions)
            {
                var dist = CalculateDistanceInMeters(sourcePosition.Location, location);
                minDistance = Math.Min(minDistance, dist);
            }

            sw.Stop();
            Console.WriteLine(minDistance + " " + sourcePositions.Count + " =>  " + sw.ElapsedMilliseconds);

            location.MinDistInKM = minDistance;
        }

        /// <summary>
        /// Load all vechicle positions from binary data
        /// </summary>
        /// <param name="fileName">binary file path</param>
        /// <returns></returns>
        internal static List<VehiclePosition> ReadAllRecords(string fileName)
        {
            var vehiclePositions = new List<VehiclePosition>();

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                {
                    // Reading character by character while you can read
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        try
                        {
                            var vechpos = new VehiclePosition
                            {
                                Position = br.ReadInt32(),
                                VehicleRegistraton = Encoding.ASCII.GetString(br.ReadBytes(10)), //.Substring(0,9),
                                Latitude = br.ReadSingle(),
                                Longitude = br.ReadSingle(),
                                RecordedTimeUTC = br.ReadUInt64(),
                            };

                            vehiclePositions.Add(vechpos);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return vehiclePositions;
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
