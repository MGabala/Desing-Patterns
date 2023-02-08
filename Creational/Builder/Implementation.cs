//!Builder - The intent of the builder pattern is to separate the construction of a complex object from its representation. By doing so, the same construction process can create different representation.

//  Use cases:  When you want to make the algorithm for creating a complex objects.

using System.Text;

namespace Builder;

//Real life example of Builder

/// <summary>
/// Product
/// </summary>
public class Car 
{
    private readonly List<string> _parts = new();
    private readonly string _carType;
    public Car(string carType)
    {
        _carType = carType;
    }

    public void AddPart(string part)
    {
        _parts.Add(part);
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach(var part in _parts)
        {
            stringBuilder.Append($"Car of type {_carType} has part {part}.");
        }
        return stringBuilder.ToString();
    }
}

/// <summary>
/// Builder
/// </summary>
public abstract class CarBuilder
{
    public Car Car { get; set; }
    public CarBuilder(string carType)
    {
        Car = new Car(carType);
    }
    public abstract void BuildEngine();
    public abstract void BuildFrame();
}

/// <summary>
/// Concrete builder class
/// </summary>
public class FORDBuilder : CarBuilder
{
    public FORDBuilder() : base("Ford")
    {

    }

    public override void BuildEngine()
    {
        Car.AddPart("V8");
    }

    public override void BuildFrame()
    {
        Car.AddPart("5-door");
    }
}

/// <summary>
/// Concrete builder class
/// </summary>
public class BMWBuilder : CarBuilder
{
    public BMWBuilder() : base("BMW")
    {

    }

    public override void BuildEngine()
    {
        Car.AddPart("V4");
    }

    public override void BuildFrame()
    {
        Car.AddPart("3-door");
    }
}


public class Garage
{
    private CarBuilder? _builder;

    public Garage()
    {

    }

    public void Construct(CarBuilder builder)
    {
        _builder = builder;
        _builder.BuildEngine();
        _builder.BuildFrame();
    }
     
    public void Show()
    {
        Console.WriteLine(_builder?.Car.ToString());
    }
}