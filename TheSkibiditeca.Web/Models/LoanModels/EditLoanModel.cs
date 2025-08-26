// Copyright (c) dominuxLABS. All rights reserved.

using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.LoanModels;

/// <summary>
/// Represents the model used to edit a loan, including the loan itself and related copies and books.
/// </summary>
public class EditLoanModel
{
    /// <summary>
    /// Gets or sets the loan being edited.
    /// </summary>
    public Loan Loan { get; set; } = null!;

    /// <summary>
    /// Gets or sets the list of copies associated with the loan.
    /// </summary>
    public List<Copy> Copies { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of books associated with the loan.
    /// </summary>
    public List<Book> Books { get; set; } = [];
}
