//Factory Method - The intent of the factory method pattern is to define an interface for creating an object,
//but to let subclasses decide which class to instantiate.Factory method lets a class defer instantiation to subclasses.

//Use cases:  enable reusing of existing objects

namespace Factory_Method;

//Real life example of Factory Method 

public abstract class DiscountService
{
    public abstract int DiscountPercentage { get; }
    public override string ToString() => GetType().Name;
}

public class CountryDiscountService : DiscountService
{
    private readonly string _countryIdentifier;
    public CountryDiscountService(string countryIdentifier)
    {
        _countryIdentifier = countryIdentifier;
    }
    public override int DiscountPercentage
    {
        get
        {
            switch (_countryIdentifier)
            {
                case "PL":
                    return 20;
                default:
                    return 10; 
            }
        }
    }
}

public class CodeDiscountService : DiscountService
{
    private readonly Guid _code;
    public CodeDiscountService(Guid code)
    {
        _code = code;
    }
    public override int DiscountPercentage
    {
        //implement here validation to check if code been used before
        get => 15;
    }
}

public abstract class DiscountFactory
{
    public abstract DiscountService CreateDiscountService();
}

public class CountryDiscountFactory : DiscountFactory
{
    private readonly string _countryIdentifier;
    public CountryDiscountFactory(string countryIdentifier)
    {
        _countryIdentifier = countryIdentifier;
    }
    public override DiscountService CreateDiscountService()
    {
        return new CountryDiscountService(_countryIdentifier);
    }
}

public class CodeDiscountFactory : DiscountFactory
{
    private readonly Guid _code;
    public CodeDiscountFactory(Guid code)
    {
        _code = code;
    }
    public override DiscountService CreateDiscountService()
    {
        return new CodeDiscountService(_code);
    }
}