using test.Models;
using test.ModelViews;

namespace test.Interfaces
{
    public interface IAnimal
    {
        public  Animalviewmodel AnimalDisplay(string? filter, string id);
        public Task<bool> AddAnimal(Animal animal);
        public bool savechanges();
    }
}
