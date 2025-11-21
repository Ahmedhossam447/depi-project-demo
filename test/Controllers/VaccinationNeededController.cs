using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    [Authorize]
    public class VaccinationNeededController : Controller
    {
        private readonly IVaccinationNeeded _vaccineRepo;

        public VaccinationNeededController(IVaccinationNeeded vaccineRepo)
        {
            _vaccineRepo = vaccineRepo;
        }



        public async Task<IActionResult> ByAnimal(int animalId)
        {
            var vaccines = await _vaccineRepo.GetByAnimalIdAsync(animalId);
            return View(vaccines);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VaccinationNeeded vaccine)
        {
            if (!ModelState.IsValid)
                return View(vaccine);

            await _vaccineRepo.AddAsync(vaccine);
            return RedirectToAction("ByAnimal", new { animalId = vaccine.MedicalRecord.Animalid });
        }

        public IActionResult Delete(int id)
        {
            _vaccineRepo.Remove(id);
            return RedirectToAction("Index");
        }
    }
}