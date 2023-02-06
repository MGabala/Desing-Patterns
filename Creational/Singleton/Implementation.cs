//Singleton - The intent of the singleton pattern is to ensure that a class only has one instance, and to provide a global point of access to it. 
//Use cases: when there must be exactly one instance of a class

namespace Singleton;


//Real life example of Singleton 
public class Logger
{
    private static readonly Lazy<Logger> _lazyLogger = new Lazy<Logger>(() => new Logger());
    public static Logger Instance
    {
        get
        {
            return _lazyLogger.Value;
        }
    }
    protected Logger() { }

    public void Log(string message)
    {
        Console.WriteLine($"Message to log: {message}");
    }
}