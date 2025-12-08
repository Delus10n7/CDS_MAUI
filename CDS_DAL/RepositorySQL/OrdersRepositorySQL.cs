using CDS_DAL.Context;
using CDS_DomainModel.Entities;
using CDS_Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_DAL.RepositorySQL
{
    public class OrdersRepositorySQL : IOrdersRepository
    {
        private SqlDbContext db;

        public OrdersRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<Orders> GetList()
        {
            return db.Orders
                .Include(o => o.Client)
                .Include(o => o.Manager)
                .Include(o => o.Car)
                .Include(o => o.Status)
                .ToList();
        }

        public Orders GetItem(int id)
        {
            return db.Orders
                .Include(o => o.Client)
                .Include(o => o.Manager)
                .Include(o => o.Car)
                .Include(o => o.Status)
                .FirstOrDefault(o => o.Id == id);
        }

        public void Create(Orders order)
        {
            db.Orders.Add(order);
        }

        public void Update(Orders order)
        {
            db.Entry(order).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Orders order = db.Orders.Find(id);
            if (order != null)
                db.Orders.Remove(order);
        }

        public List<Orders> GetOrdersByClient(int clientId)
        {
            return db.Orders
                .Where(o => o.ClientId == clientId)
                .Include(o => o.Car)
                    .ThenInclude(c => c.Configuration)
                .Include(o => o.Status)
                .ToList();
        }

        public List<Orders> GetOrdersByStatus(int statusId)
        {
            return db.Orders
                .Where(o => o.StatusId == statusId)
                .Include(o => o.Client)
                .Include(o => o.Car)
                .ToList();
        }

        public List<Orders> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);

            return db.Orders
                .Where(o => o.OrderDate >= start && o.OrderDate <= end)
                .Include(o => o.Client)
                .Include(o => o.Car)
                .Include(o => o.Manager)
                .ToList();
        }

        public List<Orders> GetTradeInOrders()
        {
            return db.Orders
                .Where(o => o.IsTradeIn.GetValueOrDefault(false))
                .Include(o => o.TradeInCar)
                .Include(o => o.Car)
                .Include(o => o.Client)
                .ToList();
        }

        public decimal GetTotalSales(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = db.Orders
                .Where(o => o.StatusId == 1); // Выполнен

            if (startDate.HasValue)
            {
                var start = DateOnly.FromDateTime((DateTime)startDate);
                query = query.Where(o => o.OrderDate >= start);
            }
            if (endDate.HasValue)
            {
                var end = DateOnly.FromDateTime((DateTime)endDate);
                query = query.Where(o => o.OrderDate <= end);
            }

            return query
                .Join(db.Cars, o => o.CarId, c => c.Id, (o, c) => new { Order = o, Car = c })
                .Sum(x => x.Car.Price.GetValueOrDefault(0) - x.Order.TradeInValue.GetValueOrDefault(0));
        }
    }
}
