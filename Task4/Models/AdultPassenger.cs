
namespace Task4.Models;

public class AdultPassenger : IPassenger
{
    public string Name { get; private set; }

    public AdultPassenger(string name)
    {
        Name = name;
    }
}
