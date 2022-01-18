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

        internal static List<VehicleLocation> LoadSampleVechicles()
        {

        }
    }
}
