using System.ComponentModel.DataAnnotations;

namespace ExtraaEdge_WEB_API.Model
{
    public class LoginModel
    {
        [Key]
        public string StoreOwnerName { get; set; }
        public string Password { get; set; }
    }
}
