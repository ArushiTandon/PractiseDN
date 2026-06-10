namespace PractiseDN.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public decimal Price { get; set; } = 0;
        public string ProductType { get; set; } = null;
    }
}
