# SwimStat - Swim Coach’s Statistics Tool
SwimStat is a command-line application written in C# aimed to assist swim coaches in analyzing the performance of their swimmers. This tool will simplify the process of tracking swimmers’ times, identifying top swimmers, monitoring improvements over time, and forming relay teams based on the swimmers’ specialties. SwimStat features a friendly command-line interface to view and analyze swimmer statistics. 
# Requirements
- dotnet installed (.NET CLI)
- .NET SDK is at version 8.0.0
# Usage
1. Navigate to the root directory of application:
```
dotnet build
```

2. Run the program with the test data in the repository, `testdata.csv`:
```
dotnet run import testdata.csv
```

3.  You should see a help menu appear once the program has started:
```
Import successful. Number of results imported: 50
Usage: [command]

These are common SwimStat commands used in various situations:

  create        Create a new relay team
  modify        Add or Remove a swimmer from relay team
  listrelays    List all relay teams
  listswimmers  List all swimmers
  performance   View individual swimmer performance
  sort          Sort swimmers
  help          Print this help menu

Type 'exit' at any time to exit the program

> 
```
# Features
### Help Menu
To view the help menu again, you can type `help` into the console. 
```
> help
Usage: [command]

These are common SwimStat commands used in various situations:

  create        Create a new relay team
  modify        Add or Remove a swimmer from relay team
  listrelays    List all relay teams
  listswimmers  List all swimmers
  performance   View individual swimmer performance
  sort          Sort swimmers
  help          Print this help menu

Type 'exit' at any time to exit the program
```
### List Swimmers
To view the list of swimmers and IDs, you can type `listswimmers` into the console.
```
> listswimmers
Swimmer ID: 101, Name: John Doe
Swimmer ID: 102, Name: James Smith
Swimmer ID: 103, Name: Michael Johnson
Swimmer ID: 104, Name: John Davis
Swimmer ID: 105, Name: Chris Anderson
```
### Sort Command
You can view the imported, sorted results, you can type `sort` into the console.
```
> sort
Available Events:
0: 100 Freestyle
1: 200 Freestyle
```
You will be prompted to pick one of the events. In this case, you can choose either 0 for 100 freestyle or 1 for 200 freestyle. Once selected, you will see a list of results with the rank, swimmer name, time, and event date. The snippiet below is abbreviated for this readme. 
```
Enter the number of the event you want to see results for:
0
Results for 100 Freestyle:
                        Name       Time        Date
 1               James Smith   00:51.80  07/20/2023
 2               James Smith   00:51.90  07/05/2023
```

### Performance
You can view the performance of an individual swimmer by using the `performance` command.
```
> performance
Enter the swimmer ID to add or 'list' to show all swimmers:
```
You will be prompted to choose a swimmer ID, but optionally you can choose to take another glance at the swimmers using `list` if you forgot the swimmer's ID. 
```
list
Swimmer ID: 101, Name: John Doe
Swimmer ID: 102, Name: James Smith
Swimmer ID: 103, Name: Michael Johnson
Swimmer ID: 104, Name: John Davis
Swimmer ID: 105, Name: Chris Anderson
```
Once a swimmer is selected, it will display each event and their performance in it over time. 
```
Enter the swimmer ID to add or 'list' to show all swimmers:
103
Performance for Michael Johnson:
100 Freestyle on 07/01/2023: 00:55.78 - New PB!
100 Freestyle on 07/05/2023: 00:55.60 - New PB!
100 Freestyle on 07/10/2023: 00:55.90 (+00:00.30)
100 Freestyle on 07/15/2023: 00:56.10 (+00:00.50)
100 Freestyle on 07/20/2023: 00:55.70 (+00:00.10)

200 Freestyle on 07/01/2023: 02:00.45 - New PB!
200 Freestyle on 07/05/2023: 01:59.95 - New PB!
200 Freestyle on 07/10/2023: 01:59.50 - New PB!
200 Freestyle on 07/15/2023: 01:59.20 - New PB!
200 Freestyle on 07/20/2023: 01:59.65 (+00:00.45)
```

### Building a Relay
To view all the currently setup relays, you can type `listrelays` into the console. 
```
> listrelays
There are no relay teams to display.
```
There are none created yet, so we need to type `create` into the console to create a relay, and follow the on screen prompts to enter the ID and Team Name. 
```
> create
Enter relay ID:
123
Enter relay team name:
Fun Relay 
Relay team 'Fun Relay' created with ID: 123.
```
Once a relay is created, you can type `modify` into the console to add or remove a swimmer from the relay.
```
> modify
Enter the relay team ID or 'list' to show all relay teams:
```
You will be prompted to choose a relay team ID, but optionally you can choose to see all the relays by typing `list`. 
```
list
Relay Team ID: 123, Name: Fun Relay
Team Members:
```
Once an ID is chosen, you can follow the prompts to add or remove a swimmer. You can once again view the list of swimmers before adding them to a relay, to ensure you have picked the correct ID. 
```
Enter the relay team ID or 'list' to show all relay teams:
123
Do you want to 'add' or 'remove' a swimmer? (add/remove)
add
Enter the swimmer ID to add or 'list' to show all swimmers:
list
Swimmer ID: 101, Name: John Doe
Swimmer ID: 102, Name: James Smith
Swimmer ID: 103, Name: Michael Johnson
Swimmer ID: 104, Name: John Davis
Swimmer ID: 105, Name: Chris Anderson
Enter the swimmer ID to add or 'list' to show all swimmers:
103
Swimmer with ID 103 has been added to the relay team.
```
Removing a Swimmer involves a very similar workflow. 
```
> modify
Enter the relay team ID or 'list' to show all relay teams:
123
Do you want to 'add' or 'remove' a swimmer? (add/remove)
remove
Enter the swimmer ID to remove or 'list' to show all swimmers in the relay:
103
Swimmer Michael Johnson (ID: 103) has been removed from the relay team.
```
Lastly, one can list out all the relay teams they have created.
```
> listrelays
Relay Team ID: 123, Name: Fun Relay
Team Members:
- Swimmer ID: 101, Name: John Doe
- Swimmer ID: 102, Name: James Smith
- Swimmer ID: 103, Name: Michael Johnson
- Swimmer ID: 104, Name: John Davis
```
