
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using test.Interfaces;
using test.Models;
using test.ModelViews;

namespace test.Controllers

{
    [Authorize]

    public class MedicalRecordController : Controller
        
    {

        private readonly IMedicalRecord _medicalRecordRepo;



        public MedicalRecordController(IMedicalRecord medicalRecordRepo)

        {

            _medicalRecordRepo = medicalRecordRepo;

        }


        public async Task<IActionResult> Details(int animalId)

        {

            var record = await _medicalRecordRepo.GetByAnimalIdAsync(animalId);

            if (record == null)

            {

                return NotFound();

            }

            return View(record);

        }

        [HttpGet]
        public IActionResult Create(int animalid)

        {
            var model = new CreateMedicalRecordViewModel
            {
                animalId = animalid
            };

            return View(model);

        }


        [HttpPost]

        public async Task<IActionResult> Create(CreateMedicalRecordViewModel model)

        {

            if (!ModelState.IsValid)

            {

                return View(model);

            }
            var record = await _medicalRecordRepo.GetByAnimalIdAsync(model.animalId);
            if ( record!= null)
            {
                return RedirectToAction("Create", "VaccinationNeeded", new { medicalid = record.Recordid });
            }
             record = new MedicalRecord
            {
                Animalid = model.animalId,
                Injurys = model.injurys,
                Status = model.status
            };


            await _medicalRecordRepo.AddAsync(record);

            return RedirectToAction("Create", "VaccinationNeeded",new {medicalid=record.Recordid });

        }


        public async Task<IActionResult> Edit(int animalId)

        {

            var record = await _medicalRecordRepo.GetByAnimalIdAsync(animalId);

            if (record == null)

            {

                return NotFound();

            }

            return View(record);

        }


        [HttpPost]

        public async Task<IActionResult> Edit(MedicalRecord record)

        {

            if (!ModelState.IsValid)

            {

                return View(record);

            }



            await _medicalRecordRepo.UpdateAsync(record);

            return RedirectToAction("Details", new { animalId = record.Animalid });

        }


        public async Task<IActionResult> Delete(int animalId)

        {

            var record = await _medicalRecordRepo.GetByAnimalIdAsync(animalId);

            if (record == null)

            {

                return NotFound();

            }

            return View(record);

        }


        [HttpPost]

        [ActionName("DeleteConfirmed")]

        public IActionResult DeleteConfirmed(int animalId)

        {

            _medicalRecordRepo.RemoveByAnimalId(animalId);

            return RedirectToAction("Index");

        }


        public IActionResult Index()

        {

            var allRecords = _medicalRecordRepo.GetAll();

            return View(allRecords);

        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int animalId)
        {
            var record = await _medicalRecordRepo.GetByAnimalIdAsync(animalId);
            if (record == null)
            {
                return NotFound();
            }

            // Toggle status between Healthy and Unhealthy
            record.Status = record.Status == "Healthy" ? "Unhealthy" : "Healthy";
            await _medicalRecordRepo.UpdateAsync(record);

            return RedirectToAction("Details", new { animalId = animalId });
        }

    }

}