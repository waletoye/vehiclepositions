using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using vehiclepositions.Models;

namespace vehiclepositions.Utilties
{
    class Helper
    {
        internal static void SortByDistance(List<VehiclePosition> vehiclePositions)
        {
            vehiclePositions.Sort((x, y) =>
            {
                return x.DistanceToOrigin.CompareTo(y.DistanceToOrigin);
            });
        }

        internal static void SortByLatitude(List<VehiclePosition> vehiclePositions)
        {
            vehiclePositions.Sort((x, y) =>
            {
                return x.Latitude.CompareTo(y.Latitude);
            });
        }

        internal static List<VehiclePosition> SortByCoord(List<VehiclePosition> vehiclePositions)
        {
            var _vehiclePositions = from coord in vehiclePositions
                         orderby coord.Latitude ascending, coord.Longitude descending
                         select coord;

            return _vehiclePositions.ToList();
        }

        internal static void SortAroundOrigin(List<VehiclePosition> vehiclePositions, Location origin)
        {
            foreach (var position in vehiclePositions)
            {
                position.DistanceToOrigin = GeoCalculator.CalculateDistanceInMeters(position.Location, origin);
            }

            SortByDistance(vehiclePositions);
        }

        internal static VehiclePosition NearestByDistance(List<VehiclePosition> vehiclePositions, double target)
        {
            int n = vehiclePositions.Count;

            // Corner cases
            if (target <= vehiclePositions[0].DistanceToOrigin)
                return vehiclePositions[0];
            if (target >= vehiclePositions[n - 1].DistanceToOrigin)
                return vehiclePositions[n - 1];

            // Doing binary search 
            int i = 0, j = n, mid = 0;
            while (i < j)
            {
                mid = (i + j) / 2;

                if (vehiclePositions[mid].DistanceToOrigin == target)
                    return vehiclePositions[mid];

                /* If target is less 
                than array element,
                then search in left */
                if (target < vehiclePositions[mid].DistanceToOrigin)
                {

                    // If target is greater 
                    // than previous to mid, 
                    // return closest of two
                    if (mid > 0 && target > vehiclePositions[mid - 1].DistanceToOrigin)
                        return GetClosest(vehiclePositions[mid - 1],
                                     vehiclePositions[mid], target);

                    /* Repeat for left half */
                    j = mid;
                }

                // If target is 
                // greater than mid
                else
                {
                    if (mid < n - 1 && target < vehiclePositions[mid + 1].DistanceToOrigin)
                        return GetClosest(vehiclePositions[mid],
                             vehiclePositions[mid + 1], target);
                    i = mid + 1; // update i
                }
            }

            // Only single element
            // left after search
            return vehiclePositions[mid];

            VehiclePosition GetClosest(VehiclePosition val1, VehiclePosition val2, double target)
            {
                if (target - val1.DistanceToOrigin >= val2.DistanceToOrigin - target)
                    return val2;
                else
                    return val1;
            }
        }

        internal static VehiclePosition NearestByLatitude(List<VehiclePosition> vehiclePositions, double latitude)
        {
            int n = vehiclePositions.Count;

            // Corner cases
            if (latitude <= vehiclePositions[0].Latitude)
                return vehiclePositions[0];
            if (latitude >= vehiclePositions[n - 1].Latitude)
                return vehiclePositions[n - 1];

            // Doing binary search 
            int i = 0, j = n, mid = 0;
            while (i < j)
            {
                mid = (i + j) / 2;

                if (vehiclePositions[mid].Latitude == latitude)
                    return vehiclePositions[mid];

                /* If target is less 
                than array element,
                then search in left */
                if (latitude < vehiclePositions[mid].Latitude)
                {

                    // If target is greater 
                    // than previous to mid, 
                    // return closest of two
                    if (mid > 0 && latitude > vehiclePositions[mid - 1].Latitude)
                        return GetClosest(vehiclePositions[mid - 1],
                                     vehiclePositions[mid], latitude);

                    /* Repeat for left half */
                    j = mid;
                }

                // If target is 
                // greater than mid
                else
                {
                    if (mid < n - 1 && latitude < vehiclePositions[mid + 1].Latitude)
                        return GetClosest(vehiclePositions[mid],
                             vehiclePositions[mid + 1], latitude);
                    i = mid + 1; // update i
                }
            }

            // Only single element
            // left after search
            return vehiclePositions[mid];

            VehiclePosition GetClosest(VehiclePosition val1, VehiclePosition val2, double latitude)
            {
                if (latitude - val1.Latitude >= val2.Latitude - latitude)
                    return val2;
                else
                    return val1;
            }
        }

        // This function prints k closest elements 
        // to x in arr[]. n is the number of 
        // elements in arr[]
        internal static List<VehiclePosition> ClosestLatitudes(List<VehiclePosition> vehiclePositions, float target)
        {
            var result = new List<VehiclePosition>();

            int closestCount = 5;
            int n = vehiclePositions.Count;


            // Find the crossover point
            int l = FindCrossOver(vehiclePositions, 0, n - 1, target);

            // Right index to search
            int r = l + 1;

            // To keep track of count of elements
            int count = 0;

            // If x is present in arr[], then reduce 
            // left index Assumption: all elements in
            // arr[] are distinct
            if (vehiclePositions[l].Latitude == target) l--;

            // Compare elements on left and right of 
            // crossover point to find the k closest
            // elements
            while (l >= 0 && r < n && count < closestCount)
            {
                if (target - vehiclePositions[l].Latitude < vehiclePositions[r].Latitude - target)
                    result.Add(vehiclePositions[l--]);
                else
                    result.Add(vehiclePositions[r++]);
                count++;
            }

            // If there are no more elements on right 
            // side, then print left elements
            while (count < closestCount && l >= 0)
            {
                result.Add(vehiclePositions[l--]);
                count++;
            }

            // If there are no more elements on left 
            // side, then print right elements
            while (count < closestCount && r < n)
            {
                result.Add(vehiclePositions[r++]);
                count++;
            }


            return result;

            int FindCrossOver(List<VehiclePosition> vehiclePositions, int low, int high, float x)
            {
                // Base cases
                // x is greater than all
                if (vehiclePositions[high].Latitude <= x)
                    return high;

                // x is smaller than all
                if (vehiclePositions[low].Latitude > x)
                    return low;

                // Find the middle point
                /* low + (high - low)/2 */
                int mid = (low + high) / 2;

                /* If x is same as middle element, then
                return mid */
                if (vehiclePositions[mid].Latitude <= x && vehiclePositions[mid + 1].Latitude > x)
                    return mid;

                /* If x is greater than arr[mid], then 
                either arr[mid + 1] is ceiling of x or
                ceiling lies in arr[mid+1...high] */
                if (vehiclePositions[mid].Latitude < x)
                    return FindCrossOver(vehiclePositions, mid + 1,
                                              high, x);

                return FindCrossOver(vehiclePositions, low, mid - 1, x);
            }
        }

        internal static List<VehiclePosition> ClosestDistances(List<VehiclePosition> vehiclePositions, double targetDistance)
        {
            var result = new List<VehiclePosition>();

            int closestCount = 1;
            int n = vehiclePositions.Count;


            // Find the crossover point
            int l = FindCrossOver(vehiclePositions, 0, n - 1, targetDistance);

            // Right index to search
            int r = l + 1;

            // To keep track of count of elements
            int count = 0;

            // If x is present in arr[], then reduce 
            // left index Assumption: all elements in
            // arr[] are distinct
            if (vehiclePositions[l].DistanceToOrigin == targetDistance) l--;

            // Compare elements on left and right of 
            // crossover point to find the k closest
            // elements
            while (l >= 0 && r < n && count < closestCount)
            {
                if (targetDistance - vehiclePositions[l].Latitude < vehiclePositions[r].DistanceToOrigin - targetDistance)
                    result.Add(vehiclePositions[l--]);
                else
                    result.Add(vehiclePositions[r++]);
                count++;
            }

            // If there are no more elements on right 
            // side, then print left elements
            while (count < closestCount && l >= 0)
            {
                result.Add(vehiclePositions[l--]);
                count++;
            }

            // If there are no more elements on left 
            // side, then print right elements
            while (count < closestCount && r < n)
            {
                result.Add(vehiclePositions[r++]);
                count++;
            }


            return result;

            int FindCrossOver(List<VehiclePosition> vehiclePositions, int low, int high, double x)
            {
                // Base cases
                // x is greater than all
                if (vehiclePositions[high].DistanceToOrigin <= x)
                    return high;

                // x is smaller than all
                if (vehiclePositions[low].DistanceToOrigin > x)
                    return low;

                // Find the middle point
                /* low + (high - low)/2 */
                int mid = (low + high) / 2;

                /* If x is same as middle element, then
                return mid */
                if (vehiclePositions[mid].DistanceToOrigin <= x && vehiclePositions[mid + 1].DistanceToOrigin > x)
                    return mid;

                /* If x is greater than arr[mid], then 
                either arr[mid + 1] is ceiling of x or
                ceiling lies in arr[mid+1...high] */
                if (vehiclePositions[mid].DistanceToOrigin < x)
                    return FindCrossOver(vehiclePositions, mid + 1,
                                              high, x);

                return FindCrossOver(vehiclePositions, low, mid - 1, x);
            }
        }
    }
}
