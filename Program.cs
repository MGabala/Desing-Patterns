#region Singleton
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
#endregion

#region FactoryMethod
//using Factory_Method;
//Console.Title = "Factory Method";
//var factories = new List<DiscountFactory>
//{
//    new CountryDiscountFactory("PL"),
//    new CodeDiscountFactory(Guid.NewGuid())
//};

//foreach(var factory in factories)
//{
//    var discountService = factory.CreateDiscountService();
//    Console.WriteLine($"Percentage {discountService.DiscountPercentage} " + $"comming from {discountService}");
//}
#endregion

#region AbstractFactory
//using Abstract;

//Console.Title = "Abstract Factory";

//var polandPurchaseFactory = new PolandPurchaseFactory();
//var polandShoppingCart = new ShoppingCart(polandPurchaseFactory);
//polandShoppingCart.CalculateCosts();

//var chinesePurchaseFactory = new ChinesePurchaseFactory();
//var chineseShoppingCart = new ShoppingCart(chinesePurchaseFactory);
//chineseShoppingCart.CalculateCosts();
#endregion

#region Builder
//using Builder;

//Console.Title = "Builder";

//var garage = new Garage();

//var fordBuilder = new FORDBuilder();
//var bmwBuilder = new BMWBuilder();

//garage.Construct(fordBuilder);
//Console.WriteLine(fordBuilder.Car.ToString());
////or:
//garage.Show();

//garage.Construct(bmwBuilder);
//Console.WriteLine(bmwBuilder.Car.ToString());
////or:
//garage.Show();
#endregion

#region Prototype
//using Prototype;

//Console.Title = "Prototype";

//var manager = new Manager("Modern-IT");
//var managerClone = (Manager)manager.Clone(true);
//Console.WriteLine($"Manager was cloned: {managerClone.Name}");

//var employee = new Employee("Adam", manager);
//var employeeClone = (Employee)employee.Clone();

////change the manager name - Deep Clone test
//managerClone.Name = "Modern-IT-Clone";
//Console.WriteLine($"Employee was cloned: {employeeClone.Name} " + $"with manager: {employeeClone.Manager.Name}");
#endregion

//#region Adapter

////using ObjectAdapter;
//using ClassAdapter;

//Console.Title = "Object Adapter";

////object adapter example
//ICityAdapter objectAdapter = new CityAdapter();
//var objectAdapterCity = objectAdapter.GetCity();
//Console.WriteLine($"{objectAdapterCity.FullName}, {objectAdapterCity.Inhabitants}");



//Console.Title = "Class Adapter";

////object adapter example
//ICityAdapter classAdapter = new CityAdapter();
//var classCity = classAdapter.GetCity();
//Console.WriteLine($"{classCity.FullName}, {classCity.Inhabitants}");

//#endregion

//#region Bridge
//using Bridge;
//Console.Title = "Bridge";
//var noCoupon = new NoCoupon();
//var oneEuroCoupon = new OneDolarCoupon();
//var twoEuroCoupon = new TwoDolarCoupon();

//var meatBasedMenu = new MeatBasedMenu(noCoupon);
//Console.WriteLine($"Price is: {meatBasedMenu.CalculatePrice()}");

//var vegetarianMenu = new VegetarianMenu(oneEuroCoupon);
//Console.WriteLine($"Price is: {vegetarianMenu.CalculatePrice()}");
//#endregion

//#region Decorator
//using Decorator;
//Console.Title = "Decorator";

//var serverMonitor = new ServerAvalibility();
//serverMonitor.IsActive("True");
//var databaseMonitor = new DatabaseAvalibility();
//databaseMonitor.IsActive("False");

////add behavior
//var statisticsDecorator = new StatisticsDecorator(serverMonitor);
//statisticsDecorator.IsActive($"Status via {nameof(StatisticsDecorator)} wrapper.");
//#endregion

#region Composite
using Composite;
Console.Title = "Composite";
var root = new Composite.FileSystemItem.Directory("root", 0);
var topLevelFile = new Composite.FileSystemItem.File("toplevel.txt", 100);
var topLevelDirectory = new Composite.FileSystemItem.Directory("topLevelDirectory", 4);

root.Add(topLevelFile);
root.Add(topLevelDirectory);

var subLevelFile1 = new Composite.FileSystemItem.File("subLevel1.txt", 200);
var subLevelFile2 = new Composite.FileSystemItem.File("subLevel2.txt", 150);

topLevelDirectory.Add(subLevelFile1);
topLevelDirectory.Add(subLevelFile2);

Console.WriteLine($"Total size of root directory is: {root.GetSize()}");
Console.WriteLine($"Size of topLevelDirectory is: {topLevelDirectory.GetSize()}");
#endregion