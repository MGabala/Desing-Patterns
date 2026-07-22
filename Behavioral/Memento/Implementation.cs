//Memento - The intent of this pattern is to capture and externalize an object's internal state so that the object can be restored to this state later, without violating encapsulation.
//  Use cases: when you need to save/restore an object's state (e.g. for an undo mechanism) without exposing its internal representation to the code doing the saving.

namespace Memento;

public class Employee
{
    public int Id { get; }
    public string Name { get; }

    public Employee(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

/// <summary>
/// Memento (opaque handle). The Caretaker can hold one of these and pass it
/// back to the Originator, but the interface exposes nothing, so it has no
/// way to inspect or tamper with the state inside.
/// </summary>
public interface IManagerMemento
{
}

/// <summary>
/// Originator
/// </summary>
public class Manager : Employee
{
    private List<Employee> _employees = new();
    public IReadOnlyList<Employee> Employees => _employees;

    public Manager(int id, string name)
        : base(id, name)
    {
    }

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }

    public void RemoveEmployee(Employee employee)
    {
        _employees.Remove(employee);
    }

    /// <summary>
    /// Capture the current state into an opaque memento.
    /// </summary>
    public IManagerMemento CreateMemento()
    {
        // snapshot a copy, so later mutations to _employees can't leak into it
        return new ManagerMemento(new List<Employee>(_employees));
    }

    /// <summary>
    /// Restore state from a memento previously created by this manager.
    /// </summary>
    public void RestoreMemento(IManagerMemento memento)
    {
        if (memento is not ManagerMemento managerMemento)
        {
            throw new ArgumentException(
                $"Memento was not created by this {nameof(Manager)}.", nameof(memento));
        }

        _employees = new List<Employee>(managerMemento.Employees);
    }

    /// <summary>
    /// For demo purposes, write out the manager's employees to the console window
    /// </summary>
    public void WriteEmployees()
    {
        Console.WriteLine($"Manager {Id}, {Name}");
        if (_employees.Any())
        {
            foreach (var employee in _employees)
            {
                Console.WriteLine($"\t Employee {employee.Id}, {employee.Name}");
            }
        }
        else
        {
            Console.WriteLine($"\t No employees.");
        }
    }

    /// <summary>
    /// ConcreteMemento — kept private so nothing outside Manager can construct
    /// one or see past the IManagerMemento marker interface to read its state.
    /// </summary>
    private class ManagerMemento : IManagerMemento
    {
        public List<Employee> Employees { get; }

        public ManagerMemento(List<Employee> employees)
        {
            Employees = employees;
        }
    }
}

/// <summary>
/// Caretaker — tracks mementos for undo, but never looks at what's inside them.
/// </summary>
public class ManagerHistory
{
    private readonly Stack<IManagerMemento> _history = new();

    public void Save(Manager manager)
    {
        _history.Push(manager.CreateMemento());
    }

    public void Undo(Manager manager)
    {
        if (_history.Count == 0)
        {
            return;
        }

        manager.RestoreMemento(_history.Pop());
    }
}
