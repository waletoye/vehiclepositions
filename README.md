# vehiclepositions
 
## FLOW
Load data from binary file, deserialise into objects

### Assumptions Made
* The vechile position coordinates are unsorted

### Case 1
Linear BulkSearch  
 This is the current benchmark  
 For each of the 10 vehicles, find the nearest position  
 * Time Complexity: O(n^2)  
 * Space Complexity: O(1)  
 * Average Processing Time: 4029 ms  
 
 ### Case 2
Parallel BulkSearch  
Concurrently search for the nearest position using threads
 * Time Complexity: O(n)  
 * Space Complexity: O(1),  all threads access the same in-memory data  
 * Average Processing Time: 1342 ms  
