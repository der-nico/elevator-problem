# elevator-problem
The following repository contains the solution for a prototype of and elevator. 
The program simulates the behaviour of the elvator with given tasks

The following classes are defined
- Program: containing the main function
- Building: A basic building with a single elvator
- Elevator: The actual elevator
- Queue: The queue of the elevator containing all Commands currently not finsihed
- Commands: A command containing the information: id of the person, when it was requested, from where it was requested and where it should go

The input and output of the application is handeled by the ElevatorInputs and the ElevatorOutput classes each using the CSVInput and the CSVOutput classes to read and store data.
The required input is a .csv file where its path is conifgured in the App.config configuration. After running the application the output is stored in a .csv defined in the configuration file. This output contains 4 comma separated columns: the time, a semicolonseparated list of people in the elevator, the current floor the elevator is at and a semicolon separted list of floors that are still requested (in the order they will be targeted).


The main application starts a timer in seconds that runs up and whenever either a new task is requested (from the csv input) or a task is finished (either picking someone up or dropping someone off) the state of the elvator is changed. 
This change contains move the elevator floor accordingly to the time add new task to the queue and start exectuing the next task.

In this prototype some additional output is printed to the screen.
Everytime a task is finalised (the combination of picking up and dropping off) the total time spend on this task is shown.
Additionally the time outside the elvator and the "minimal" exectuion time are printed. The minimal execution time is defined as the time it would take the levator to finish the task without considering any other tasks. See example below  

``` Finished task for 15 in 269s (9s to pick up, minimal time=40)```

Everytime the elvator stops at a floor the current state is printed (identical to the inforamtion store in the output .csv. See example below:

``` State: t=425, people id's=20,  currentfloor=8, floorstogo=9, 8, 7, 6, 5, 10, 4, 3, 2```


