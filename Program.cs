using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

class Program
{

    // Class to hold state and related methods
    public static class Team
    {
        public static List<SwimResult> SwimResults { get; set; } = new List<SwimResult>();
        public static List<Swimmer> Swimmers { get; set; } = new List<Swimmer>();
        public static List<Relay> Relays { get; set; } = new List<Relay>();

        public static void ListRelays()
        {
            if (Relays.Count == 0)
            {
                Console.WriteLine("There are no relay teams to display.");
            }

            foreach (var relay in Relays)
            {
                Console.WriteLine($"Relay Team ID: {relay.Id}, Name: {relay.Name}");
                Console.WriteLine("Team Members:");
                foreach (var member in relay.Swimmers)
                {
                    Console.WriteLine($"- Swimmer ID: {member.Id}, Name: {member.Name}");
                }
            }
        }

        public static void ListSwimmers()
        {
            if (Swimmers.Count == 0)
            {
                Console.WriteLine("There are no swimmers to display.");
            }
            foreach (var swimmer in Swimmers)
            {
                Console.WriteLine($"Swimmer ID: {swimmer.Id}, Name: {swimmer.Name}");
            }
        }
    }

    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "import")
        {
            string filePath = args[1];
            try
            {
                ImportSwimResults(filePath);
                Console.WriteLine("Import successful. Number of results imported: " + Team.SwimResults.Count);
                startCLI();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during import: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Please specify a command. Example usage: Program import [filepath]");
        }
    }

    static void ImportSwimResults(string filePath)
    {
        var lines = File.ReadAllLines(filePath).Skip(1); // Skip the CSV header

        int lineCount = 2; // Used for debugging purposes, can tell you where there might be bad data
        foreach (var line in lines)
        {
            var columns = line.Split(',');

            if (columns.Length != 7)
            {
                throw new InvalidOperationException($"Line {lineCount}: Expected 7 columns, found {columns.Length}.");
            }

            if (!int.TryParse(columns[0], out int resultId))
            {
                throw new InvalidOperationException($"Line {lineCount}: Invalid ResultId '{columns[0]}'.");
            }

            if (!int.TryParse(columns[1], out int eventDistance))
            {
                throw new InvalidOperationException($"Line {lineCount}: Invalid EventDistance '{columns[1]}'.");
            }

            if (!TimeSpan.TryParseExact(columns[3], @"mm\:ss\.ff", CultureInfo.InvariantCulture, out TimeSpan time))
            {
                throw new InvalidOperationException($"Line {lineCount}: Invalid Time '{columns[3]}'. Expected format is 'mm:ss.ff'.");
            }

            if (!int.TryParse(columns[4], out int swimmerId))
            {
                throw new InvalidOperationException($"Line {lineCount}: Invalid SwimmerId '{columns[4]}'.");
            }

            if (!DateTime.TryParse(columns[6], out DateTime eventDate))
            {
                throw new InvalidOperationException($"Invalid date format in line: {line}");
            }

            var existingSwimmer = Team.Swimmers.FirstOrDefault(s => s.Id == swimmerId);
            Swimmer swimmer;
            if (existingSwimmer != null) // Check to see if swimmer exists first
            {
                swimmer = existingSwimmer;
            }
            else // If not, create a new swimmer
            {
                swimmer = new Swimmer(swimmerId, columns[5]);
                Team.Swimmers.Add(swimmer);
            }

            var result = new SwimResult(resultId, eventDistance, columns[2], time, swimmer, eventDate);

            // Add to team results
            if (!Team.SwimResults.Contains(result))
            {
                Team.SwimResults.Add(result);
            }

            // Add to individual result
            if (!swimmer.SwimResults.Contains(result))
            {
                swimmer.SwimResults.Add(result);
            }

            lineCount++;
        }
    }

    static void startCLI()
    {
        string input = "";
        PrintHelp();
        while (input.ToLower() != "exit")
        {
            Console.Write("\n> ");
            input = Console.ReadLine() ?? string.Empty;
            string[] commandParts = input.Split(' ');
            switch (commandParts[0].ToLower())
            {
                case "create":
                    CreateRelay();
                    break;
                case "modify":
                    ModifyRelay();
                    break;
                case "listrelays":
                    Team.ListRelays();
                    break;
                case "listswimmers":
                    Team.ListSwimmers();
                    break;
                case "sort":
                    ChooseAndSortEvent();
                    break;
                case "performance":
                    ViewPerformance();
                    break;
                case "help":
                    PrintHelp();
                    break;
                case "exit":
                    Console.WriteLine("Exiting program.");
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("Usage: [command]\n");
        Console.WriteLine("These are common SwimStat commands used in various situations:\n");
        Console.WriteLine("  create        Create a new relay team");
        Console.WriteLine("  modify        Add or Remove a swimmer from relay team");
        Console.WriteLine("  listrelays    List all relay teams");
        Console.WriteLine("  listswimmers  List all swimmers");
        Console.WriteLine("  performance   View individual swimmer performance");
        Console.WriteLine("  sort          Sort swimmers");
        Console.WriteLine("  help          Print this help menu\n");
        Console.WriteLine("Type 'exit' at any time to exit the program");
    }

    static void CreateRelay()
    {
        Console.WriteLine("Enter relay ID:");
        if (!int.TryParse(Console.ReadLine(), out int relayId))
        {
            Console.WriteLine("Invalid relay ID. Please enter a numeric value.");
            return;
        }

        // Check if Relay ID already exists
        if (Team.Relays.Any(rt => rt.Id == relayId))
        {
            Console.WriteLine($"A relay with ID {relayId} already exists.");
            return;
        }

        // Save Team Name
        Console.WriteLine("Enter relay team name:");
        string name = Console.ReadLine() ?? string.Empty;

        // Create Relay
        Relay relay = new Relay(relayId, name);
        Team.Relays.Add(relay);
        Console.WriteLine($"Relay team '{name}' created with ID: {relayId}.");
    }

    static void ModifyRelay()
    {
        // If there are no relays, quit the function
        if (Team.Relays.Count == 0)
        {
            Console.WriteLine("There are no relay teams. Create a relay team first.");
            return;
        }
        int relayId = -1;
        Relay? relay = null;
        // Stay in loop until user picks a relay ID
        while (relay == null)
        {
            Console.WriteLine("Enter the relay team ID or 'list' to show all relay teams:");
            string input = Console.ReadLine() ?? string.Empty;

            // Show all relay teams
            if (input.ToLower() == "list")
            {
                Team.ListRelays();
            }
            else if (int.TryParse(input, out relayId))
            {
                // Find relay by ID
                relay = Team.Relays.FirstOrDefault(t => t.Id == relayId);
                if (relay == null)
                {
                    Console.WriteLine($"Relay team with ID {relayId} does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a numeric relay team ID or 'list'.");
            }
        }

        Console.WriteLine("Do you want to 'add' or 'remove' a swimmer? (add/remove)");
        string action = Console.ReadLine() ?? string.Empty;

        switch (action.ToLower())
        {
            case "add":
                AddSwimmerToRelay(relay);
                break;
            case "remove":
                RemoveSwimmerFromRelay(relay);
                break;
            default:
                Console.WriteLine("Invalid action. Please enter 'add' or 'remove'.");
                break;
        }
    }

    static void AddSwimmerToRelay(Relay relay)
    {
        Swimmer? swimmer = null;
        while (swimmer == null)
        {
            Console.WriteLine("Enter the swimmer ID to add or 'list' to show all swimmers:");
            string input = Console.ReadLine() ?? string.Empty;

            // Show all swimmers
            if (input.ToLower() == "list")
            {
                Team.ListSwimmers();
            }
            else if (int.TryParse(input, out int swimmerId))
            {
                // Find swimmer by ID
                swimmer = Team.Swimmers.FirstOrDefault(s => s.Id == swimmerId);
                if (swimmer == null)
                {
                    Console.WriteLine($"Swimmer with ID {swimmerId} does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a numeric swimmer ID or 'list'.");
            }
        }
        relay.AddSwimmer(swimmer);
    }

    static void RemoveSwimmerFromRelay(Relay relay)
    {
        Swimmer? swimmer = null;
        while (swimmer == null)
        {
            Console.WriteLine("Enter the swimmer ID to remove or 'list' to show all swimmers in the relay:");
            string input = Console.ReadLine() ?? string.Empty;

            // Show all relay swimmers
            if (input.ToLower() == "list")
            {
                relay.ListSwimmers();
            }
            else if (int.TryParse(input, out int swimmerId))
            {
                // Find relay swimmer by ID
                swimmer = Team.Swimmers.FirstOrDefault(s => s.Id == swimmerId);
                if (swimmer == null)
                {
                    Console.WriteLine($"Swimmer with ID {swimmerId} does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a numeric swimmer ID or 'list'.");
            }
        }
        relay.RemoveSwimmer(swimmer);
    }

    static void ChooseAndSortEvent()
    {
        // Put all event names in a numbered list so it's easy to pick from as a user
        var eventNames = Team.SwimResults
            .Select(result => $"{result.EventDistance} {result.Stroke}")
            .Distinct()
            .OrderBy(e => e)
            .ToList();

        if (eventNames.Count == 0)
        {
            Console.WriteLine("No events found.");
            return;
        }

        Console.WriteLine("Available Events:");
        for (int i = 0; i < eventNames.Count; i++)
        {
            Console.WriteLine($"{i}: {eventNames[i]}");
        }

        Console.WriteLine("Enter the number of the event you want to see results for:");
        // Prevent out of range event numbers from user
        if (!int.TryParse(Console.ReadLine(), out int eventNumber) || eventNumber < 0 || eventNumber >= eventNames.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        SortSwimmersByEvent(eventNames[eventNumber]);
    }

    static void SortSwimmersByEvent(string eventName)
    {
        // Split up the event distance and stroke
        var parts = eventName.Split(' ');
        if (parts.Length != 2 || !int.TryParse(parts[0].Replace("m", ""), out int eventDistance))
        {
            Console.WriteLine("Invalid event format.");
            return;
        }
        string stroke = parts[1];

        // Put all results in a sorted list based on time
        var eventResults = Team.SwimResults
            .Where(result => result.EventDistance == eventDistance && result.Stroke.Equals(stroke, StringComparison.OrdinalIgnoreCase))
            .OrderBy(result => result.Time)
            .ToList();

        if (eventResults.Count == 0)
        {
            Console.WriteLine($"No results found for event '{eventName}'.");
            return;
        }

        int rank = 1; // Hold rank of swimmer in that event
        Console.WriteLine($"Results for {eventName}:");
        Console.WriteLine($"   {"Name", 25}{"Time", 11}{"Date", 12}");
        foreach (var result in eventResults)
        {
            var formattedTime = result.Time.ToString(@"mm\:ss\.ff");
            var formattedEventDate = result.EventDate.ToString("MM/dd/yyyy");
            Console.WriteLine($"{rank, 2} {result.Swimmer.Name, 25}   {formattedTime}  {formattedEventDate}");
            rank++;
        }
    }

    static void ViewPerformance()
    {
        Swimmer? swimmer = null;
        while (swimmer == null)
        {
            Console.WriteLine("Enter the swimmer ID to add or 'list' to show all swimmers:");
            string input = Console.ReadLine() ?? string.Empty;

            // Show all swimmers
            if (input.ToLower() == "list")
            {
                Team.ListSwimmers();
            }
            else if (int.TryParse(input, out int swimmerId))
            {
                // Find swimmer by ID
                swimmer = Team.Swimmers.FirstOrDefault(s => s.Id == swimmerId);
                if (swimmer == null)
                {
                    Console.WriteLine($"Swimmer with ID {swimmerId} does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a numeric swimmer ID or 'list'.");
            }
        }

        var sortedResults = swimmer.SwimResults
        .OrderBy(result => result.EventDistance)
        .ThenBy(result => result.Stroke)
        .ThenBy(result => result.EventDate);

        Console.WriteLine($"Performance for {swimmer.Name}:");

        // Hold the best times for each event-stroke combination
        var bestTimes = new Dictionary<string, TimeSpan>();

        // Holds last event so that the events can be split up visually
        string? lastEventKey = null;

        foreach (var result in sortedResults)
        {
            string eventKey = $"{result.EventDistance} {result.Stroke}";

            if (lastEventKey != null && lastEventKey != eventKey)
            {
                Console.WriteLine(); // Line break between different events
            }
            lastEventKey = eventKey;

            string formattedDate = result.EventDate.ToString("MM/dd/yyyy");
            string formattedTime = result.Time.ToString(@"mm\:ss\.ff");

            // Check if this result is a new PB and update bestTimes if so
            if (!bestTimes.ContainsKey(eventKey) || result.Time < bestTimes[eventKey])
            {
                bestTimes[eventKey] = result.Time;
                Console.WriteLine($"{result.EventDistance} {result.Stroke} on {formattedDate}: {formattedTime} - New PB!");
            }
            else
            {
                // Calculate the time gain or drop from their PB
                TimeSpan timeDifference = result.Time - bestTimes[eventKey];
                string timeDiffFormatted = (timeDifference < TimeSpan.Zero ? "-" : "+") + timeDifference.ToString(@"mm\:ss\.ff").TrimStart('-');
                Console.WriteLine($"{result.EventDistance} {result.Stroke} on {formattedDate}: {formattedTime} ({timeDiffFormatted})");
            }
        }
    }
}

