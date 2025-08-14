// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents an audit log entry in the library system.
    /// </summary>
    [Table("AuditLogs")]
    public class AuditLog
    {
        /// <summary>
        /// Gets or sets the audit log identifier.
        /// </summary>
        [Key]
        public int AuditLogId { get; set; }

        /// <summary>
        /// Gets or sets the affected table name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the affected record identifier.
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// Gets or sets the action performed.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Action { get; set; } = string.Empty; // INSERT, UPDATE, DELETE

        /// <summary>
        /// Gets or sets the old data before the change.
        /// </summary>
        [Column(TypeName = "text")]
        public string? OldData { get; set; }

        /// <summary>
        /// Gets or sets the new data after the change.
        /// </summary>
        [Column(TypeName = "text")]
        public string? NewData { get; set; }

        /// <summary>
        /// Gets or sets the system user who made the change.
        /// </summary>
        [StringLength(100)]
        public string? SystemUser { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the change.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the IP address from which the change was made.
        /// </summary>
        [StringLength(45)]
        public string? IpAddress { get; set; }
    }
}
