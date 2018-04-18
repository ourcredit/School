using System;
using System.Collections.Generic;
using System.Text;

namespace School.Others.Dtos
{
  public  class DealOrderInput
    {
        public string MachineCode { get; set; }
        public List<OrderInfoA> orders { get; set; }
        public class OrderInfoA
        {
            public string orderId { get; set; }
            public string orderStatus  { get; set; }
        }
    }
    public class CashOrderInput
    {
        public string MachineCode { get; set; }
        public List<OrderInfoB> orders { get; set; }
        public class OrderInfoB
        {
            public int productId { get; set; }
            public int price { get; set; }
            public DateTime venDoutDate { get; set; }
            public string orderStatus { get; set; }
        }
    }
}
