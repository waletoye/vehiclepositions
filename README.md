# vehiclepositions
 
## FLOW
Load data from binary file, deserialise into objects

### Assumptions Made
* The vechile position coordinates are unsorted

### Case 1
Linear BulkSearch  
 This is the current benchmark  
 * Time Complexity: O(n^2)  
 * Space Complexity: O(1)  
 * Average Processing Time: 4029 ms  
 
 ### Case 2
Parallel BulkSearch  
Divide the list (2M) into 10 and use a thread each to concurrently search each of the 200,000 items
 * Time Complexity: O(n/p),  n => 2million, p => no of threads    
 * Space Complexity: O(1),  all threads access the same in-memory data  
 * Average Processing Time: 1342 ms  
