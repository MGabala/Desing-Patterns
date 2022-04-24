public class Orders
{
    public int Price { get; set; } 
    public string Name { get; set; }

    public Orders(int price, string name)
    {
        Price = price;   
        Name = name;
    }
    List<Orders> orders = new List<Orders>();
    
}
