//Prototype - The intent of this pattern is to specify the kinds of objects to create using a prototypical instance, and create new objects by copying this prototype.

// Use cases: When a system should be independent of how its objects are created.


using Newtonsoft.Json;

namespace Prototype;

//Real life example of Prototype 


/// <summary>
/// Prototype
/// </summary>
public abstract class Person
{
	public abstract string Name { get; set; }
	public abstract Person Clone(bool deepClone);
}


/// <summary>
/// Concrete prototype1
/// </summary>
public class Manager : Person
{
	public override string Name{ get; set; }

	public Manager(string name)
	{
		Name = name;
	}

	public override Person Clone(bool deepClone = false)
	{
		if (deepClone)
		{
			var serializedManager = JsonConvert.SerializeObject(this);
			return JsonConvert.DeserializeObject<Manager>(serializedManager);
		}
		return (Person)MemberwiseClone();
	}
}

public class Employee : Person
{
	public Manager Manager { get; set; }
	public override string Name { get ; set ; }
	public Employee(string name, Manager manager)
	{
		Name = name;
		Manager = manager;
	}

	public override Person Clone(bool deepClone = false)
	{
		if (deepClone)
		{
			var serializedEmployee = JsonConvert.SerializeObject(this);
			return JsonConvert.DeserializeObject<Employee>(serializedEmployee);
		}
		return (Person)MemberwiseClone();
	}
}