//Bridge - The intent of this pattern is to decouple an abstraction from its implementation so the two can vary independently.
//Use cases: Used to let abstraction and implementations vary independently.

namespace Bridge;

//Real life example of Bridge

/// <summary>
/// Abstraction
/// </summary>
public abstract class Menu
{
    public readonly ICoupon _coupon = null!;
    public abstract int CalculatePrice();
    public Menu(ICoupon coupon)
    {
        _coupon = coupon ?? throw new ArgumentNullException(nameof(coupon));
    }
}

/// <summary>
/// Implementor
/// </summary>
public interface ICoupon
{
    int CouponValue { get; }
}

/// <summary>
/// Concrete implementations
/// </summary>
public class NoCoupon : ICoupon
{
   public int CouponValue { get => 0; }
}

public class OneDolarCoupon : ICoupon
{
    public int CouponValue { get => 1; }
}

public class TwoDolarCoupon : ICoupon
{
    public int CouponValue { get => 2; }
}

/// <summary>
/// RefinedAbstraction
/// </summary>
public class VegetarianMenu : Menu
{
    public VegetarianMenu(ICoupon coupon) : base(coupon)
    {    
    }
    public override int CalculatePrice()
    {
        return 20 - _coupon.CouponValue;
    }
}

public class MeatBasedMenu : Menu
{
    public MeatBasedMenu(ICoupon coupon) : base(coupon)
    {
    }
    public override int CalculatePrice()
    {
        return 30 - _coupon.CouponValue;
    }
}