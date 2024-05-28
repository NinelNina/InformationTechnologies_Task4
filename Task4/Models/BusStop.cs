using System.Collections.ObjectModel;
using System.ComponentModel;
using Task4.Models;

public class BusStop : INotifyPropertyChanged
{
    public int StopNumber { get; private set; }
    private ObservableCollection<IPassenger> waitingPassengers;
    private static Random rand = new Random();

    public ObservableCollection<IPassenger> WaitingPassengers
    {
        get { return waitingPassengers; }
        private set
        {
            waitingPassengers = value;
            OnPropertyChanged(nameof(WaitingPassengers));
        }
    }

    public BusStop(int stopNumber)
    {
        StopNumber = stopNumber;
        WaitingPassengers = new ObservableCollection<IPassenger>();
        AddRandomPassengers();
    }

    public void AddPassenger(IPassenger passenger)
    {
        WaitingPassengers.Add(passenger);
        OnPropertyChanged(nameof(WaitingPassengers));
    }

    public void RemovePassenger(IPassenger passenger)
    {
        WaitingPassengers.Remove(passenger);
        OnPropertyChanged(nameof(WaitingPassengers));
    }

    private void AddRandomPassengers()
    {
        int passengerCount = rand.Next(1, 10);
        for (int i = 0; i < passengerCount; i++)
        {
            AddPassenger(new AdultPassenger($"Passenger {i + 1}"));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
