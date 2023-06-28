using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);
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
