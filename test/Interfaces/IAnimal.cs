using test.Models;
using test.ModelViews;

namespace test.Interfaces
{
    public interface IAnimal
    {
        public  Animalviewmodel AnimalDisplay(string? filter, string id,bool mine);
        public Task<Animal> GetByIdAsync(int id);
        public Task<bool> DeleteAnimal(Animal animal);
        public Task<bool> AddAnimal(Animal animal);
        public bool savechanges();
    }
}
