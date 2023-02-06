using Singleton;

var instance1 = Logger.Instance;
var instance2 = Logger.Instance;

if (instance1 == instance2 && instance2 == Logger.Instance)
{
    Console.WriteLine("Instances are the same");
}
instance1.Log("This is message from instance1");
instance2.Log("This is message from instance2");
Logger.Instance.Log("This is message from Logger.Instance");