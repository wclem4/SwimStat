public partial class Swimmer : Person
{
    public List<SwimResult> SwimResults { get; set; }
    public Swimmer(int id, string name) : base(id, name)
    {
        SwimResults = new List<SwimResult>();
    }

    public override bool Equals(object? obj)
    {
        return obj is Swimmer swimmer && this.Id == swimmer.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
