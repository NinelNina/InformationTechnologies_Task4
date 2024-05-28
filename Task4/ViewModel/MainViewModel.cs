using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Task4.Models;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Bus> Buses { get; set; }
    public ObservableCollection<BusStop> BusStops { get; set; }
    private static Random rand = new Random();

    public MainViewModel()
    {
        Buses = new ObservableCollection<Bus>();
        BusStops = new ObservableCollection<BusStop>();

        GenerateRandomBusStops();
        GenerateRandomBuses();

        foreach (var b in Buses)
        {
            b.BusStopped += Bus_BusStopped;
            b.BusFull += Bus_BusFull;
            b.StartMoving();
        }
    }

    private void GenerateRandomBusStops()
    {
        int stopCount = rand.Next(3, 6);
        for (int i = 1; i <= stopCount; i++)
        {
            BusStops.Add(new BusStop(i));
        }
    }

    private void GenerateRandomBuses()
    {
        int busCount = rand.Next(3, 6);
        for (int i = 1; i <= busCount; i++)
        {
            var bus = new Bus(i, rand.Next(10, 21));
            bus.SetBusStops(BusStops.ToList());
            Buses.Add(bus);
        }
    }

    private void Bus_BusStopped(object sender, BusStopEventArgs e)
    {
        var bus = sender as Bus;
        var busStop = e.BusStop;
        if (bus != null && busStop != null)
        {
            OnPropertyChanged(nameof(BusStops));
            OnPropertyChanged(nameof(Buses));
            if (busStop.WaitingPassengers.Any())
            {
                if (bus.FreeSeats > 0)
                {
                    MessageBox.Show($"Bus {bus.BusNumber} stopped at Bus Stop {busStop.StopNumber}. Passengers are boarding.");
                }
                else {
                    MessageBox.Show($"Bus {bus.BusNumber} stopped at Bus Stop {busStop.StopNumber}. Bus is full.");
                }
            }
            else
            {
                MessageBox.Show($"Bus {bus.BusNumber} stopped at Bus Stop {busStop.StopNumber}. No passengers to board.");
            }
        }
    }

    private void Bus_BusFull(object sender, EventArgs e)
    {
        var bus = sender as Bus;
        if (bus != null)
        {
            MessageBox.Show($"Bus {bus.BusNumber} with capacity {bus.Capacity} is full.");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
