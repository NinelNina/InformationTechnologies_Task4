namespace Task4.Models;

public class ChildPassenger : IPassenger
{
    public string Name { get; private set; }

    public ChildPassenger(string name)
    {
        Name = name;
    }
}
