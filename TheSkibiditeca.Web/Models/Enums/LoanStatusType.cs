// Copyright (c) dominuxLABS. All rights reserved.

namespace TheSkibiditeca.Web.Models.Enums
{
    /// <summary>
    /// Represents the status of a loan in the library system.
    /// </summary>
    public enum LoanStatusType
    {
        /// <summary>
        /// The loan is currently active.
        /// </summary>
        Active = 1,

        /// <summary>
        /// The book has been returned.
        /// </summary>
        Returned = 2,

        /// <summary>
        /// The loan is past the due date.
        /// </summary>
        Overdue = 3,

        /// <summary>
        /// The loan has been renewed.
        /// </summary>
        Renewed = 4,

        /// <summary>
        /// The book has been reported as lost.
        /// </summary>
        Lost = 5,
    }
}
