
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models

{
    public class StoreAddress
    {
        [Key] // This attribute defines the primary key
        public Guid StoreID { get; set; }

		private string _address;

        public string Address 
        {
            get => _address;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The store address must contain a minimum of two characters.\r\n\r\n\r\n\r\n\r\n");
                }
                _address = value;
            }
        }
        public Province Province { get; set; }

        public HashSet<LaptopStore> LaptopStores { get; set; } = new HashSet<LaptopStore> ();
    }

    public enum Province
    {
        Manitoba,
        Ontario,
        Yukon,
        NewBrunswick,
        NewfoundlandAndLabrador,
        BritishColumbia,
        Nunavut,
        Alberta,
        PrinceEdwardIsland,
        NovaScotia,
        Quebec,
        Saskatchewan,
        NorthwestTerritories
    }

}
