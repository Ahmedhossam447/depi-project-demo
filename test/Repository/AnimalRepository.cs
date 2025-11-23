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
                animals = _context.Animals
                    .Include(a => a.MedicalRecords)
                    .Where(anm => anm.Userid == userid);
                
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
                    // For complex queries with FromSql, Include might not work directly if the shape changes, 
                    // but here we are selecting specific columns in the raw SQL which returns Animal entities.
                    // However, FromSql doesn't support Include easily if the SQL doesn't return all columns or if it's a complex projection.
                    // The current SQL selects specific columns. 
                    // Let's try to rewrite this using LINQ if possible, or just use LINQ for the other parts.
                    // The SQL uses "NOT EXISTS" for requests.
                    
                    // Let's try to keep the SQL but we can't easily Include on it if it's partial.
                    // Actually, the SQL selects columns into the Animal entity.
                    // If we want to check for MedicalRecords, we need them loaded.
                    // Let's rewrite to LINQ to support Include safely and cleaner.
                    
                    animals = _context.Animals
                        .Include(a => a.MedicalRecords)
                        .Where(a => a.Userid != userid && !_context.Requests.Any(r => r.Userid == userid && r.AnimalId == a.AnimalId));
                }
                else
                {
                    animals = _context.Animals
                        .Include(a => a.MedicalRecords)
                        .Where(anm => anm.Userid != userid && anm.Type == filter && !_context.Requests.Any(r => r.Userid == userid && r.AnimalId == anm.AnimalId));
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
