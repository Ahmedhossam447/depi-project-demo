using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Interfaces;
using test.Models;
using test.ViewModels;

namespace test.Repository
{
    public class AnimalRepository:IAnimal
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly DepiContext _context;

        public AnimalRepository(DepiContext context,UserManager<ApplicationUser> usermanager) { 
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
                animals = _context.Animals
                    .Include(a => a.MedicalRecords)
                    .Where(anm => anm.Userid == userid);
                
                animviewmodel = new Animalviewmodel
                {
                    animals = animals.ToList(),
                    filter = filter ?? "any",
                    IsMine = true
                };
                return animviewmodel;
            }
            else
            {

                if (filter == null || filter == "any")
                {
                    
                    animals = _context.Animals
                        .Include(a => a.MedicalRecords)
                        .Include(a => a.User)
                        .Where(a => a.Userid != userid && !_context.Requests.Any(r => r.Userid == userid && r.AnimalId == a.AnimalId));
                }
                else
                {
                    animals = _context.Animals
                        .Include(a => a.MedicalRecords)
                        .Include(a => a.User)
                        .Where(anm => anm.Userid != userid && anm.Type == filter && !_context.Requests.Any(r => r.Userid == userid && r.AnimalId == anm.AnimalId));
                }
                animviewmodel = new Animalviewmodel
                {
                    animals = animals.ToList(),
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
        public Task<List<Animal>> GetAllUserAnimalsAsync(string id)
        {
            var animals = _context.Animals
                .Include(a => a.MedicalRecords)
                .Where(a => a.Userid == id)
                .ToListAsync();
            return animals;
        }
        public async Task<List<Animal>> GetAllAnimalsAsync() { 
            var animals= await _context.Animals.ToListAsync();
            return animals;
        }

        public async Task<bool> UpdateAnimal(Animal animal)
        {
            _context.Animals.Update(animal);
            return savechanges();
        }

        public async Task<Animal?> FindDuplicateAsync(string name, string type, byte? age, string userId)
        {
            return await _context.Animals
                .FirstOrDefaultAsync(a => a.Name == name && a.Type == type && a.Age == age && a.Userid == userId);
        }
    }
}
