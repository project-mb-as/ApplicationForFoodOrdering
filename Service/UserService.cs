using Domain.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

//TODO make custom Exception for db exceptions

namespace Service
{
    enum Role
    {
        Admin,
        Cook,
        User
    }

    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByEmail(string email);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        List<string> GetEmailsFromAllThatDidNotOrder(DateTime date);
    }
    public class UserService : IUserService
    {
        private HranaContext _context;

        public UserService(HranaContext context)
        {
            _context = context;
        }

        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetByEmail(string email)
        {
            return _context.Users.Where(o => o.Email == email).FirstOrDefault();
        }


        public User Create(User user, string password)
        {
            //validation
            //if (string.IsNullOrWhiteSpace(password))
            //    //throw new AppException("Password is required");
            //    throw new Exception("Password is required");

            //if (_context.Users.Any(x => x.Username == user.Username))
            //    throw new Exception("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User user, string password = null)
        {
            //var user = _context.Users.Find(userParam.UserId);

            //if (user == null)
            //    throw new AppException("User not found");

            // update username if it has changed
            //if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            //{
            //    // throw error if the new username is already taken
            //    if (_context.Users.Any(x => x.Username == userParam.Username))
            //        throw new Exception("Username " + userParam.Username + " is already taken");

            //    user.Username = userParam.Username;
            //}

            // update user properties if provided
            //if (!string.IsNullOrWhiteSpace(userParam.FirstName))
            //    user.FirstName = userParam.FirstName;

            //if (!string.IsNullOrWhiteSpace(userParam.LastName))
            //    user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public List<string> GetEmailsFromAllThatDidNotOrder(DateTime date)
        {
            List<string> ret = null;
            var menu = _context.Menii.Where(o => o.Datum.Date == date.Date).Include(o => o.Narudzbe).FirstOrDefault();
            if(menu != null)
            {
                var usersThatDidOrderd = menu.Narudzbe.Select(o => o.UserId);
                ret = _context.Users.Where(o => o.ReceiveOrderWarningEmails && !usersThatDidOrderd.Any(o1 => o1 == o.UserId))
                    .Select(o => o.Email).ToList();
            }
            return ret;
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
