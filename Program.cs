using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World 2 - testes   !");
app.MapPost("/", () => new {Name = "Marcio Martins, Age=52"});
app.MapGet("/AddHEader", (HttpResponse response) => {
    response.Headers.Add("teste","Marcio Martins");
    return new {Name = "Stephany Batista", Age = 35};
});
app.MapPost("/saveproduct", (Product product) => {
    return product.Code + " - " + product.Name;
});

//app.api.com/users?datastart={date}&dateend={date}
app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});

//api.app.com/user/{code}
app.MapGet("/getproduct/{code}", ([FromRouteAttribute] string code) => {
    return code;
});

//api.app.com.
app.MapGet("/getproductbyheader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();
});

app.Run();

public class Product {
    public string Code {get; set; }
    public string Name {get; set; }
}