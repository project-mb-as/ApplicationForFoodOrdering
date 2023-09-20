using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Domain.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HranaContext context)
        {
            //Remove EnsureCreated when data needs to be preserved - EnsureCreated create new empty db
            //context.Database.EnsureCreated();

            if (!context.Hrana.Any())
            {
                var hrana = new Hrana[]
                {
                new Hrana{Naziv="Pekarski kropmir", Stalna=true}
                //new Hrana{Naziv="Pileci file sa zara", Stalna = true},
                //new Hrana{Naziv="Lignje", Stalna=false}
                };
                foreach (Hrana h in hrana)
                {
                    context.Hrana.Add(h);
                }
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                byte[] passwordHash, passwordSalt;
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin"));
                }
                var admin = new User
                {
                    Email = "admin",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                context.Add(admin);
                context.SaveChanges();
            }

            //if (!context.Menii.Any())
            //{
            //    var meni = new Meni[]
            //    {
            //    new Meni{Datum= DateTime.Now },
            //    new Meni{Datum= DateTime.Now.AddDays(1) }
            //    };
            //    foreach (Meni m in meni)
            //    {
            //        context.Add(m);
            //    }
            //    context.SaveChanges();
            //}

            //if (!context.HranaMeni.Any())
            //{
            //    var hranaMeni = new HranaMeni[]
            //    {
            //        new HranaMeni{HranaId = 1, MeniId = 1},
            //        new HranaMeni{HranaId = 2, MeniId = 1},
            //        new HranaMeni{HranaId = 3, MeniId = 1},
            //        new HranaMeni{HranaId = 3, MeniId = 2}
            //    };
            //    foreach(var hm in hranaMeni)
            //    {
            //        context.Add(hm);
            //    }
            //    context.SaveChanges();
            //}

            //if (!context.Prilozi.Any())
            //{
            //    var prilozi = new Prilog[]
            //    {
            //        new Prilog{Naziv = "Hljeb"},
            //        new Prilog{Naziv = "Sezonska salata"},
            //        new Prilog{Naziv = "Jogurt"},
            //        new Prilog{Naziv = "Mlijeko"}
            //    };
            //    foreach(var prilog in prilozi)
            //    {
            //        context.Add(prilog);
            //    }
            //    context.SaveChanges();
            //}

            //if (!context.HranaPrilozi.Any())
            //{
            //    var hranaPrilozi = new HranaPrilog[]
            //    {
            //        new HranaPrilog{ HranaId = 1, PrilogId = 1, Varijanta = 1},
            //        new HranaPrilog{ HranaId = 1, PrilogId = 2, Varijanta = 1},
            //        //new HranaPrilog{ HranaId = 1, PrilogId = 1, Varijanta = 2},
            //        new HranaPrilog{ HranaId = 1, PrilogId = 3, Varijanta = 2},
            //        new HranaPrilog{ HranaId = 2, PrilogId = 1, Varijanta = 1},
            //        new HranaPrilog{ HranaId = 2, PrilogId = 3, Varijanta = 1},
            //        //new HranaPrilog{ HranaId = 2, PrilogId = 1, Varijanta = 2},
            //        new HranaPrilog{ HranaId = 2, PrilogId = 4, Varijanta = 2},
            //        new HranaPrilog{ HranaId = 3, PrilogId = 1, Varijanta = 1},
            //        new HranaPrilog{ HranaId = 3, PrilogId = 2, Varijanta = 1},
            //    };
            //    foreach (var hp in hranaPrilozi)
            //    {
            //        context.Add(hp);
            //    }
            //    context.SaveChanges();
            //}


            //var bookbs = new Book[]
            //{
            //    new Book{Name="Prva knjiga"},
            //    new Book{Name="Druga knjiga"},

            //};
            //foreach (Book b in bookbs)
            //{
            //    context.Books.Add(b);
            //}
            //context.SaveChanges();

            //var users = new User[]
            //{
            //    new User{FirstName="Prvi", LastName = "Korisnik"},
            //    new User{FirstName="Drugi", LastName = "Korisnik"},

                //};
                //foreach (User u in users)
                //{
                //    context.Users.Add(u);
                //}
                //context.SaveChanges();

                //var enrollments = new SavedBook[]
                //{
                //    new SavedBook{UserId=1,BookId=1}
                //};
                //foreach (SavedBook e in enrollments)
                //{
                //    context.SavedBooks.Add(e);
                //}
                //context.SaveChanges();
        }
    }
}
