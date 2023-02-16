//Adapter - The intent of this pattern is to convert the interface of a class into another interface clients expect.Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.

//  Use cases: Allows two different classess work together via Interfaces.

namespace ClassAdapter;

//Real life example of Class Adapter

public class CityFromExtertnalSystem
{
	public string Name { get; private set; }
	public string NickName { get; private set; }
	public int Inhabitants { get; private set; }
	public CityFromExtertnalSystem(string name, string nickName, int inhabitants)
	{
		Name = name;
		NickName = nickName;
		Inhabitants = inhabitants;
	}
}

/// <summary>
/// Adaptee
/// </summary>
public class ExternalSystem
{
	public CityFromExtertnalSystem GetCity()
	{
		return new CityFromExtertnalSystem("Los Angeles", "Mefju", 5000);
	}
}

public class City
{
	public string FullName { get; private set; }
	public long Inhabitants { get; private set; }
	public City(string fullName, long inhabitants)
	{
		FullName = fullName;
		Inhabitants = inhabitants;
	}
}

/// <summary>
/// Target
/// </summary>
public interface ICityAdapter
{
	City GetCity();
}

public class CityAdapter : ExternalSystem, ICityAdapter
{

	public City GetCity()
	{
		//call into the external system
		var cityFromExternalSystem = base.GetCity();

		//adapt the CityFromExternalCity to a City
		return new City($"{cityFromExternalSystem.Name} - {cityFromExternalSystem.NickName}", cityFromExternalSystem.Inhabitants);
	}
}
