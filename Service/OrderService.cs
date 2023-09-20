using Domain.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

//TODO make custom Exception for db exceptions

namespace Service
{
    public interface IOrderService
    {
        IEnumerable<Narudzba> GetAll(int menuId);
        IEnumerable<Narudzba> GetAllForUser(int userId);
        Narudzba GetByMenuId(int menuId, int userId);
        void Delete(int id);
        void DeleteAllForUser(int userId);
        void Delete(Narudzba order);
        Narudzba Get(int id);
        Narudzba CreateOrUpdate(Narudzba hrana, int userId);

    }
    public class OrderService : IOrderService
    {
        private HranaContext _context;

        public OrderService(HranaContext context)
        {
            _context = context;
        }

        public IEnumerable<Narudzba> GetAll(int menuId)
        {
            return _context.Narudzbe.Where(o => o.MeniId == menuId)
                        .Include(o => o.User)
                        .Include(o => o.SideDishes)
                        .Include(o => o.Hrana);
        }

        public IEnumerable<Narudzba> GetAllForUser(int userId)
        {
            return _context.Narudzbe.Where(o => o.UserId == userId);
        }


        public Narudzba CreateOrUpdate(Narudzba order, int userId)
        {
            Narudzba ret;
            var oldOrder = GetByMenuId(order.MeniId, Convert.ToInt32(userId));
            if (oldOrder != null)
            {
                oldOrder.SideDishes = order.SideDishes;
                oldOrder.TimeId = order.TimeId;
                oldOrder.HranaId = order.HranaId;
                oldOrder.LocationId = order.LocationId;
                oldOrder.Note = order.Note;
                _context.Entry(oldOrder).State = EntityState.Modified;
                ret = oldOrder;
            }
            else
            {
                ret = _context.Narudzbe.Add(order).Entity;
            }
            _context.SaveChanges();
            return ret;
        }

        public Narudzba GetByMenuId(int menuId, int userId)
        {
            return _context.Narudzbe.Where(o => o.MeniId == menuId && o.UserId == userId)
                .Include(o => o.SideDishes).FirstOrDefault();
        }

        public Narudzba Get(int id)
        {
            return _context.Narudzbe.Where(o => o.NarudzbaId == id)
                .Include(o => o.Hrana)
                .Include(o => o.Meni)
                .Include(o => o.SideDishes)
                    .ThenInclude(sideDish => sideDish.Prilog)
                .FirstOrDefault();
        }

        public void Delete(int id)
        {
            var order = _context.Narudzbe.Find(id);
            if (order != null)
            {
                _context.Narudzbe.Remove(order);
                _context.SaveChanges();
            }
        }

        public void DeleteAllForUser(int userId)
        {
            var orders = _context.Narudzbe.Where(o => o.UserId == userId);
            if (orders.Count() > 0)
            {
                _context.Narudzbe.RemoveRange(orders);
                _context.SaveChanges();
            }
        }

        public void Delete(Narudzba order)
        {
            if (order != null)
            {
                _context.OrderSideDishes.RemoveRange(_context.OrderSideDishes.Where(o => o.NarudzbaId == order.NarudzbaId));
                _context.Narudzbe.Remove(order);
                _context.SaveChanges();
            }
        }



    }
}
