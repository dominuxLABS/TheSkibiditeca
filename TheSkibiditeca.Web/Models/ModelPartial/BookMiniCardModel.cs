// Copyright (c) dominuxLABS. All rights reserved.

namespace TheSkibiditeca.Web.Models.ModelPartial
{
    /// <summary>
    /// Lightweight model used to render a small card for a book (image, title and id) in lists or partial views.
    /// </summary>
    public class BookMiniCardModel
    {
        /// <summary>
        /// Gets or sets the URL of the book cover image.
        /// </summary>
        public required string ImageURL { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the book identifier as a string (used for links/routes).
        /// </summary>
        public required string BookID { get; set; }
    }
}
