using System.Reactive.Linq;
using GenOne.Geolocation;
using System.Reactive.Subjects;
using GenOne.Blazor.Map;

namespace BlazorHybrid;

public class UserLocationProvider : IUserLocationProvider
{
    private readonly IObservable<GpsData> _observable;

    public UserLocationProvider()
    {
        var subject = new BehaviorSubject<GpsData>(Data(-18.955815, -18.879892));

        Observable.Interval(TimeSpan.FromSeconds(5))
            .Select(_ => Data(RandomInRange(-18.955815, -18.879892), RandomInRange(47.489333, 47.583593)))
            .Subscribe(subject);

        _observable = subject;
    }

    private static GpsData Data(double latitude, double longitude)
    {
        return new GpsData(new GpsLocation(latitude, longitude), 1, DateTimeOffset.Now, 0, 0, 0, 0, 0);
    }

    public double RandomInRange(double minNumber, double maxNumber) 
    {
        return Random.Shared.NextDouble() * (maxNumber - minNumber) + minNumber;
    }

    public IDisposable Subscribe(IObserver<GpsData> observer)
    {
        return _observable.Subscribe(observer);
    }
}