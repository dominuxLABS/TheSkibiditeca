// Copyright (c) dominuxLABS. All rights reserved.

using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.LoanModels;

/// <summary>
/// Model used to create a loan including the loan details and the copies to be loaned.
/// </summary>
public class CreateLoanModel
{
    /// <summary>
    /// Gets or sets the loan details to create.
    /// </summary>
    public required Loan Loan { get; set; }

    /// <summary>
    /// Gets or sets the collection of copies associated with the loan.
    /// </summary>
    public required List<Copy> Copies { get; set; }
}
