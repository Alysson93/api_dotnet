public static class ProductRepository
{
	public static List<Product> Products { get; set; } = new List<Product>();

	public static void Init(IConfiguration configuration)
	{
		var products = configuration.GetSection("Products").Get<List<Product>>();
		Products = products;
	}

	public static void Add(Product product)
	{
		Products.Add(product);
	}

	public static List<Product> Get()
	{
		return Products;
	}

	public static Product Get(string code)
	{
		return Products.FirstOrDefault(p => p.Code == code);
	}

	public static List<Product> GetByName(string name)
	{
		return Products.Where(p => p.Name == name).ToList();
	}

	public static void Remove(Product product)
	{
		Products.Remove(product);
	}

}