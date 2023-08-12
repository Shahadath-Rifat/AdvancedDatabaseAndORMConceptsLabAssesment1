using System;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            LaptopStoreContext db = new LaptopStoreContext(serviceProvider.GetRequiredService<DbContextOptions<LaptopStoreContext>>());

            db.Database.EnsureDeleted();  // delete the existing database ..if any
            db.Database.Migrate();



            // Brands 
            Brand brandOne = new Brand { Name = "MSI" };
            Brand brandTwo = new Brand { Name = "Apple" };
            Brand brandThree = new Brand { Name = "Asus" };

            if (!db.brands.Any()) 
            {
                db.brands.Add(brandOne); 
                db.brands.Add(brandTwo);
                db.brands.Add(brandThree);
                db.SaveChanges();
            }



            // Laptops
            Laptop laptopOne = new Laptop { Model = "GT76 Titan", Price = 1399, Condition = LaptopCondition.New, Brand = brandOne };
            Laptop laptopTwo = new Laptop { Model = "TUF", Price = 1099, Condition = LaptopCondition.Refurbished, Brand = brandThree };
            Laptop laptopThree = new Laptop { Model = "MacBook Pro", Price = 1150, Condition = LaptopCondition.Rental, Brand = brandTwo };
            Laptop laptopFour = new Laptop { Model = "MacBook Air", Price = 999, Condition = LaptopCondition.Rental, Brand = brandTwo };
            Laptop laptopFive = new Laptop { Model = "GP75 Leopard", Price = 1699, Condition = LaptopCondition.Refurbished, Brand = brandOne };

            
            if (!db.laptops.Any())
            {
                db.laptops.Add(laptopOne);
                db.laptops.Add(laptopTwo);
                db.laptops.Add(laptopThree);
                db.laptops.Add(laptopFour);
                db.laptops.Add(laptopFive);
                db.SaveChanges();
            }




            // Store Address
            StoreAddress storeOne = new StoreAddress { Address = "111 Main St.", Province = Province.Yukon };
            StoreAddress storeTwo = new StoreAddress { Address = "222 Elm St.", Province = Province.Manitoba };
            StoreAddress storeThree = new StoreAddress { Address = "333 Broadway Ave.", Province = Province.NovaScotia };


            if (!db.locations.Any())
            {
                db.locations.Add(storeOne);
                db.locations.Add(storeTwo);
                db.locations.Add(storeThree);
                db.SaveChanges();
            }




            // Laptop Store
            LaptopStore LaptopStoreOne = new LaptopStore { StoreAddress = storeOne, Laptop = laptopOne, Quantity = 10 }; 
            LaptopStore LaptopStoreTwo = new LaptopStore { StoreAddress = storeOne, Laptop = laptopFour, Quantity = 13 }; 
            LaptopStore LaptopStoreThree = new LaptopStore { StoreAddress = storeTwo, Laptop = laptopTwo, Quantity = 7 }; 
            LaptopStore LaptopStoreFour = new LaptopStore { StoreAddress = storeTwo, Laptop = laptopThree, Quantity = 2 };
            LaptopStore LaptopStoreFive = new LaptopStore { StoreAddress = storeThree, Laptop = laptopThree, Quantity = -10 }; 
            LaptopStore LaptopStoreSix = new LaptopStore { StoreAddress = storeThree, Laptop = laptopFive, Quantity = 22 };

            
            if (!db.LaptopStores.Any())
            {
                db.LaptopStores.Add(LaptopStoreOne);
                db.LaptopStores.Add(LaptopStoreTwo);
                db.LaptopStores.Add(LaptopStoreThree);
                db.LaptopStores.Add(LaptopStoreFour);
                db.LaptopStores.Add(LaptopStoreFive);
                db.LaptopStores.Add(LaptopStoreSix);
                db.SaveChanges();
            }
        }
    }
}

