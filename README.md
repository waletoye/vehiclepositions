# vehiclepositions
 
## WALKTHROUGH
1. Load data from binary file
2. Deserialise into objects 
3. Handle Case 1 or Case 2 

<br />

Case 1 shows the current benchmark, while Case 2 demonstrates an optimzation  

The goal is to reduce the Time Complexity from the current benchmark of O(n^2) to O(n)   
**O(n^2)** > O(n log n) > **O(n)** > O(log n), O(1)  


### Assumptions / Ideas
* The app machine is has multiple cores
* The vechile position coordinates are unsorted
   * Thus requires visiting each item
* Sorting is not allowed, since there's no global 'reference point'
   * Without sorting, the likely Time Complexity is variations of O(n)
   * Also sorting is meaningless since we are looking for "nearest" vehicles not exact match

### Case 1 - Linear BulkSearch  
 This is the current benchmark  
 For each of the 10 vehicles, find the nearest position  
 * Time Complexity: O(n^2)  
 * Space Complexity: O(1)  
 * Average Processing Time: 4029 ms  
 
 ### Case 2 - Parallel BulkSearch  
Concurrently search for the nearest position using parallel search
 * Time Complexity: O(n)  
 * Space Complexity: O(1),  all threads access the same in-memory data  
 * Average Processing Time: 1342 ms  
