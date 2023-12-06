public class Relay
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Swimmer> Swimmers { get; set; }

    public Relay(int id, string name) {
        Id = id;
        Name = name;
        Swimmers = new List<Swimmer>();
    }

    public void AddSwimmer(Swimmer swimmer)
    {
        if (this.Swimmers.Any(s => s.Id == swimmer.Id))
        {
            Console.WriteLine($"Swimmer with ID {swimmer.Id} is already on the relay team.");
            return;
        }

        if (this.Swimmers.Count >= 4)
        {
            Console.WriteLine("Relay team is already full.");
            return;
        }

        this.Swimmers.Add(swimmer);
        Console.WriteLine($"Swimmer with ID {swimmer.Id} has been added to the relay team.");
    }

    public void RemoveSwimmer(Swimmer swimmer)
    {
        if (this.Swimmers.Contains(swimmer))
        {
            this.Swimmers.Remove(swimmer);
            Console.WriteLine($"Swimmer {swimmer.Name} (ID: {swimmer.Id}) has been removed from the relay team.");
        }
        else
        {
            Console.WriteLine($"Swimmer {swimmer.Name} (ID: {swimmer.Id}) is not on the relay team.");
        }
    }

    public void ListSwimmers()
    {
        if (Swimmers.Count == 0)
        {
            Console.WriteLine("There are no swimmers in this relay.");
        }
        foreach (var swimmer in this.Swimmers)
        {
            Console.WriteLine($"Swimmer ID: {swimmer.Id}, Name: {swimmer.Name}");
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Relay relay && this.Id == relay.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

