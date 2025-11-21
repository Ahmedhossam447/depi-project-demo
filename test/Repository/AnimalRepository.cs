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

        public Animalviewmodel AnimalDisplay(string? filter, string userid,bool mine)
        {
            IQueryable<Animal> animals;
            Animalviewmodel animviewmodel;
            if (mine)
            {
                animals = from anm in _context.Animals where anm.Userid == userid select anm;
                animviewmodel = new Animalviewmodel
                {
                    animals = animals,
                    filter = filter ?? "any",
                    IsMine = true
                };
                return animviewmodel;
            }
            else
            {

                if (filter == null || filter == "any")
                {

                    animals = _context.Animals.FromSql($"select name,type,age,animalid,photo,userid from Animals as a where userid != {userid} and NOT EXISTS(select animalid from requests where userid = {userid} and animalID= a.animalID)");

                }
                else
                {
                    animals = from anm in _context.Animals where anm.Userid! != userid && anm.Type == filter && (_context.Requests.FirstOrDefault(r => r.Userid == userid && r.AnimalId == anm.AnimalId) == null) select anm;

                }
                animviewmodel = new Animalviewmodel
                {
                    animals = animals,
                    filter = filter ?? "any",
                    IsMine = false
                };
            }

            return animviewmodel;
        }

        public bool savechanges()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public async Task<Animal> GetByIdAsync(int id)
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.AnimalId == id);
            return  animal;
        }
        public async Task<bool> DeleteAnimal(Animal animal)
        {
            _context.Animals.Remove(animal);
            return savechanges();
        }

    }
}
