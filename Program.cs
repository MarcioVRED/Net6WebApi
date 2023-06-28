using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World 2 - testes   !");
app.MapPost("/", () => new {Name = "Marcio Martins, Age=52"});
app.MapGet("/AddHEader", (HttpResponse response) => {
    response.Headers.Add("teste","Marcio Martins");
    return new {Name = "Stephany Batista", Age = 35};
});
app.MapPost("/products", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created($"/products/{product.Code}", product.Code);
});

//app.api.com/users?datastart={date}&dateend={date}
app.MapGet("/products", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});

//api.app.com/user/{code}
app.MapGet("/products/{code}", ([FromRouteAttribute] string code) => {
    var product = ProductRepository.GetBy(code);
    if(product!=null) 
       return Results.Ok(product);
    return Results.NotFound();
});

//api.app.com.
app.MapGet("/productsbyheader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();
});

app.MapPut("/products", (Product product) => {
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name= product.Name;
    return Results.Ok();
});

app.MapDelete("/products/{code}", ([FromRoute] string code) => {
    var productDeleted = ProductRepository.GetBy(code);
    ProductRepository.Remove(productDeleted);
    return Results.Ok();
});

app.Run();

public static class ProductRepository {
    public static List<Product> Products { get; set; }

    public static void Add(Product product)
    {
        if(Products == null)
            Products = new List<Product>();

        Products.Add(product);    
    }

    public static Product GetBy(string code) 
    {
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product) 
    {
        Products.Remove(product);
    }
}

public class Category {
    public int Id {get;set;}
    public string Name { get; set; }
}

public class Tag {
    public int Id { get; set; }
    public string Name { get; set; }
}
public class Product {
    public int Id {get; set;}
    public string Code {get; set; }
    public string Name {get; set; }
    public string Description {get; set;}
    public Category Category { get; set; }
    public List<Tag> Tags { get; set; }
}

public class ApplicationDbContext : DbContext {
    public DbSet<Product> Products {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired(false);
        modelBuilder.Entity<Product>().Property(p => p.Code).HasMaxLength(20).IsRequired(false);
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@Sql2019;MultipleActiveResultSets=true;Encrypt=Yes;TrustServerCertificate=Yes");
    
}