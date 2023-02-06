//#region Singleton
//using Singleton;

//Console.Title = "Singleton";
//var instance1 = Logger.Instance;
//var instance2 = Logger.Instance;

//if (instance1 == instance2 && instance2 == Logger.Instance)
//{
//    Console.WriteLine("Instances are the same");
//}
//instance1.Log("This is message from instance1");
//instance2.Log("This is message from instance2");
//Logger.Instance.Log("This is message from Logger.Instance");
//#endregion

#region FactoryMethod
using Factory_Method;

var factories = new List<DiscountFactory>
{
    new CountryDiscountFactory("PL"),
    new CodeDiscountFactory(Guid.NewGuid())
};

foreach(var factory in factories)
{
    var discountService = factory.CreateDiscountService();
    Console.WriteLine($"Percentage {discountService.DiscountPercentage} " + $"comming from {discountService}");
}

#endregion