using CarritoAPI.Data;
using CarritoAPI.Domain;
using CarritoAPI.Repositories;
using CarritoAPI.Repositories.Interfaces;
using CarritoAPI.Services;
using CarritoAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseInMemoryDatabase("CartDB"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CartDbContext>();

    if (!context.Users.Any() && !context.Products.Any())
    {
        var users = new List<User>
        {
            new User { Dni = "12345678", Name = "Juan Pérez", IsVip = false },
            new User { Dni = "87654321", Name = "Ana Gómez", IsVip = true },
            new User { Dni = "34567890", Name = "Luis Torres", IsVip = false }
        };
        context.Users.AddRange(users);

        var products = new List<Product>
        {
            new Product { Name = "Producto A", Price = 100.50m },
            new Product { Name = "Producto B", Price = 200.75m },
            new Product { Name = "Producto C", Price = 50.00m },
            new Product { Name = "Producto D", Price = 300.00m },
            new Product { Name = "Producto E", Price = 500.00m },
            new Product { Name = "Producto F", Price = 400.00m }
        };
        context.Products.AddRange(products);

        var carts = new List<Cart>
        {
            new Cart
            {
                UserId = users[0].Id, // Juan Pérez
                Type = "Common",
                Items = new List<ItemCart>
                {
                    new ItemCart { ProductId = 1, Quantity = 2 },
                    new ItemCart { ProductId = 3, Quantity = 1 } 
                }
            },
            new Cart
            {
                UserId = users[1].Id, // Ana Gómez
                Type = "VIP",
                Items = new List<ItemCart>
                {
                    new ItemCart { ProductId = 5, Quantity = 1 }, // Producto E
                    new ItemCart { ProductId = 6, Quantity = 2 }  // Producto F
                }
            }
        };
        context.Carts.AddRange(carts);

        // Guardar Cambios
        await context.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
