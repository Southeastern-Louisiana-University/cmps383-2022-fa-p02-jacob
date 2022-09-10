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

var id = 1;
var products = new List<ProductDto>
{
   new ProductDto {Id =id++, Name = "Orbit", Price= 3.05M, Description=" silver beige toy poodle"},
   new ProductDto {Id =id++, Name = "Yogi", Price= 3.05M, Description=" blue toy poodle"},
   new ProductDto {Id =id++, Name = "Tribble", Price= 3.05M, Description=" black toy poodle"},
   new ProductDto {Id =id++, Name = "Daisy", Price= 3.05M, Description="black and white toy poodle"},
};


app.MapGet("/api/products", () =>
{
    //return list of products and status code 200
    return products;
})
.WithName("ListAllProducts").Produces(200, typeof(ProductDto[]));



app.MapGet("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(product => product.Id == id);

    if (product == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);
})
.WithName("GetProductById");

app.MapPost("/api/products", (ProductDto product) =>
{
    //add new prouct to the DB
    //Check for name, name less than 120, description, price greater than zero
    if (product.Name == null || product.Name.Length > 120 || product.Description == null || product.Price <= 0)
    {
        return Results.BadRequest(product);
    }
    product.Id = id++;
    products.Add(product);
    return Results.CreatedAtRoute("CreateProduct", product);

}).WithName("CreateProduct")
    .Produces(400)
    .Produces(201, typeof(ProductDto));

app.Run();

internal record Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
};
//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program { }