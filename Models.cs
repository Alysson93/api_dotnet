public class Product
{
	public int Id { get; set; }
	public string? Code { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public Category Category { get; set; }
	public int CategoryId { get; set; }
	public List<Tag> Tags { get; set; }
}

public class Category
{
	public int Id { get; set; }
	public string? Name { get; set; }
}

public class Tag
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public int ProductId { get; set; }
}