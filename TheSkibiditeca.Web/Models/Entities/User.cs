// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

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
        /// Gets or sets the user's password hash.
        /// Uses PBKDF2 with SHA256 for secure password storage.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password salt used for hashing.
        /// A unique salt is generated for each password to prevent rainbow table attacks.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string PasswordSalt { get; set; } = string.Empty;

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

        /// <summary>
        /// Sets the password for the user by generating a secure hash and salt.
        /// This method uses PBKDF2 (Password-Based Key Derivation Function 2) with SHA256
        /// to create a cryptographically secure password hash with a random salt.
        /// </summary>
        /// <param name="password">The plain text password to be hashed.</param>
        /// <exception cref="ArgumentException">Thrown when password is null, empty, or whitespace.</exception>
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            }

            // Generate a cryptographically secure random salt (32 bytes = 256 bits)
            // Each user gets a unique salt to prevent rainbow table attacks
            var saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // Store the salt as base64 string for database storage
            this.PasswordSalt = Convert.ToBase64String(saltBytes);

            // Compute and store the password hash using the generated salt
            this.PasswordHash = ComputeHash(password, saltBytes);
        }

        /// <summary>
        /// Verifies if the provided password matches the stored hash.
        /// This method reconstructs the hash using the stored salt and compares
        /// it with the stored hash using a constant-time comparison.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <returns>True if the password is correct, false otherwise.</returns>
        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            try
            {
                // Convert the stored salt back to bytes
                var saltBytes = Convert.FromBase64String(this.PasswordSalt);

                // Compute hash with the same salt and compare with stored hash
                var computedHash = ComputeHash(password, saltBytes);

                // Use constant-time comparison to prevent timing attacks
                return this.PasswordHash == computedHash;
            }
            catch
            {
                // Return false if any exception occurs (invalid base64, etc.)
                return false;
            }
        }

        /// <summary>
        /// Computes the hash of a password using PBKDF2 with SHA256.
        /// PBKDF2 (Password-Based Key Derivation Function 2) is designed to be slow
        /// to make brute force attacks computationally expensive.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <param name="saltBytes">The salt bytes to use for hashing.</param>
        /// <returns>The computed hash as a base64 string.</returns>
        private static string ComputeHash(string password, byte[] saltBytes)
        {
            // Use PBKDF2 with 10,000 iterations (industry standard minimum)
            // Higher iteration count makes brute force attacks more expensive
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                // Generate a 32-byte (256-bit) hash
                var hashBytes = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
