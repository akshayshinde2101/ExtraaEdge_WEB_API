namespace ExtraaEdge_WEB_API.Model
{
    public class Brand
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }

        public ICollection<Mobile> Mobiles { get; set; }

    }
}
