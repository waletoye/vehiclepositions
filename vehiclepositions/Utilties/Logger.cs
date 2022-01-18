using System;
using System.Collections.Generic;
using System.Text;
using vehiclepositions.Models;

namespace vehiclepositions.Utilties
{
    class Logger
    {
        internal static void ConsoleLogger(VehicleLocation sampleVechicle, string registrationPlate, double minDistance, VehiclePosition nearestVehicle)
        {
            Console.WriteLine("{0} => Vechicle: {1}, distance: {2} meters, geo: {3}", sampleVechicle, registrationPlate, minDistance, nearestVehicle);
        }

        internal static void ConsoleLogger(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
