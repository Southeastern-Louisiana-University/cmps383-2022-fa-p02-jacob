using FA22.P02.Web.Features;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
var myId = 1;

var products = new List<ProductsDto>
{
    new ProductsDto
    {
        Id = myId++,
        Name = "button",
        Description = "Very Shiney",
        Price = 99.99m
    },
    new ProductsDto
    {
        Id = myId++,
        Name = "Different",
        Description = "this is different",
        Price = 20.01m
    },
    new ProductsDto
    {
        Id = myId++,
        Name = "NowOrLater",
        Description = "sweet and sour",
        Price = 0.50m
    }

};

app.MapGet("/api/products", () =>
{
    return products;
})
    .Produces(200, typeof(ProductsDto[]));

app.MapGet("/api/products/{id}", (int id) =>
{
    var result = products.FirstOrDefault(x => x.Id == id);
    if (result == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result);
})
 .WithName("GetById");

app.MapPost("/api/products", (ProductsDto product) =>
 {
     if (product.Name == null || product.Name.Length > 120 || product.Price <= 0 || product.Description == null)
     {
         return Results.BadRequest(product);
     }

     product.Id = myId++;
     products.Add(product);

     return Results.CreatedAtRoute("GetById", new { id = product.Id }, product);

 })
    .Produces(400)
    .Produces(201, typeof(ProductsDto));


app.MapPut("/api/products/{id}", (int id, ProductsDto product) =>
{
    if (product.Name == null || product.Name.Length > 120 || product.Description == null || product.Price <= 0)
    {
        return Results.BadRequest();
    }
    var current = products.FirstOrDefault(x => x.Id == id);

    if (current == null)
    {
        return Results.NotFound();
    }
 
    return Results.Ok(current);
    
})
    .Produces(400)
    .Produces(200 ,typeof(ProductsDto));

app.MapDelete("/api/products/{id}", (int id) =>
{
    var current = products.FirstOrDefault(x => x.Id == id);

    if (current == null)
    {
        return Results.NotFound();

    }
    products.Remove(current);


    return Results.Ok();
})
    .Produces(404)
    .Produces(400)
    .Produces(200, typeof(ProductsDto));
    
app.Run();


//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program { }