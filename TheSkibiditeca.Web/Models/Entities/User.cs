// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a user in the library system.
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user code (student ID, employee ID, etc.).
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the user's address.
        /// </summary>
        [StringLength(300)]
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the user type identifier.
        /// </summary>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the career or department.
        /// </summary>
        [StringLength(150)]
        public string? CareerDepartment { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow.Date;

        /// <summary>
        /// Gets or sets the membership expiration date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? MembershipExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the user type navigation property.
        /// </summary>
        public virtual UserType UserType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the loans made by this user.
        /// </summary>
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

        /// <summary>
        /// Gets or sets the reservations made by this user.
        /// </summary>
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        /// <summary>
        /// Gets or sets the fines associated with this user.
        /// </summary>
        public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        [NotMapped]
        public string FullName => $"{this.FirstName} {this.LastName}";

        /// <summary>
        /// Gets a value indicating whether the user's membership is active.
        /// </summary>
        [NotMapped]
        public bool IsMembershipActive => this.MembershipExpirationDate == null || this.MembershipExpirationDate > DateTime.UtcNow.Date;
    }
}
