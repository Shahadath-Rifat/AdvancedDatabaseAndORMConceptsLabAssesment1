

using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication2.Data;
using Microsoft.AspNetCore.Http.Json;
using WebApplication2.Models;



var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("LaptopStoreContextConnection");

builder.Services.AddDbContext<LaptopStoreContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

 using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    await SeedData.Initialize(services);
}


// Add Endpoints


// Endpoints for showing all laptops in the database
app.MapGet("/laptops/search", (LaptopStoreContext db) =>
{
    try
    {
        var laptops = db.laptops.Include(l => l.Brand).ToList();
        return Results.Ok(laptops);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});



// Endpoints for getting the average price of all laptops among a specific brand
app.MapGet("/brands/{brandName}/averagePrice", (LaptopStoreContext db, string brandName) =>
{
    try
    {
        Brand brand = db.brands.FirstOrDefault(b => b.Name.ToUpper() == brandName.ToUpper());

        if (brand == null)
        {
            return Results.NotFound($"Brand '{brandName}' not found.");
        }

        List<Laptop> laptopsByBrand = db.laptops.Where(l => l.Brand == brand).ToList();

        int laptopCount = laptopsByBrand.Count();
        decimal averagePrice = laptopCount > 0 ? laptopsByBrand.Average(l => l.Price) : 0;

        var result = new
        {
            LaptopCount = laptopCount,
            AveragePrice = averagePrice
        };

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});




// Returns Store by Province
app.MapGet("/stores/groupByProvince", (LaptopStoreContext db) =>
{
    try
    {
        var storesByProvince = db.locations  // https://stackoverflow.com/a/56010990/21047723 
            .GroupBy(s => s.Province)
            .Where(group => group.Any()) // Only include groups with stores
            .Select(group => new
            {
                Province = group.Key.ToString(), // Get the province from the group
                Stores = group.Select(s => new
                {
                    StoreID = s.StoreID,
                    Address = s.Address
                })
                .ToList()
            })
            .ToList();

        return Results.Ok(storesByProvince);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});


// Endpoint for showing all the laptops available in a store
app.MapGet("/stores/{storeID}/inventory", (LaptopStoreContext db, Guid storeID) =>
{
    try
    {
        List<Laptop> laptopsInStore = db.LaptopStores
            .Where(ls => ls.StoreID == storeID && ls.Quantity > 0)
            .Include(ls => ls.Laptop)
            .Select(ls => ls.Laptop)
            .ToList();

        Console.WriteLine($"Found {laptopsInStore.Count} laptops in store {storeID}");

        if (laptopsInStore.Count == 0)
        {
            return Results.Ok("No laptops available for now in this store.");
        }

        return Results.Ok(laptopsInStore);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});



// Endpoint for Posting new quantity for Laptop
app.MapPost("/stores/{storeID}/{laptopID}/changeQuantity", (LaptopStoreContext db, Guid storeID, Guid laptopID, int newQuantity) =>
{
    try
    {
        var laptopStore = db.LaptopStores
            .SingleOrDefault(ls => ls.StoreID == storeID && ls.LaptopID == laptopID);

        if (laptopStore == null)
        {
            return Results.NotFound($"Laptop store with StoreID: {storeID} and LaptopID: {laptopID} not found.");
        }

        laptopStore.Quantity = newQuantity;
        db.SaveChanges();

        return Results.Ok($"Quantity for LaptopID: {laptopID} at StoreID: {storeID} updated to {newQuantity}");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();
