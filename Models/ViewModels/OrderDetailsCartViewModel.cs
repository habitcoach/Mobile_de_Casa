using System.Collections.Generic;

namespace Mobile_de_Casa.Models.ViewModels
{
    public class OrderDetailsCartViewModel
    {
        public List<ShoppingCart> listCart { get; set; }
        public OrderMaster orderMaster { get; set; }
    }
}
