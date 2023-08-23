using System.ComponentModel.DataAnnotations;

namespace ExtraaEdge_WEB_API.Model
{
    public class Customer
    {
        [Key]
        public int CustId { get; set; }

        public string CustName { get; set; }

        public string CustPhoneNo { get; set; }

        public string CustAddress { get; set; }

        public string CustEmail { get; set; }

        public string CustPassword { get; set; }
    }
}
