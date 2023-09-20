using Domain.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

//TODO make custom Exception for db exceptions

namespace Service
{
    public interface IHranaService
    {
        IEnumerable<Hrana> GetAll();
        IEnumerable<Prilog> GetAllSideDishes();
        Hrana GetById(int id);
        int CreateSideDish(string name);
        Prilog GetSideDish(int id);
        void Delete(int id);
        Hrana CreateOrUpdate(Hrana hrana);
        void SetRate(int userId, int foodId, int mark);
        void SetComment(int userId, int foodId, string comment, string image);
        IEnumerable<Komentar> GetComments(int foodId);
    }
    public class HranaService : IHranaService
    {
        private HranaContext _context;

        public HranaService(HranaContext context)
        {
            _context = context;
        }

        public IEnumerable<Hrana> GetAll()
        {
            return _context.Hrana
                        .Include(o => o.Ocjene)
                        .Include(o => o.Prilozi)
                        .Include(o => o.Komentari);
            //.ThenInclude(o => o.Prilog);
        }

        public IEnumerable<Prilog> GetAllSideDishes()
        {
            return _context.Prilozi;
        }

        public Hrana CreateOrUpdate(Hrana hrana)
        {
            Hrana ret;
            if (hrana.HranaId != 0)
            {
                _context.HranaPrilozi.RemoveRange(_context.HranaPrilozi.Where(o => o.HranaId == hrana.HranaId));
                ret = _context.Hrana.Update(hrana).Entity;
            }
            else
            {
                ret = _context.Hrana.Add(hrana).Entity;
            }
            _context.SaveChanges();
            return ret;
        }
        public int CreateSideDish(string name)
        {
            var sideDish = _context.Prilozi.Add(new Prilog { Naziv = name });
            _context.SaveChanges();
            return sideDish.Entity.PrilogId;
        }

        public Prilog GetSideDish(int id)
        {
            return _context.Prilozi.Find(id);
        }

        public void SetRate(int userId, int foodId, int mark)
        {
            var rate = _context.Ocjene.Where(o => o.UserId == userId && o.HranaId == foodId).FirstOrDefault();
            if (rate == null)
            {
                var newRate = new Ocjena
                {
                    UserId = userId,
                    HranaId = foodId,
                    Vrijednost = mark
                };
                _context.Ocjene.Add(newRate);
            }
            else
            {
                rate.Vrijednost = mark;
                _context.Ocjene.Update(rate);
            }
            _context.SaveChanges();
        }

        public void SetComment(int userId, int foodId, string comment, string image)
        {

            var newComment = new Komentar
            {
                UserId = userId,
                HranaId = foodId,
                Comment = comment,
                Slika = image,
                Time = DateTime.Now
            };
            _context.Komentari.Add(newComment);

            var food = _context.Hrana.Find(foodId);
            if (string.IsNullOrEmpty(food.Image))
            {
                food.Image = image;
            }

            _context.SaveChanges();
        }

        public IEnumerable<Komentar> GetComments(int foodId)
        {
            return _context.Komentari.Where(o => o.HranaId == foodId).OrderByDescending(o => o.Time).Include(o => o.User);
        }

        public Hrana GetById(int id)
        {
            return _context.Hrana.Find(id);
        }

        public void Delete(int id)
        {
            var hrana = _context.Hrana.Find(id);
            if (hrana != null)
            {
                _context.Hrana.Remove(hrana);
                _context.SaveChanges();
            }
        }


    }
}
