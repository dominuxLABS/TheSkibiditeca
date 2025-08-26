using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models;
using TheSkibiditeca.Web.Models.Entities;
using TheSkibiditeca.Web.Models.Enums;
using TheSkibiditeca.Web.Models.LoanModels;

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
        return RedirectToAction("List", "Loan");
    }

    public async Task<IActionResult> Details(int? loanid) {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if(user == null) return RedirectToAction("Lost", "Home");
        if(loanid == null) return RedirectToAction("Index", "Home");
        var currentLoan = _context.Loans.Find(loanid);
        if(user.Id != currentLoan.UserId) return RedirectToAction("Lost", "Home");
        if(currentLoan == null) return RedirectToAction("Index", "Home");
        var loanDetails = _context.LoanDetails.Where(e => e.LoanId == currentLoan.LoanId).ToList();

        ViewBag.Loan = currentLoan;
        ViewBag.LoanDetails = loanDetails;
        ViewBag.Copies = new List<Copy>();
        ViewBag.Books = new List<Book>();

        foreach(LoanDetails ld in loanDetails) {
            var copy = _context.Copies.Find(ld.CopyId);
            if(copy != null) {
                ViewBag.Copies.Add(copy);
                var book = _context.Books.Find(copy.BookId);
                ViewBag.Books.Add(book);
            }
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.User = (await _userManager.GetUserAsync(HttpContext.User)).FirstName;
        ViewBag.ShoopingBooks = shoppingCart.copies;
        foreach(Copy c in shoppingCart.copies) {
            c.Book = _context.Books.Find(c.BookId);
        }

        return View();
    }

    public async Task<IActionResult> List() {
        var user = await this._userManager.GetUserAsync(this.HttpContext.User);
        if (user == null) {
            return this.RedirectToAction("Lost", "Home");
        }

        ViewBag.Loans = _context.Loans.Where(e => e.UserId == user.Id).ToList();
        return View();
    }

    // POST: LOANS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLoanModel model)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        model.copies = shoppingCart.copies;
        Loan nLoan = new() {
            UserId = user.Id,
            Status = 0,
            LoanDate = model.loan.LoanDate,
            ExpectedReturnDate = model.loan.ExpectedReturnDate,
            RenewalsCount = 0,
            MaxRenewals = 5,
        };
        model.loan = nLoan;
        _context.Loans.Add(nLoan);
        _context.SaveChanges();

        Loan currentLoan = _context.Loans.Where(e => e.UserId == nLoan.UserId).OrderByDescending(x => x.LoanDate).First();
        foreach (Copy c in model.copies) {
            Copy db_book = _context.Copies.Find(c.CopyId);
            db_book.IsActive = false;

            LoanDetails detail = new() {
                LoanId = currentLoan.LoanId,
                CopyId = c.CopyId,
                Quantity = model.copies.Count,
                DateAdded = DateTime.Now,
            };
            _context.LoanDetails.Add(detail);
        }

        _context.SaveChanges();
        return RedirectToAction("Details", "Loan", new { loanid = currentLoan.LoanId});
    }

    // GET: LOANS/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? loanid)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        ViewBag.User = user.FullName;
        if (loanid == null)
        {
            return NotFound();
        }

        var loan = await _context.Loans.FindAsync(loanid);
        if (loan == null)
        {
            return NotFound();
        }

        var model = new EditLoanModel() {
            loan = loan
        };
        return View(model);
    }

    // POST: LOANS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditLoanModel model)
    {
        try
        {
            if(model.loan.Status == LoanStatusType.Returned) {
                model.loan.ActualReturnDate = DateTime.Now;
                var details = _context.LoanDetails.Where(e => e.LoanId == model.loan.LoanId);
                foreach(LoanDetails item in details) {
                    var copy = _context.Copies.Find(item.CopyId);
                    copy.IsActive = true;
                }
            }

            _context.Update(model.loan);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LoanExists(model.loan.LoanId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        
        return RedirectToAction("List", "Loan");
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

    public async Task<IActionResult> Admin() {
        ViewBag.Loans = _context.Loans.ToList();
        return View();
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
