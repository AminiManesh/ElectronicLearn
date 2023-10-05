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
        void UpdateOrderSum(int orderId);
    }
}
