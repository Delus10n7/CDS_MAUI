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
    public class OrderStatusRepositorySQL : IRepository<OrderStatus>
    {
        private SqlDbContext db;

        public OrderStatusRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<OrderStatus> GetList()
        {
            return db.OrderStatus.ToList();
        }

        public OrderStatus GetItem(int id)
        {
            return db.OrderStatus.FirstOrDefault(i => i.Id == id);
        }

        public void Create(OrderStatus item)
        {
            db.OrderStatus.Add(item);
        }

        public void Update(OrderStatus item)
        {
            db.OrderStatus.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            OrderStatus orderStatus = db.OrderStatus.Find(id);
            if (orderStatus != null)
                db.OrderStatus.Remove(orderStatus);
        }
    }
}
