using Microsoft.AspNetCore.Mvc;


namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class LabtestController : Controller
    {
        private readonly IRepository<LabTest> _repoLabTest;
        private readonly IFileHandler _fileHandler;
    
        public LabtestController(IRepository<LabTest> repoLabTest, IFileHandler fileHandler )
        {
            _repoLabTest = repoLabTest;
            _fileHandler = fileHandler;
        
        }
        public async Task<IActionResult> Index(int page = 1, string? query = null, CancellationToken cancellationToken = default)
        {
            var labtests = await _repoLabTest.GetAllAsync(
                filter: null,
                cancellationToken: cancellationToken,
                Tracked: false,
                includes: i => i.Include(t => t.Appointment).ThenInclude(i => i!.Doctor));

            //search
            if (!string.IsNullOrEmpty(query))
            {
                var filter = query.ToLower().Trim();
                labtests = labtests.Where(t => t.TestName.ToLower().Contains(filter));
            }

            //Pagination 
            var labtestsCount = labtests.Count();
            var totalPages = Math.Ceiling(labtestsCount / 10.0);
            labtests = labtests.Skip((page - 1) * 10).Take(10);

            var model = new LabTestVM()
            {
                labTests = labtests,
                TotalPages = totalPages,
                currentPage = page,
                query = query ?? string.Empty
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create(int appointmentId, int patientId)
        {
            var model = new CreateLabTestVM()
            {
                AppointmentId = appointmentId,
                PatientId = patientId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create (CreateLabTestVM model , CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) 
            {
                TempData["error"] = "Failed To Create Test";
                return View(model);
            }

            var test = new LabTest()
            {
                TestName = model.TestName,
                TestDate = model.TestDate,
                testType = model.testType,
                LabName = model.LabName,
                Summary = model.Summary,
                AppointmentId = model.AppointmentId,
                PatientId = model.PatientId,
            };
            await _repoLabTest.CreateAsync(test, cancellationToken);
            await _repoLabTest.CommitChangesAsync(cancellationToken);
            TempData["success"] = "Lab Test Added Successfully";
            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id, CancellationToken cancellationToken = default)
        {
            var test = await _repoLabTest.GetOneAsync(filter: x => x.Id == id, cancellationToken, Tracked: false,
                includes: a => a.Include(a => a.Patient)
                 .Include(a => a.Appointment).ThenInclude(a => a!.Doctor)
                 .Include(t => t.Attachments));

             if(test is null)
            {
                return NotFound();
            }

            var model = new UpdateLabTestVM()
            {
                Id = test.Id,
                TestName = test.TestName,
                testType = test.testType,
                TestDate = test.TestDate,
                LabName = test.LabName,
                Summary = test.Summary ?? string.Empty,
                AppointmentId = test.AppointmentId,
                PatientId = test.PatientId,
                PatientName = test.Patient?.Name,
                DoctorName = test.Appointment?.Doctor?.Name,
                ExistingAttachments = test.Attachments.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateLabTestVM model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                var currentTest = await _repoLabTest.GetOneAsync(
             filter: t => t.Id == model.Id,
             cancellationToken: cancellationToken,
             Tracked: false,
             includes: q => q.Include(t => t.Patient)
                              .Include(t => t.Appointment).ThenInclude(a => a!.Doctor)
                              .Include(t => t.Attachments));
                                                                 
                //هنا بقوله املا الموديل بالداتا دي تاني لانها مش هتتعدل معايا 
                if (currentTest is not null)
                {
                    model.PatientName = currentTest.Patient?.Name;
                    model.DoctorName = currentTest.Appointment?.Doctor?.Name;
                    model.ExistingAttachments = currentTest.Attachments.ToList();
                }
                return View(model);
            }

            var updatedTest = await _repoLabTest.GetOneAsync(filter: x => x.Id == model.Id,
                cancellationToken, Tracked: true, includes: x => x.Include(x => x.Attachments));

            if (updatedTest is null) 
            {
                return RedirectToAction("Index");
            }
            if(model.TestFiles is not null && model.TestFiles.Count > 0)
            {
                var folderType = model.testType == TestType.Scan ? AttachmentType.scans : AttachmentType.labs;


                foreach (var file in model.TestFiles)
                {
                    string uploadedFile = await _fileHandler.CreateFileAsync(file, folderType);
                    updatedTest.Attachments.Add(new LabTestAttachment
                    {
                        FileName = uploadedFile,
                    });
                }
            }
            updatedTest.TestName = model.TestName;
            updatedTest.testType = model.testType;
            updatedTest.TestDate = model.TestDate;
            updatedTest.LabName = model.LabName;
            updatedTest.Summary = model.Summary;

            await _repoLabTest.CommitChangesAsync(cancellationToken);

            TempData["success"] = "Lab Test updated successfully with audit logs.";
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Details(int id , CancellationToken cancellationToken = default)
        {
            var LabTest = await _repoLabTest.GetOneAsync(filter: x => x.Id == id, cancellationToken, Tracked: false,
                includes: a => a.Include(a => a.Patient)
                 .Include(a => a.Appointment).ThenInclude(a => a!.Doctor)
                 .Include(t => t.Attachments));
            return View(LabTest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken = default)
        {
            var labtest = await _repoLabTest.GetOneAsync(t => t.Id == id , cancellationToken);
            if (labtest is null)
            {
                return NotFound();
            }
            _repoLabTest.Delete(labtest);
            await _repoLabTest.CommitChangesAsync(cancellationToken);
            TempData["success"] = "Lab Test Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
