﻿#region Singleton
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

#region Adapter

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

//#region Composite
//using Composite;
//Console.Title = "Composite";
//var root = new Composite.FileSystemItem.Directory("root", 0);
//var topLevelFile = new Composite.FileSystemItem.File("toplevel.txt", 100);
//var topLevelDirectory = new Composite.FileSystemItem.Directory("topLevelDirectory", 4);

//root.Add(topLevelFile);
//root.Add(topLevelDirectory);

//var subLevelFile1 = new Composite.FileSystemItem.File("subLevel1.txt", 200);
//var subLevelFile2 = new Composite.FileSystemItem.File("subLevel2.txt", 150);

//topLevelDirectory.Add(subLevelFile1);
//topLevelDirectory.Add(subLevelFile2);

//Console.WriteLine($"Total size of root directory is: {root.GetSize()}");
//Console.WriteLine($"Size of topLevelDirectory is: {topLevelDirectory.GetSize()}");
//#endregion

//#region Facade
//using Facade;
//Console.Title = "Facade";
//var facade = new DiscountFacade();

//Console.WriteLine($"Discount percentage for customer with id 1: {facade.CalculateDiscountPercentage(1)}");
//Console.WriteLine($"Discount percentage for customer with id 10: {facade.CalculateDiscountPercentage(10 )}");
#endregion

#region Proxy
//using Proxy;
//Console.Title = "Proxy";

//// without proxy
//Console.WriteLine("Constructing document.");
//var myDocument = new Proxy.Document("MyDocument.pdf");
//Console.WriteLine("Document constructed.");
//myDocument.DisplayDocument();

//Console.WriteLine();

//// with proxy 
//Console.WriteLine("Constructing document proxy.");
//var myDocumentProxy = new Proxy.DocumentProxy("MyDocument.pdf");
//Console.WriteLine("Document proxy constructed.");
//myDocumentProxy.DisplayDocument();

//Console.WriteLine();

//// with chained proxy
//Console.WriteLine("Constructing protected document proxy.");
//var myProtectedDocumentProxy = new Proxy.ProtectedDocumentProxy("MyDocument.pdf", "Viewer");
//Console.WriteLine("Protected document proxy constructed.");
//myProtectedDocumentProxy.DisplayDocument();

//Console.WriteLine();

//// with chained proxy, no access
//Console.WriteLine("Constructing protected document proxy.");
//myProtectedDocumentProxy = new Proxy.ProtectedDocumentProxy("MyDocument.pdf", "AnotherRole");
//Console.WriteLine("Protected document proxy constructed.");
//myProtectedDocumentProxy.DisplayDocument();
#endregion

#region Flyweight
//using Flyweight;
//Console.Title = "Flyweight";

//var aBunchOfCharacters = "abba";

//var characterFactory = new CharacterFactory();

//// Get the flyweight(s)
//var characterObject = characterFactory.GetCharacter(aBunchOfCharacters[0]);
//// Pass through extrinsic state
//characterObject?.Draw("Arial", 12);

//characterObject = characterFactory.GetCharacter(aBunchOfCharacters[1]);
//characterObject?.Draw("Trebuchet MS", 14);

//characterObject = characterFactory.GetCharacter(aBunchOfCharacters[2]);
//characterObject?.Draw("Times New Roman", 16);

//characterObject = characterFactory.GetCharacter(aBunchOfCharacters[3]);
//characterObject?.Draw("Comic Sans", 18);

//// create unshared concrete flyweight (paragraph)
//var paragraph = characterFactory.CreateParagraph(
//    new List<ICharacter>() { characterObject }, 1);

//// draw the paragraph
//paragraph.Draw("Lucinda", 12);
#endregion

#region TemplateMethod
//using Template_Method;

//Console.Title = "Template Method";

//ExchangeMailParser exchangeMailParser = new();
//Console.WriteLine(exchangeMailParser.ParseMailBody("bf3a298c-9990-4b02-873d-3d3c98ad16d2"));
//Console.WriteLine();

//ApacheMailParser apacheMailParser = new();
//Console.WriteLine(apacheMailParser.ParseMailBody("07b8a8c7-c598-4b6c-9049-ecce9fe4a56b"));
//Console.WriteLine();

//EudoraMailParser eudoraMailParser = new();
//Console.WriteLine(eudoraMailParser.ParseMailBody("789fe935-ced2-4fbd-865b-172909560a93"));
#endregion

#region Strategy
//using Strategy;

//Console.Title = "Strategy";

//var order = new Order("Marvin Software", 5, "Visual Studio License");
//order.Export(new CSVExportService());
//order.Export(new JsonExportService());
#endregion

#region Command
//using Command;

//Console.Title = "Command";

//CommandManager commandManager = new();
//IEmployeeManagerRepository repository = new EmployeeManagerRepository();

//commandManager.Invoke(
//    new AddEmployeeToManagerList(repository, 1, new Employee(111, "Kevin")));
//repository.WriteDataStore();

//commandManager.Undo();
//repository.WriteDataStore();

//commandManager.Invoke(
//    new AddEmployeeToManagerList(repository, 1, new Employee(222, "Clara")));
//repository.WriteDataStore();

//commandManager.Invoke(
//    new AddEmployeeToManagerList(repository, 2, new Employee(333, "Tom")));
//repository.WriteDataStore();

//// try adding the same employee again
//commandManager.Invoke(
//    new AddEmployeeToManagerList(repository, 2, new Employee(333, "Tom")));
//repository.WriteDataStore();

//commandManager.UndoAll();
//repository.WriteDataStore();
#endregion

#region Memento
//using Memento;

//Console.Title = "Memento";

//CommandManager commandManager = new();
//IEmployeeManagerRepository repository = new EmployeeManagerRepository();

//commandManager.Invoke(
//    new AddEmployeeToManagerList(repository, 1, new Employee(111, "Kevin")));
//repository.WriteDataStore();

//commandManager.Undo();
//repository.WriteDataStore();

//commandManager.Invoke(
//    new AddEmployeeToManagerList(repository, 1, new Employee(222, "Clara")));
//repository.WriteDataStore();

//commandManager.Invoke(
//    new AddEmployeeToManagerList(repository, 2, new Employee(333, "Tom")));
//repository.WriteDataStore();

//// try adding the same employee again
//commandManager.Invoke(
//    new AddEmployeeToManagerList(repository, 2, new Employee(333, "Tom")));
//repository.WriteDataStore();

//commandManager.UndoAll();
//repository.WriteDataStore();
#endregion

#region Mediator
//using Mediator;

//Console.Title = "Mediator";

//TeamChatRoom teamChatroom = new();

//var sven = new Lawyer("Sven");
//var kenneth = new Lawyer("Kenneth");
//var ann = new AccountManager("Ann");
//var john = new AccountManager("John");
//var kylie = new AccountManager("Kylie");

//teamChatroom.Register(sven);
//teamChatroom.Register(kenneth);
//teamChatroom.Register(ann);
//teamChatroom.Register(john);
//teamChatroom.Register(kylie);

//ann.Send("Hi everyone, can someone have a look at file ABC123?  I need a compliance check.");
//sven.Send("On it!");
//sven.Send("Ann", "Could you join me in a Teams call?");
//sven.Send("Ann", "All good :)");
//ann.SendTo<AccountManager>("The file was approved!");
#endregion

#region ChainOfResponsibility
//using Chain_Of_Responsibility;

//using System.ComponentModel.DataAnnotations;

//Console.Title = "Chain of Responsibility";

//var validDocument = new Document("How to Avoid Java Development",
//    DateTimeOffset.UtcNow, true, true);
//var invalidDocument = new Document("How to Avoid Java Development",
//    DateTimeOffset.UtcNow, false, true);

//var documentHandlerChain = new DocumentTitleHandler();
//documentHandlerChain
//    .SetSuccessor(new DocumentLastModifiedHandler())
//    .SetSuccessor(new DocumentApprovedByLitigationHandler())
//    .SetSuccessor(new DocumentApprovedByManagementHandler());

//try
//{
//    documentHandlerChain.Handle(validDocument);
//    Console.WriteLine("Valid document is valid.");
//    documentHandlerChain.Handle(invalidDocument);
//    Console.WriteLine("Invalid document is valid.");
//}
//catch (ValidationException validationException)
//{
//    Console.WriteLine(validationException.Message);
//}
#endregion

#region Observer
//using Observer;

//Console.Title = "Observer";

//TicketStockService ticketStockService = new();
//TicketResellerService ticketResellerService = new();
//OrderService orderService = new();

//// add two observers
//orderService.AddObserver(ticketStockService);
//orderService.AddObserver(ticketResellerService);

//// notify
//orderService.CompleteTicketSale(1, 2);

//Console.WriteLine();

//// remove one observer
//orderService.RemoveObserver(ticketResellerService);

//// notify
//orderService.CompleteTicketSale(2, 4);
#endregion

#region State
//using State;

//Console.Title = "State";

//BankAccount bankAccount = new();
//bankAccount.Deposit(100);
//bankAccount.Withdraw(500);
//bankAccount.Withdraw(100);
//// deposit enough to go to gold
//bankAccount.Deposit(2000);
//// should be in gold now
//bankAccount.Deposit(100);
//// back to overdraw
//bankAccount.Withdraw(3000);
//// should transition to regular
//bankAccount.Deposit(3000);
//// should still be in regular
//bankAccount.Deposit(100);
#endregion

#region Iterator
//using Iterator;
//Console.Title = "Iterator";

//// create the collection
//PeopleCollection people = new();

//people.Add(new Iterator.Person("Mateusz Gabała", "Polska"));
//people.Add(new Iterator.Person("Second Person", "Belgium"));
//people.Add(new Iterator.Person("Third Person", "The Netherlands"));
//people.Add(new Iterator.Person("Fourth Person", "Germany"));

//// create the iterator
//var peopleIterator = people.CreateIterator();

//// use the iterator to run through the people
//// in the collection; they should come out 
//// in alphabetical order
//for (Iterator.Person person = peopleIterator.First();
//    !peopleIterator.IsDone;
//    person = peopleIterator.Next())
//{
//    Console.WriteLine(person?.Name);
//}

//Console.ReadKey();
#endregion

#region Visitor
//using System.ComponentModel;

//using Visitor;

//Console.Title = "Visitor";

//// create container & add concrete elements
//var container = new Visitor.Container();

//container.Customers.Add(new Customer("Sophie", 500));
//container.Customers.Add(new Customer("Karen", 1000));
//container.Customers.Add(new Customer("Sven", 800));
//container.Employees.Add(new Employee("Kevin", 18));
//container.Employees.Add(new Employee("Tom", 5));

//// create visitor
//DiscountVisitor discountVisitor = new();

//// pass it through
//container.Accept(discountVisitor);

//// write out gathered amount
//Console.WriteLine($"Total discount: {discountVisitor.TotalDiscountGiven}");

//Console.ReadKey();
#endregion

#region Interpreter
using Interpreter;

Console.Title = "Interpreter";

var expressions = new List<RomanExpression>
{
    new RomanHunderdExpression(),
    new RomanTenExpression(),
    new RomanOneExpression(),
};

var context = new RomanContext(5);
foreach (var expression in expressions)
{
    expression.Interpret(context);
}
Console.WriteLine($"Translating Arabic numerals to Roman numerals: 5 = {context.Output}");

context = new RomanContext(81);
foreach (var expression in expressions)
{
    expression.Interpret(context);
}
Console.WriteLine($"Translating Arabic numerals to Roman numerals: 81 = {context.Output}");

context = new RomanContext(733);
foreach (var expression in expressions)
{
    expression.Interpret(context);
}
Console.WriteLine($"Translating Arabic numerals to Roman numerals: 733 = {context.Output}");

Console.ReadKey();
#endregion