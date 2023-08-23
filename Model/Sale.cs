using System.ComponentModel.DataAnnotations;

namespace ExtraaEdge_WEB_API.Model
{
    public class Sale
    {
        public int SaleID { get; set; }
        public int MobileIdNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public int SaleAmount { get; set; }
        public int DiscountApplied { get; set; }
        public Mobile Mobile { get; set; }
       
    }
}
