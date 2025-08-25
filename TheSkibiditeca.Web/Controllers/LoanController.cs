
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models;
using TheSkibiditeca.Web.Models.Entities;

public class LoanController : Controller
{
    private readonly LibraryDbContext _context;
    private readonly ShoppingCart shoppingCart;
    private readonly UserManager<User> _userManager;

    public LoanController(LibraryDbContext context, ShoppingCart cart, UserManager<User> user)
    {
        _context = context;
        shoppingCart = cart;
        _userManager = user;
    }

    // GET: LOANS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Loans.ToListAsync());
    }

    // GET: LOANS/Details/5
    public async Task<IActionResult> Details(int? loanid)
    {
        if (loanid == null)
        {
            return NotFound();
        }

        var loan = await _context.Loans
            .FirstOrDefaultAsync(m => m.LoanId == loanid);
        if (loan == null)
        {
            return NotFound();
        }

        return View(loan);
    }

    // GET: LOANS/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.User = (await _userManager.GetUserAsync(HttpContext.User)).FirstName;
        ViewBag.ShoopingBooks = shoppingCart.books;
        return View();
    }

    // POST: LOANS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID,Title,ReleaseDate,Genre,Price")] Loan loan)
    {
        if (ModelState.IsValid)
        {
            _context.Add(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(loan);
    }

    // GET: LOANS/Edit/5
    public async Task<IActionResult> Edit(int? loanid)
    {
        if (loanid == null)
        {
            return NotFound();
        }

        var loan = await _context.Loans.FindAsync(loanid);
        if (loan == null)
        {
            return NotFound();
        }
        return View(loan);
    }

    // POST: LOANS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? loanid, [Bind("ID,Title,ReleaseDate,Genre,Price")] Loan loan)
    {
        if (loanid != loan.LoanId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(loan);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(loan.LoanId))
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
        return View(loan);
    }

    // GET: LOANS/Delete/5
    public async Task<IActionResult> Delete(int? loanid)
    {
        if (loanid == null)
        {
            return NotFound();
        }

        var loan = await _context.Loans
            .FirstOrDefaultAsync(m => m.LoanId == loanid);
        if (loan == null)
        {
            return NotFound();
        }

        return View(loan);
    }

    // POST: LOANS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? loanid)
    {
        var loan = await _context.Loans.FindAsync(loanid);
        if (loan != null)
        {
            _context.Loans.Remove(loan);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LoanExists(int? loanid)
    {
        return _context.Loans.Any(e => e.LoanId == loanid);
    }
}
