using ECommerce.Shared.Core.Base;

namespace ECommerce.Catalog.Domain
{
    internal class Product : EntityBase
    {
        public DateTime CreateAt { get; private set; }
        public DateTime? UpdateAt { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public string CategoryId { get; private set; }
        public ICollection<string> ProductImages { get; } = new List<string>();

        public static Product Create(string name, string description, decimal price, int stock, string categoryId)
        {
            return new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                CategoryId = categoryId,
                CreateAt = DateTime.UtcNow
            };
        }

        public void AddImage(string imageUrl)
        {
            if (ProductImages.Contains(imageUrl))
            {
                ProductImages.Remove(imageUrl);
            }

            ProductImages.Add(imageUrl);
            UpdateAt = DateTime.UtcNow;
        }

        public void Update(string name, string description, decimal price, int stock, string categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            UpdateAt = DateTime.UtcNow;
        }
    }
}
