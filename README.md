# SpatialRovers

Backend: 
- in the case of wanting to create more types of Rovers, we should create an abstract class and make each Rover inherit from it
- in that case the code can be adapted to use the Factory pattern as each type of Rover needs to decide how to "act" based on the instructions he recieves
- we would extract the functionality of Act and RunSequence as virtual methods in this abstract class as to have the option of overriding and extending.

Frontend: 
- Add a functionality to create additional rovers after uploading the initial CSV file
- Better styling (colour coded and tooltips as to have it still visible even with multiple rovers).

How to run:
- Run the application and upload a CSV file like the one attached in the repository => Parse => Start Rovers
- Can also create a text file, put one entry for a rover on a new line and save the file as .CSV
