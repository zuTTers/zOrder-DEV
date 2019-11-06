using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using zOrder.OMS.Helper;
using zOrder.OMS.Models;

namespace zOrder.OMS.Controllers.GENERAL
{
    public class ReceiptController : ApiController
    {
        //Fiş bilgileri
        [HttpGet, HttpPost]
        [ActionName("Detail")]
        public JsonResult<ReturnValue> Detail(int id)
        {
            DisplayReceipt orderreceipt = new DisplayReceipt();
            ReturnValue ret = new ReturnValue();
            try
            {
                ret.success = false;
                //Shared.CheckSession();

                using (var db = new zOrderEntities())
                {
                    var orderdata = db.Orders.Where(x => x.Order_Id == id).FirstOrDefault();

                    orderreceipt.Order_Id = orderdata.Order_Id;
                    orderreceipt.CreatedUser = orderdata.CreatedUser;
                    orderreceipt.CreatedUserText = orderdata.Users.Name;
                    orderreceipt.CustomerName = orderdata.CustomerName;
                    orderreceipt.PhoneNumber = orderdata.PhoneNumber;
                    orderreceipt.Debt = orderdata.Debt;
                    orderreceipt.Addition = orderdata.Addition;
                    //orderreceipt.Discount = orderdata.Discount;
                    orderreceipt.OrderDate = orderdata.OrderDate;
                    orderreceipt.CreatedDate = orderdata.CreatedDate;
                    orderreceipt.TTotalPrice = Convert.ToInt32(orderdata.OrderDetail.Sum(x => x.TotalPrice).Value);
                    orderreceipt.NTotalPrice = Convert.ToInt32(orderdata.OrderDetail.Sum(x => x.TotalPrice).Value) - ((Convert.ToInt32(orderdata.OrderDetail.Sum(x => x.TotalPrice).Value) * Convert.ToInt32(orderdata.Discount) / 100));
                    orderreceipt.TQuantity = orderdata.OrderDetail.Sum(x => x.Quantity).Value;
                    orderreceipt.DetailList = new List<OrderDetails>();
                    orderreceipt.IsPaid = orderdata.IsPaid.HasValue ? orderdata.IsPaid.Value : false;

                    foreach (var item in orderdata.OrderDetail.ToList())
                    {
                        orderreceipt.DetailList.Add(new OrderDetails
                        {
                            Operation_Id = item.Operation_Id,
                            Quantity = item.Quantity,
                            Price = Convert.ToInt32(item.Price),
                            TotalPrice = Convert.ToInt32(item.TotalPrice),
                            OrderDetail_Id = item.OrderDetail_Id,
                            OperationText = item.Operations.Name,
                            ProductName = item.Operations.Products.Name

                        });
                    }

                    ret.retObject = orderreceipt;
                }
                ret.success = true;
                ret.message = "";
            }
            catch (Exception ex)
            {
                //ex.AddToDBLog("HomeController.Receipt", ex.Message);
                ret.success = false;
                ret.error = ex.Message;
            }

            return Json(ret);
        }
    }

    public class OrderDetails
    {
        public int OrderDetail_Id { get; set; }
        public Nullable<int> Order_Id { get; set; }
        public Nullable<int> Operation_Id { get; set; }
        public string OperationText { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> TTotalPrice { get; set; }
        public int TQuantity { get; set; }
        public string ProductName { get; set; }

    }

    public class DisplayReceipt
    {
        public int Order_Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Debt { get; set; }
        public string Addition { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public string CreatedUserText { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ProductName { get; set; }
        public decimal TTotalPrice { get; set; }
        public decimal NTotalPrice { get; set; }
        public int TQuantity { get; set; }
        public bool IsPaid { get; set; }
        public List<OrderDetails> DetailList { get; set; }
        public Nullable<int> Discount { get; set; }

    }


}
