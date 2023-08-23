namespace ExtraaEdge_WEB_API.Model
{
    public class Mobile
    {
        public int MobileID { get; set; }
        public int BrandID { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }


        public Brand Brand { get; set; }
        public ICollection<Sale> Sales { get; set; }

    }
}
