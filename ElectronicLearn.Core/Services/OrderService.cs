using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Context;
using ElectronicLearn.DataLayer.Entities.Course;
using ElectronicLearn.DataLayer.Entities.Order;
using ElectronicLearn.DataLayer.Entities.User;
using ElectronicLearn.DataLayer.Entities.Wallet;
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
        private readonly IWalletService _walletService;
        public OrderService(ElectronicLearnContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        public int AddDiscount(Discount discount)
        {
            _context.Add(discount);
            _context.SaveChanges();
            return discount.DiscountId;
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

        public DiscountStatusType ApplyDiscount(int orderId, string code)
        {
            var discount = _context.Discounts.SingleOrDefault(d => d.DiscountCode == code);

            if (discount == null)
                return DiscountStatusType.NotAvailable;

            if (discount.UsableCount != null && discount.UsableCount < 1)
                return DiscountStatusType.OutOfStock;

            if (discount.StartTime != null && discount.StartTime > DateTime.Now)
                return DiscountStatusType.NotStarted;

            if (discount.EndTime != null && discount.EndTime <= DateTime.Now)
                return DiscountStatusType.TimeExpired;

            var order = GetOrderById(orderId);

            if (_context.UsersDiscounts.Any(ud => ud.UserId == order.UserId && ud.DiscountId == discount.DiscountId))
                return DiscountStatusType.UserUsed;

            int offer = (order.OrderSum * discount.Percent) / 100;
            order.OrderSum -= offer;
            UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }
            _context.Discounts.Update(discount);
            _context.UsersDiscounts.Add(new UserDiscount()
            {
                UserId = order.UserId,
                DiscountId = discount.DiscountId
            });
            _context.SaveChanges();

            return DiscountStatusType.Available;
        }

        public bool FinalOrder(int userId, int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Course)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null || order.IsFinally)
            {
                return false;
            }

            if (order.OrderSum <= _walletService.GetUserBalance(userId))
            {
                order.IsFinally = true;
                var transactionId = _walletService.AddTransaction(userId, order.OrderSum, $"فاکتور شماره #{order.OrderId}", false, true);
                _walletService.UpdateUserBalance(transactionId);

                foreach (var item in order.OrderItems)
                {
                    if (_context.UsersCourses.Any(uc => uc.UserId == userId && uc.CourseId == item.CourseId))
                    {
                        _context.UsersCourses.Add(new UserCourse()
                        {
                            CourseId = item.CourseId,
                            UserId = userId
                        });
                    }
                }

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public List<Discount> GetAllDiscounts()
        {
            return _context.Discounts.ToList();
        }

        public Discount GetDiscountById(int discountId)
        {
            return _context.Discounts.Find(discountId);
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public Order GetOrderForUserPanel(int userId, int orderId)
        {
            var order = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Course).FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
            return order;
        }

        public List<Order> GetUserOrders(int userId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Course)
                .Where(o => o.UserId == userId).ToList();
        }

        public bool IsCodeExists(string discountCode)
        {
            return _context.Discounts.Any(d => d.DiscountCode == discountCode);
        }

        public void RemoveDiscount(Discount discount)
        {
            _context.Discounts.Remove(discount);
            _context.SaveChanges();
        }

        public void RemoveDiscount(int discountId)
        {
            _context.Discounts.Remove(_context.Discounts.Find(discountId));
            _context.SaveChanges();
        }

        public int UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            _context.SaveChanges();
            return discount.DiscountId;
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
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