//Facade - The intent of this pattern is to provide a unified interface to a set of interfaces in a subsystem. It defines a higher-level interface that makes the subsystem easier to use.
//Use cases: Providing a simple interface into a complex subsystem.

namespace Facade;

//Real life example

/// <summary>
/// Subsystem class
/// </summary>
public class OrderService
{
    public bool HasEnoughOrders(int customerId)
    {
        //Fake calculation
        return (customerId > 5);
    }
}
/// <summary>
/// Subsystem class
/// </summary>
public class CustomerDiscountBaseService
{
    public double CalculateDiscountBase(int customerId)
    {
        //Fake calculations
        return (customerId > 10) ? 12 : 20;
    }
}
/// <summary>
/// Subsystem class
/// </summary>
public class DayOfTheWeekFactorService
{
    public double CalculateDayOfTheWeekFactor()
    {
        //Fake calculations
        switch (DateTime.UtcNow.DayOfWeek)
        {
            case DayOfWeek.Saturday:
            case DayOfWeek.Sunday:
                return 0.8;
            default:
                return 1.2;
        }
    }
}
/// <summary>
/// Facade
/// </summary>
public class DiscountFacade
{
    private readonly OrderService _orderService = new();
    private readonly CustomerDiscountBaseService _customerDiscountBaseService = new();
    private readonly DayOfTheWeekFactorService _dayOfTheWeekFactoryService = new();

    public double CalculateDiscountPercentage(int customerId)
    {
        if (!_orderService.HasEnoughOrders(customerId))
        {
            return 0;
        }
        return _customerDiscountBaseService.CalculateDiscountBase(customerId) * _dayOfTheWeekFactoryService.CalculateDayOfTheWeekFactor();
    }
}