
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using test.Interfaces;
using test.Models;

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


        public IActionResult Create()

        {

            return View();

        }


        [HttpPost]

        public async Task<IActionResult> Create(MedicalRecord record)

        {

            if (!ModelState.IsValid)

            {

                return View(record);

            }



            await _medicalRecordRepo.AddAsync(record);

            return RedirectToAction("Details", new { animalId = record.Animalid });

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

    }

}