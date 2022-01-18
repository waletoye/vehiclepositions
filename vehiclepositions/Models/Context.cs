using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace vehiclepositions.Models
{
    class Context
    {
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

        /// <summary>
        /// The 10 vehicles
        /// </summary>
        /// <returns>List of the 10 vehicles</returns>
        internal static List<VehicleLocation> LoadSampleVechicles()
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

            return sampleVehicles;
        }
    }
}
