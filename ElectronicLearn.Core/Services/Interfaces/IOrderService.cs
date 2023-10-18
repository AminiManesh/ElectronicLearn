using ElectronicLearn.Core.DTOs;
using ElectronicLearn.DataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services.Interfaces
{
    public interface IOrderService
    {
        int AddOrder(int userId, int courseId);
        void UpdateOrder(Order order);
        void UpdateOrderSum(int orderId);
        Order GetOrderForUserPanel(int userId, int orderId);
        Order GetOrderById(int orderId);
        bool FinalOrder(int userId, int orderId);
        List<Order> GetUserOrders(int userId);

        #region Discount
        DiscountStatusType ApplyDiscount(int orderId, string code);
        List<Discount> GetAllDiscounts();
        int AddDiscount(Discount discount);
        Discount GetDiscountById(int discountId);
        int UpdateDiscount(Discount discount);
        void RemoveDiscount(int discountId);
        void RemoveDiscount(Discount discount);
        bool IsCodeExists(string discountCode);
        #endregion
    }
}
