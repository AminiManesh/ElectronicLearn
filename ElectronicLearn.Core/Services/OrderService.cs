using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Context;
using ElectronicLearn.DataLayer.Entities.Course;
using ElectronicLearn.DataLayer.Entities.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ElectronicLearnContext _context;
        public OrderService(ElectronicLearnContext context)
        {
            _context = context;
        }

        public int AddOrder(int userId, int courseId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinally);

            if (order == null)
            {
                var coursePrice = _context.Courses.Find(courseId).CoursePrice;
                order = new Order()
                {
                    CreateDate = DateTime.Now,
                    UserId = userId,
                    IsFinally = false,
                    User = _context.Users.Find(userId),
                    OrderSum = coursePrice,
                    OrderItems = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Count = 1,
                            CourseId = courseId,
                            Price = _context.Courses.Find(courseId).CoursePrice
                        }
                    }
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            else
            {
                var orderItem = order.OrderItems.FirstOrDefault(o => o.CourseId == courseId);
                if (order.OrderItems != null && orderItem != null)
                {
                    orderItem.Count += 1;
                    _context.OrderItems.Update(orderItem);
                }
                else
                {
                    orderItem = new OrderItem()
                    {
                        Count = 1,
                        CourseId = courseId,
                        OrderId = order.OrderId,
                        Price = _context.Courses.Find(courseId).CoursePrice
                    };
                    _context.OrderItems.Add(orderItem);
                }
                _context.SaveChanges();
                UpdateOrderSum(order.OrderId);
            }
            return order.OrderId;
        }

        public void UpdateOrderSum(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            var orderSum = _context.OrderItems.Where(oi => oi.OrderId == orderId).Sum(oi => oi.Price * oi.Count);
            order.OrderSum = orderSum;
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
