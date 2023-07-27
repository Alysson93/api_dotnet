using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionString"])
);

var app = builder.Build();

var configuration = app.Configuration;
ProductRepository.Init(configuration);

if (app.Environment.IsDevelopment())
{
	app.MapGet("/", (HttpResponse response) => {
		response.Headers.Add("Endpoint", "Root");
		return new {Message = "Hello World"};
	});
	app.MapGet("/token", (HttpRequest request) => {
		return request.Headers["jwt-token"].ToString();
	});
}

// Retorno de todos
app.MapGet("/product", (AppDbContext context) => {
	var products = context.Products.ToList();
	return Results.Ok(products);
});

// Retorno por Id
app.MapGet("/product/{id}", ([FromRoute] int id, AppDbContext context) => {
	var product = context.Products.Where(p => p.Id == id)
	.Include(p => p.Category)
	.Include(p => p.Tags)
	.First();
	if (product == null) return Results.NotFound();
	return Results.Ok(product);
});

// Retorno por Code
app.MapGet("/product/code", ([FromQuery] string code, AppDbContext context) => {
	var product = context.Products.Where(p => p.Code == code)
	.Include(p => p.Category)
	.Include(p => p.Tags)
	.First();
	if (product == null) return Results.NotFound();
	return Results.Ok(product);
});

// Retorno por Name
app.MapGet("/product/name", ([FromQuery] string name, AppDbContext context) => {
	var products = context.Products.Where(p => p.Name == name)
	.Include(p => p.Category)
	.Include(p => p.Tags)
	.ToList();
	return Results.Ok(products);
});

// Criação de produtos
app.MapPost("/product", (ProductRequest request, AppDbContext context) => {
	var category = context.Categories.Where(c => c.Id == request.CategoryId).First();
	var product = new Product {
		Code = request.Code,
		Name = request.Name,
		Description = request.Description,
		Category = category
	};
	if (request.Tags != null)
	{
		product.Tags = new List<Tag>();
		foreach(var item in request.Tags)
		{
			product.Tags.Add(new Tag{Name = item});
		}
	}
	context.Products.Add(product);
	context.SaveChanges();
	return Results.Created($"/product/{product.Id}", product.Id);
});

// Atualização de produto por Id
app.MapPut("/product/{id}", ([FromRoute] int id, ProductRequest request, AppDbContext context) => {
	var product = context.Products.Where(p => p.Id == id)
	.Include(p => p.Tags)
	.First();
	product.Code = request.Code;
	product.Name = request.Name;
	product.Description = request.Description;
	var category = context.Categories.Where(c => c.Id == request.CategoryId).First();
	product.Category = category;
	product.Tags = new List<Tag>();
	if (request.Tags != null)
	{
		product.Tags = new List<Tag>();
		foreach(var item in request.Tags)
		{
			product.Tags.Add(new Tag{Name = item});
		}
	}
	context.SaveChanges();
	return Results.Ok(product);
});

// Remoção de produto por Id
app.MapDelete("/product/{id}", ([FromRoute] int id, AppDbContext context) => {
	var product = context.Products.Where(p => p.Id == id).First();
	context.Products.Remove(product);
	context.SaveChanges();
	return Results.Ok();
});

app.Run();
