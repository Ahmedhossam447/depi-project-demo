using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Interfaces;
using test.Models;
using test.ModelViews;

namespace test.Repository
{
    public class AnimalRepository:IAnimal
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly DepiContext _context;

        public AnimalRepository(DepiContext context,UserManager<IdentityUser> usermanager) { 
            _usermanager = usermanager;
        
        _context= context;
        }

        public async Task<bool> AddAnimal(Animal animal)
        {
            await _context.Animals.AddAsync(animal);
            return savechanges();
        }

        public Animalviewmodel AnimalDisplay(string? filter, string userid)
        {
            IQueryable<Animal> animals;

            if (filter == null || filter == "any")
            {

                animals = _context.Animals.FromSql($"select name,type,age,animalid,photo,userid from Animals as a where userid != {userid} and NOT EXISTS(select animalid from requests where userid = {userid} and animalID= a.animalID)");

            }
            else
            {
                animals = from anm in _context.Animals where anm.Userid! != userid && anm.Type == filter && (_context.Requests.FirstOrDefault(r => r.Userid == userid && r.AnimalId == anm.AnimalId) == null) select anm;
            }
             var animviewmodel = new ModelViews.Animalviewmodel
            {
                animals = animals,
                filter = filter ?? "any"
            };
            return animviewmodel;
        }

        public bool savechanges()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

       
    }
}
