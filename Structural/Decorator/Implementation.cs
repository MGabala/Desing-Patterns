//Decorator - The intent of this pattern is to attach additional responsibilities to an object dynamically. A decorator thus provides a flexible alternative to subclassing for extending functionality.
//Use cases: attach additional responsibilities to an object dynamically.

namespace Decorator;

//Real life example of Decorator 

/// <summary>
/// Components
/// </summary>
public interface IAvalibilityService
{
    bool IsActive(string status);
}

public class ServerAvalibility : IAvalibilityService
{
    public bool IsActive(string status)
    {
        Console.WriteLine($"Server \"{status}\" " + $"sent via {nameof(ServerAvalibility)}");
        return true;
    }
}

public class DatabaseAvalibility : IAvalibilityService
{
    public bool IsActive(string status)
    {
        Console.WriteLine($"Database \"{status}\" " + $"sent via {nameof(DatabaseAvalibility)}");
        return true;
    }
}

/// <summary>
/// Decorator
/// </summary>
public abstract class AvalibilityServiceDecoratorBase : IAvalibilityService
{
    private readonly IAvalibilityService _avalibilityService;
    public AvalibilityServiceDecoratorBase(IAvalibilityService avalibilityService)
    {
        _avalibilityService = avalibilityService ?? throw new ArgumentNullException(nameof(avalibilityService));
    }
    public virtual bool IsActive(string status)
    {
        return _avalibilityService.IsActive(status);
    }
}

public class StatisticsDecorator : AvalibilityServiceDecoratorBase
{
    public StatisticsDecorator(IAvalibilityService avalibilityService) : base(avalibilityService) 
    {
    }

    public override bool IsActive(string status)
    {
        //Fake statistics
        Console.WriteLine($"Collecting statistics in {nameof(StatisticsDecorator)}");
        return base.IsActive(status);
    }
}