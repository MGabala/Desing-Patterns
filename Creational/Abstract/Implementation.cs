//Abstract Factory - Similar as Factory method but depends on Interface families
//  Use cases: when a system should be independent of how its products are created, composed and represented.

namespace Abstract;

//Real life example of Abstract Factory

/// <summary>
/// AbstractFactory
/// </summary>
public interface IShoppingCartPurchaseFactory
{
    IDiscountService CreateDiscountService();
    IShippingCostsService CreateShippingCostsService();
}

/// <summary>
/// AbstractProduct
/// </summary>
public interface IDiscountService
{
    int DiscountPercentage { get; }
}

/// <summary>
/// Concrete Product
/// </summary>
public class PolandDiscountService : IDiscountService
{
    public int DiscountPercentage => 25;
}

/// <summary>
/// Concrete Product
/// </summary>
public class ChineseDiscountService : IDiscountService
{
    public int DiscountPercentage => 10;
}

/// <summary>
/// AbstractProduct
/// </summary>
public interface IShippingCostsService
{
    decimal ShippingCosts { get; }
}

/// <summary>
/// Concrete Product
/// </summary>
public class PolandShippingCostsService : IShippingCostsService
{
    public decimal ShippingCosts => 20;
}

/// <summary>
/// Concrete Product
/// </summary>
public class ChineseShippingCostsService : IShippingCostsService
{
    public decimal ShippingCosts => 45;
}

/// <summary>
/// Concrete Factory
/// </summary>
public class PolandPurchaseFactory : IShoppingCartPurchaseFactory
{
    public IDiscountService CreateDiscountService()
    {
        return new PolandDiscountService();
    }

    public IShippingCostsService CreateShippingCostsService()
    {
        return new PolandShippingCostsService();
    }
}

/// <summary>
/// Concrete Factory
/// </summary>
public class ChinesePurchaseFactory : IShoppingCartPurchaseFactory
{
    public IDiscountService CreateDiscountService()
    {
        return new ChineseDiscountService();    
    }

    public IShippingCostsService CreateShippingCostsService()
    {
        return new ChineseShippingCostsService();
    }
}

/// <summary>
/// Client class
/// </summary>
public class ShoppingCart
{
    private readonly IDiscountService _discountService;
    private readonly IShippingCostsService _shippingCostsService;
    private int _orderCosts;

    //Constructor
    public ShoppingCart(IShoppingCartPurchaseFactory factory)
    {
        _discountService = factory.CreateDiscountService();
        _shippingCostsService = factory.CreateShippingCostsService();
        _orderCosts = 200;
    } 
    public void CalculateCosts()
    {
        Console.WriteLine($"Total costs = " + $"{_orderCosts - (_orderCosts/100*_discountService.DiscountPercentage) + _shippingCostsService.ShippingCosts }");
    }
}