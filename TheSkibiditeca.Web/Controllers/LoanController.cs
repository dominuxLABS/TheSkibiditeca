// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models;
using TheSkibiditeca.Web.Models.Entities;
using TheSkibiditeca.Web.Models.Enums;
using TheSkibiditeca.Web.Models.LoanModels;

namespace TheSkibiditeca.Web.Controllers;

/// <summary>
/// Controller responsible for managing loans, including creating, editing, listing, and deleting loan records.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LoanController"/> class.
/// </remarks>
/// <param name="context">The <see cref="LibraryDbContext"/> instance used to access the database.</param>
/// <param name="cart">The <see cref="ShoppingCart"/> instance representing the current user's shopping cart.</param>
/// <param name="user">The <see cref="UserManager{User}"/> instance used to manage users.</param>
public class LoanController(LibraryDbContext context, ShoppingCart cart, UserManager<User> user) : Controller
{
    private readonly LibraryDbContext context = context;
    private readonly ShoppingCart shoppingCart = cart;
    private readonly UserManager<User> userManager = user;

    /// <summary>
    /// Redirects to the list of loans.
    /// </summary>
    /// <returns>An IActionResult that redirects to the Loans list action.</returns>
    public IActionResult Index()
    {
        return this.RedirectToAction("List", "Loan");
    }

    /// <summary>
    /// Displays the details of a specific loan for the authenticated user.
    /// </summary>
    /// <param name="loanId">The id of the loan to display.</param>
    /// <returns>An IActionResult that renders the details view or redirects if the user is not authenticated or the loan does not exist.</returns>
    public async Task<IActionResult> Details(int? loanId)
    {
        var user = await this.userManager.GetUserAsync(this.HttpContext.User);
        if (user == null)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        if (loanId is null)
        {
            return this.RedirectToAction("Index", "Home");
        }

        var currentLoan = this.context.Loans.Find(loanId);
        if (currentLoan is null)
        {
            return this.RedirectToAction("Index", "Home");
        }

        if (user.Id != currentLoan.UserId)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        var loanDetails = this.context.LoanDetails.Where(e => e.LoanId == currentLoan.LoanId).ToList();

        this.ViewBag.Loan = currentLoan;
        this.ViewBag.LoanDetails = loanDetails;
        this.ViewBag.Copies = new List<Copy>();
        this.ViewBag.Books = new List<Book>();

        foreach (LoanDetails ld in loanDetails)
        {
            var copy = this.context.Copies.Find(ld.CopyId);
            if (copy != null)
            {
                this.ViewBag.Copies.Add(copy);
                var book = this.context.Books.Find(copy.BookId);
                this.ViewBag.Books.Add(book);
            }
        }

        return this.View();
    }

    /// <summary>
    /// Displays the Create loan view for the authenticated user and populates the shopping cart copies for display.
    /// </summary>
    /// <returns>An IActionResult that renders the create loan view or redirects if the user is not authenticated.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = await this.userManager.GetUserAsync(this.HttpContext.User);
        if (user is null)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        this.ViewBag.User = user.FirstName;
        var copies = this.shoppingCart.copies ?? new List<Copy>();
        this.ViewBag.ShoopingBooks = copies;
        foreach (Copy c in copies)
        {
            c.Book = this.context.Books.Find(c.BookId);
        }

        return this.View();
    }

    /// <summary>
    /// Shows the list of loans for the currently authenticated user.
    /// </summary>
    /// <returns>An IActionResult that renders the view displaying the user's loans or redirects if the user is not authenticated.</returns>
    public async Task<IActionResult> List()
    {
        var user = await this.userManager.GetUserAsync(this.HttpContext.User);
        if (user is null)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        this.ViewBag.Loans = this.context.Loans.Where(e => e.UserId == user.Id).ToList();
        return this.View();
    }

    /// <summary>
    /// Creates a new loan from the provided model, updates copy availability and loan details, and redirects to the loan details page.
    /// </summary>
    /// <param name="model">The CreateLoanModel containing the copies and loan information to create.</param>
    /// <returns>An IActionResult that redirects to the details view of the created loan or to an error/redirect action if the user is not authenticated.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLoanModel model)
    {
        var user = await this.userManager.GetUserAsync(this.HttpContext.User);
        if (user == null)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        model.Copies = this.shoppingCart.copies;
        Loan nLoan = new()
        {
            UserId = user.Id,
            Status = 0,
            LoanDate = model.Loan.LoanDate,
            ExpectedReturnDate = model.Loan.ExpectedReturnDate,
            RenewalsCount = 0,
            MaxRenewals = 5,
        };
        model.Loan = nLoan;
        this.context.Loans.Add(nLoan);
        this.context.SaveChanges();

        Loan currentLoan = this.context.Loans.Where(e => e.UserId == nLoan.UserId).OrderByDescending(x => x.LoanDate).First();
        foreach (Copy c in model.Copies)
        {
            Copy? db_book = this.context.Copies.Find(c.CopyId);
            if (db_book is not null)
            {
                db_book.IsActive = false;
            }

            LoanDetails detail = new()
            {
                LoanId = currentLoan.LoanId,
                CopyId = c.CopyId,
                Quantity = model.Copies.Count,
                DateAdded = DateTime.Now,
            };
            this.context.LoanDetails.Add(detail);
        }

        this.context.SaveChanges();
        this.shoppingCart.copies.Clear();
        return this.RedirectToAction("Details", "Loan", new { loanid = currentLoan.LoanId });
    }

    /// <summary>
    /// Displays the edit form for the specified loan.
    /// </summary>
    /// <param name="loanid">The id of the loan to edit.</param>
    /// <returns>An IActionResult that renders the edit view or NotFound if the loan does not exist or the user is not authenticated.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int? loanid)
    {
        var user = await this.userManager.GetUserAsync(this.HttpContext.User);
        if (user == null)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        this.ViewBag.User = user.FullName;
        if (loanid == null)
        {
            return this.NotFound();
        }

        var loan = await this.context.Loans.FindAsync(loanid);
        if (loan == null)
        {
            return this.NotFound();
        }

        var model = new EditLoanModel()
        {
            Loan = loan,
        };
        return this.View(model);
    }

    /// <summary>
    /// Updates the specified loan with values from the provided model; when the loan is marked as returned,
    /// sets the actual return date and reactivates the associated copies.
    /// </summary>
    /// <param name="model">The EditLoanModel containing the updated loan data.</param>
    /// <returns>An IActionResult that redirects to the loan list on success or returns NotFound on concurrency errors.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditLoanModel model)
    {
        try
        {
            if (model.Loan.Status == LoanStatusType.Returned)
            {
                model.Loan.ActualReturnDate = DateTime.Now;
                var details = this.context.LoanDetails.Where(e => e.LoanId == model.Loan.LoanId);
                foreach (LoanDetails item in details)
                {
                    var copy = this.context.Copies.Find(item.CopyId);
                    if (copy is not null)
                    {
                        copy.IsActive = true;
                    }
                }
            }

            this.context.Update(model.Loan);
            await this.context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.LoanExists(model.Loan.LoanId))
            {
                return this.NotFound();
            }
            else
            {
                throw;
            }
        }

        return this.RedirectToAction("List", "Loan");
    }

    /// <summary>
    /// Displays the delete confirmation view for the specified loan.
    /// </summary>
    /// <param name="loanid">The id of the loan to delete.</param>
    /// <returns>An IActionResult that renders the delete confirmation view or NotFound if the loan does not exist.</returns>
    public async Task<IActionResult> Delete(int? loanid)
    {
        if (loanid == null)
        {
            return this.NotFound();
        }

        var loan = await this.context.Loans
            .FirstOrDefaultAsync(m => m.LoanId == loanid);
        if (loan == null)
        {
            return this.NotFound();
        }

        return this.View(loan);
    }

    /// <summary>
    /// Displays an administrative list of all loans.
    /// </summary>
    /// <returns>An IActionResult that renders the administrative loans view.</returns>
    public async Task<IActionResult> Admin()
    {
        this.ViewBag.Loans = await this.context.Loans.ToListAsync();
        return this.View();
    }

    /// <summary>
    /// Deletes the specified loan and redirects to the loan index.
    /// </summary>
    /// <param name="loanid">The id of the loan to delete.</param>
    /// <returns>A redirection to the Index action.</returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? loanid)
    {
        var loan = await this.context.Loans.FindAsync(loanid);

        if (loan != null)
        {
            this.context.Loans.Remove(loan);
        }

        await this.context.SaveChangesAsync();
        return this.RedirectToAction(nameof(this.Index));
    }

    private bool LoanExists(int? loanid)
    {
        return this.context.Loans.Any(e => e.LoanId == loanid);
    }
}
