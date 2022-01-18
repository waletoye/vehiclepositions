# vehiclepositions
 
## FLOW
Load data from binary file, deserialise into objects

### Case 1
Linear BulkSearch  
 This is the current benchmark  
 * Time Complexity: O(n^2)  
 * Space Complexity: O(1)  
 * Average Processing Time: 4029 ms  
 
 ### Case 2
Parallel BulkSearch
 * Time Complexity: O(n)  
 * Space Complexity: O(k), k is number of threads  
 * Average Processing Time: 1342 ms  
