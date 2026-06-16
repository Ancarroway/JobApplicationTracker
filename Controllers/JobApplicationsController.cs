
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobApplicationTracker.Models;
using JobApplicationTracker.Data;

public class JobApplicationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public JobApplicationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: JOBAPPLICATIONS/Details/5
    public async Task<IActionResult> Index(string? searchString)
    {
        var applications = from j in _context.JobApplications
                              select j;
        if (!String.IsNullOrEmpty(searchString))
        {
            applications = applications.Where(s =>
            s.CompanyName!.Contains(searchString) ||
            s.JobTitle!.Contains(searchString) ||
            s.Location.Contains(searchString));
        }

        ViewBag.AppliedCount=
            await _context.JobApplications.CountAsync(j => j.Status == "Applied");
        ViewBag.InterviewCount =
            await _context.JobApplications.CountAsync(j => j.Status == "Interviewing");
        ViewBag.OfferCount =
            await _context.JobApplications.CountAsync(j => j.Status == "Offer");
        ViewBag.RejectedCount =
            await _context.JobApplications.CountAsync(j => j.Status == "Rejected");

        return View (await applications.ToListAsync());
    }

    // GET: JOBAPPLICATIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: JOBAPPLICATIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("id,CompanyName,JobTitle,Location,Status,DateApplied,SalaryRange,JobPostingUrl,Notes")] JobApplication jobapplication)
    {
        if (ModelState.IsValid)
        {
            _context.Add(jobapplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(jobapplication);
    }

    // GET: JOBAPPLICATIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jobapplication = await _context.JobApplications.FindAsync(id);
        if (jobapplication == null)
        {
            return NotFound();
        }
        return View(jobapplication);
    }

    // POST: JOBAPPLICATIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("id,CompanyName,JobTitle,Location,Status,DateApplied,SalaryRange,JobPostingUrl,Notes")] JobApplication jobapplication)
    {
        if (id != jobapplication.id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(jobapplication);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobApplicationExists(jobapplication.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(jobapplication);
    }

    // GET: JOBAPPLICATIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jobapplication = await _context.JobApplications
            .FirstOrDefaultAsync(m => m.id == id);
        if (jobapplication == null)
        {
            return NotFound();
        }

        return View(jobapplication);
    }

    // POST: JOBAPPLICATIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var jobapplication = await _context.JobApplications.FindAsync(id);
        if (jobapplication != null)
        {
            _context.JobApplications.Remove(jobapplication);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool JobApplicationExists(int? id)
    {
        return _context.JobApplications.Any(e => e.id == id);
    }
}
