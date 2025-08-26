// Copyright (c) dominuxLABS. All rights reserved.

using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models;
using TheSkibiditeca.Web.Models.BookModels;
using TheSkibiditeca.Web.Models.Entities;
using TheSkibiditeca.Web.Models.ModelPartial;

namespace TheSkibiditeca.Web.Controllers;

/// <summary>
/// Controller for managing book-related actions such as listing, creating,
/// viewing details, and managing the shopping cart.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BookController"/> class.
/// </remarks>
/// <param name="dbo">The library database context.</param>
/// <param name="user">The user manager for user-related operations.</param>
/// <param name="bookCart">The shopping cart instance used to manage selected copies.</param>
public class BookController(LibraryDbContext dbo, UserManager<User> user, ShoppingCart bookCart) : Controller
{
    private readonly LibraryDbContext db = dbo;
    private readonly UserManager<User> userM = user;
    private readonly ShoppingCart carro = bookCart;

    /// <summary>
    /// Redirects to the book list page.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> that redirects the client to the book list page.</returns>
    public IActionResult Index()
    {
        return this.RedirectToAction("List");
    }

    /// <summary>
    /// Displays the form to create a new book; only accessible to authorized users with sufficient role.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an IActionResult which
    /// is either the Create view when the user is authorized or a redirect to the Lost page when not.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = await this.userM.GetUserAsync(this.HttpContext.User);
        if (user == null)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        if (user.UserTypeId < 2)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        this.ViewBag.Categories = this.db.Categories.ToArray();
        this.ViewBag.Authors = this.db.Authors.ToArray();
        return this.View();
    }

    /// <summary>
    /// Handles POST requests to create a new book, its copies and author associations, and saves them to the database.
    /// </summary>
    /// <param name="bookModel">The BookCreateModel containing the book entity, number of copies and associated author ids.</param>
    /// <returns>Redirects to the Book list view after successful creation.</returns>
    [HttpPost]
    public IActionResult Create(BookCreateModel bookModel)
    {
        Book nbook = bookModel.book;
        this.db.Books.Add(nbook);
        this.db.SaveChanges();
        Book lastAdded = this.db.Books.OrderBy(e => e.BookId).LastOrDefault() ?? nbook;
        for (int i = 0; i < bookModel.Copies; i++)
        {
            this.db.Copies.Add(new Copy()
            {
                BookId = lastAdded.BookId,
                ISBN = DbSeeder.GenerateIsbn(),
                PublisherName = "GenericPublisher",
                PhysicalLocation = "GenericPosition",
                IsActive = true,
            });
        }

        foreach (string authorId in bookModel.authors)
        {
            this.db.BookAuthors.Add(new BookAuthor()
            {
                BookId = lastAdded.BookId,
                AuthorId = int.Parse(authorId, CultureInfo.InvariantCulture),
                Role = "Writer",
            });
        }

        this.db.SaveChanges();
        return this.RedirectToAction("List", "Book");
    }

    /// <summary>
    /// Displays a paginated list of books, optionally filtered by a search string.
    /// </summary>
    /// <param name="searchStr">Optional search string to filter book titles.</param>
    /// <param name="page">The 1-based page number to display.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A task that represents the asynchronous operation returning an <see cref="IActionResult"/> for the list view.</returns>
    public async Task<IActionResult> List(string? searchStr, int page = 1, int pageSize = 30)
    {
        var allBooks = new List<BookMiniCardModel>();
        foreach (Book b in this.db.Books)
        {
            allBooks.Add(new BookMiniCardModel()
            {
                BookID = b.BookId.ToString(CultureInfo.InvariantCulture),
                Title = b.Title,
                ImageURL = b.CoverImageUrl ?? string.Empty,
            });
        }

        if (!string.IsNullOrEmpty(searchStr))
        {
            allBooks = allBooks.Where(b => b.Title.Contains(searchStr, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var paginatedBooks = allBooks.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        this.ViewBag.AllBooks = paginatedBooks;
        var user = await this.userM.GetUserAsync(this.HttpContext.User);
        if (user != null)
        {
            this.ViewBag.RoleID = user.UserTypeId;
        }

        this.ViewData["CurrentPage"] = page;
        this.ViewData["CurrentFilter"] = searchStr;
        this.ViewData["PageSize"] = pageSize;
        this.ViewData["TotalPages"] = (int)Math.Ceiling(allBooks.Count / (double)pageSize);
        return this.View();
    }

    /// <summary>
    /// Displays details for a specific book identified by its id (string); if parsing fails defaults to id = 1.
    /// Populates ViewBag with book metadata, authors and available copy count for rendering the Details view.
    /// </summary>
    /// <param name="bookId">The string representation of the book identifier to show details for.</param>
    /// <returns>An <see cref="IActionResult"/> that renders the Details view for the requested book or a NotFound result when the book does not exist.</returns>
    public async Task<IActionResult> Details(string? bookId)
    {
        if (!int.TryParse(bookId, out var id))
        {
            id = 1;
        }

        var user = await this.userM.GetUserAsync(this.HttpContext.User);
        if (user != null)
        {
            this.ViewBag.RoleID = user.UserTypeId;
        }

        var c = this.db.Books.Find(id);
        if (c == null)
        {
            return this.NotFound();
        }

        // log a serialized copy for debugging (kept intentionally simple)
        var authorsIds = this.db.BookAuthors.Where(e => e.BookId == c.BookId).Select(e => e.AuthorId);
        var authorNames = this.db.Authors.Where(e => authorsIds.Contains(e.AuthorId)).Select(e => e.FullName);

        this.ViewBag.ID = bookId;
        this.ViewBag.Title = c.Title;
        this.ViewBag.Description = c.Description;
        this.ViewBag.Year = c.PublicationYear;
        this.ViewBag.Authors = string.Join(",", authorNames);
        this.ViewBag.Count = this.db.Copies.Where(e => e.BookId == c.BookId && e.IsActive == true).Count();
        this.ViewBag.URL = c.CoverImageUrl;
        return this.View();
    }

    /// <summary>
    /// Adds an available copy of the specified book to the shopping cart and redirects to the appropriate page.
    /// </summary>
    /// <param name="bookId">The identifier of the book to add.</param>
    /// <param name="once">If true, redirect to Loan creation after adding; otherwise return to book details.</param>
    /// <returns>An IActionResult redirecting to the Details or Create/Loan action.</returns>
    public IActionResult AddCart(string bookId, bool once = false)
    {
        var bookIdInt = int.Parse(bookId, CultureInfo.InvariantCulture);
        var allCopies = this.db.Copies.Where(e => e.BookId == bookIdInt);
        var aviableCopy = this.db.Copies.Where(e => e.BookId == bookIdInt && !this.carro.copies.Select(c => c.ISBN).Contains(e.ISBN));
        if (this.carro.copies.Count > allCopies.Count())
        {
            return this.RedirectToAction("Details", "Book", new { bookId });
        }

        if (aviableCopy != null && aviableCopy.Any())
        {
            this.carro.copies.Add(aviableCopy.First());
        }

        if (once)
        {
            return this.RedirectToAction("Create", "Loan");
        }

        return this.RedirectToAction("Details", "Book", new { bookId });
    }

    /// <summary>
    /// Removes a copy of the specified book from the shopping cart and redirects to the Loan creation page.
    /// </summary>
    /// <param name="bookId">The identifier of the book to remove from the cart.</param>
    /// <returns>An <see cref="IActionResult"/> that redirects to the Loan creation action.</returns>
    public IActionResult RemoveCart(string bookId)
    {
        if (this.carro.copies.Count == 0)
        {
            return this.RedirectToAction("Create", "Loan");
        }

        var copiesIncar = this.carro.copies.FirstOrDefault(e => e.BookId == int.Parse(bookId, CultureInfo.InvariantCulture));
        if (copiesIncar != null)
        {
            this.carro.copies.Remove(copiesIncar);
        }

        return this.RedirectToAction("Create", "Loan");
    }
}
