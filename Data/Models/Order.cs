namespace Data.Models;

public class Order : BaseModel
{
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}