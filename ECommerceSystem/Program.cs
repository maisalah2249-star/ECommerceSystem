
using ECommerceSystem.Data;
using ECommerceSystem.Entities;
using Microsoft.EntityFrameworkCore;

using var context = new ECommerceContext();
Console.WriteLine($"Database: {context.Database.GetDbConnection().Database}");
Console.WriteLine($"Server: {context.Database.GetDbConnection().DataSource}");

Console.WriteLine($"Categories: {context.Categories.Count()}");
Console.WriteLine($"Products: {context.Products.Count()}");
Console.WriteLine($"Customers: {context.Customers.Count()}");
Console.WriteLine($"Orders: {context.Orders.Count()}");

if (!context.Categories.Any())
{
    var electronics = new Category { Name = "Electronics" };
    var books = new Category { Name = "Books" };

    context.Categories.AddRange(electronics, books);
    context.SaveChanges();

    context.Products.AddRange(
        new Product
        {
            Name = "Laptop",
            Price = 25000,
            CategoryId = electronics.Id
        },
        new Product
        {
            Name = "Mouse",
            Price = 500,
            CategoryId = electronics.Id
        },
        new Product
        {
            Name = "C# Book",
            Price = 350,
            CategoryId = books.Id
        }
    );

    context.SaveChanges();
}

if (!context.Customers.Any())
{
    context.Customers.AddRange(
        new Customer
        {
            Name = "Ahmed Ali",
            Email = "ahmed@example.com"
        },
        new Customer
        {
            Name = "Mona Hassan",
            Email = "mona@example.com"
        }
    );

    context.SaveChanges();
}

if (!context.Orders.Any())
{
    var customer = context.Customers.First();
    var products = context.Products.Take(2).ToList();

    var order = new Order
    {
        OrderDate = DateTime.Now,
        CustomerId = customer.Id
    };

    context.Orders.Add(order);
    context.SaveChanges();

    context.OrderDetails.AddRange(
        new OrderDetail
        {
            OrderId = order.Id,
            ProductId = products[0].Id,
            Quantity = 1
        },
        new OrderDetail
        {
            OrderId = order.Id,
            ProductId = products[1].Id,
            Quantity = 2
        }
    );

    context.SaveChanges();
}

Console.WriteLine("=== Categories and Products ===");

var categories = context.Categories
    .Include(c => c.Products)
    .ToList();

foreach (var category in categories)
{
    Console.WriteLine($"Category: {category.Name}");

    foreach (var product in category.Products)
    {
        Console.WriteLine(
            $"Product: {product.Name} | Price: {product.Price}");
    }
}

Console.WriteLine("\n=== Orders ===");

var orders = context.Orders
    .Include(o => o.Customer)
    .Include(o => o.OrderDetails)
    .ThenInclude(od => od.Product)
    .ToList();

foreach (var order in orders)
{
    Console.WriteLine($"Order ID: {order.Id}");
    Console.WriteLine($"Customer: {order.Customer.Name}");
    Console.WriteLine($"Date: {order.OrderDate}");

    foreach (var detail in order.OrderDetails)
    {
        Console.WriteLine(
            $"Product: {detail.Product.Name} | Quantity: {detail.Quantity}");
    }
}
