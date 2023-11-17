using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;

namespace ConsoleApp;

public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) 
        => new List<Delivery>(deliveries.Where(d => d.PaymentId != null)); //TODO: Завдання 1

    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
    public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries)
        => new List<Delivery>(deliveries.Where(delivery => delivery.Status != DeliveryStatus.Done 
                                                           && delivery.Status != DeliveryStatus.Cancelled)); //TODO: Завдання 2
    
    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    public IEnumerable<DeliveryShortInfo> DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) 
        => new List<DeliveryShortInfo>(
                deliveries
                    .Where(d => d.ClientId == clientId)
                    .Select(x => new DeliveryShortInfo() 
                    {
                        Id = x.Id,
                        StartCity = x.Direction.Origin.City,
                        EndCity = x.Direction.Destination.City,
                        ClientId = x.ClientId,
                        Type = x.Type,
                        LoadingPeriod = x.LoadingPeriod,
                        ArrivalPeriod = x.ArrivalPeriod,
                        Status = x.Status,
                        CargoType = x.CargoType
                    })); //TODO: Завдання 3
    
    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have specified type
    /// </summary>
    public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery> deliveries, string cityName, DeliveryType type) 
        => new List<Delivery>(
            deliveries
                .Where(d => d.Direction.Origin.City == cityName 
                            && d.Type == type)
                .Take(10));//TODO: Завдання 4
    
    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    public IEnumerable<Delivery> OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) 
        => new List<Delivery>(
            deliveries
                .OrderBy(d => d.Status)
                .ThenBy(d => d.LoadingPeriod.Start));//TODO: Завдання 5

    /// <summary>
    /// Count unique cargo types
    /// </summary>
    public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) => 
        deliveries
            .DistinctBy(d => d.CargoType)
            .Count(); //TODO: Завдання 6

    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    public Dictionary<DeliveryStatus, int> CountsByDeliveryStatus(IEnumerable<Delivery> deliveries)
        => deliveries
            .GroupBy(d => d.Status)
            .ToDictionary(g => g.Key, g => g.Count());//TODO: Завдання 7
    
    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap between end of loading period and start of arrival period (calculate in minutes)
    /// </summary>
    public IEnumerable<AverageGapsInfo> AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries) 
        => new List<AverageGapsInfo>(deliveries.GroupBy(d => d.Direction).Select(group => new AverageGapsInfo()
        {
            AverageGap = group.Average(d => (d.ArrivalPeriod.Start.Value - d.LoadingPeriod.End.Value).Minutes),
            StartCity = group.Key.Origin.City,
            EndCity = group.Key.Destination.City
            
        }));//TODO: Завдання 8

    /// <summary>
    /// Paging helper
    /// </summary>
    public IEnumerable<TElement> Paging<TElement, TOrderingKey>(IEnumerable<TElement> elements,
        Func<TElement, TOrderingKey> ordering,
        Func<TElement, bool>? filter = null,
        int countOnPage = 100,
        int pageNumber = 1) => new List<TElement>(
        elements.Where(filter ?? (e => true)).OrderBy(ordering).Skip((pageNumber - 1) * countOnPage).Take(countOnPage)
        ); //TODO: Завдання 9 
}