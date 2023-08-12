
namespace WebApplication2.Models
{
    public class LaptopStore
	{
		 public Guid ID { get; set; }

         public Guid StoreID { get; set; }

         public StoreAddress StoreAddress { get; set; }

         public Guid LaptopID { get; set; }

         public Laptop Laptop { get; set; }

         public int Quantity { get; set; }   
    }
}
