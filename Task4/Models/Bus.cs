using System.ComponentModel;
using Task4.Models;

public class Bus : INotifyPropertyChanged
{
    public int BusNumber { get; private set; }
    public int Capacity { get; private set; }
    private List<IPassenger> passengers;
    private List<BusStop> busStops;
    private Random rand;
    private CancellationTokenSource cts;

    private int _freeSeats;
    public int FreeSeats
    {
        get { return _freeSeats; }
        set
        {
            _freeSeats = value;
            OnPropertyChanged(nameof(FreeSeats));
        }
    }

    public Bus(int busNumber, int capacity)
    {
        BusNumber = busNumber;
        Capacity = capacity;
        passengers = new List<IPassenger>();
        rand = new Random();
        FreeSeats = capacity;
        FillRandomPassengers();
    }

    private void FillRandomPassengers()
    {
        int passengerCount = rand.Next(0, Capacity);
        for (int i = 0; i < passengerCount; i++)
        {
            passengers.Add(new AdultPassenger($"Passenger {i + 1}"));
        }
        FreeSeats = Capacity - passengers.Count;
    }

    public void SetBusStops(List<BusStop> stops)
    {
        busStops = stops;
    }

    public event EventHandler<BusStopEventArgs> BusStopped;
    public event EventHandler BusFull;
    public event PropertyChangedEventHandler PropertyChanged;

    public async void StartMoving()
    {
        cts = new CancellationTokenSource();
        await Task.Run(async () =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                int delay = rand.Next(1000, 50000);
                await Task.Delay(delay, cts.Token);
                if (!cts.Token.IsCancellationRequested)
                {
                    Move(cts.Token);
                }
            }
        });
    }

    private void Move(CancellationToken token)
    {
        StopAtRandomBusStop();
    }

    public void StopAtRandomBusStop()
    {
        if (busStops != null && busStops.Count > 0)
        {
            int stopIndex = rand.Next(busStops.Count);
            StopAtBusStop(busStops[stopIndex]);
        }
    }

    public void StopAtBusStop(BusStop stop)
    {
        BusStopped?.Invoke(this, new BusStopEventArgs { BusStop = stop });

        foreach (var passenger in stop.WaitingPassengers.ToList())
        {
            if (passengers.Count < Capacity)
            {
                passengers.Add(passenger);
                stop.RemovePassenger(passenger);
                FreeSeats = Capacity - passengers.Count;
            }
            else
            {
                BusFull?.Invoke(this, EventArgs.Empty);
                break;
            }
        }

        OnPropertyChanged(nameof(FreeSeats));
        stop.OnPropertyChanged(nameof(stop.WaitingPassengers));
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class BusStopEventArgs : EventArgs
{
    public BusStop BusStop { get; set; }
}
