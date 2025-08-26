// Copyright (c) dominuxLABS. All rights reserved.

namespace TheSkibiditeca.Web.Models.ModelPartial
{
    /// <summary>
    /// Represents a card with an image, title, and description.
    /// </summary>
    /// <remarks>
    /// This model is typically used to display card-like UI elements containing an image, a title, and a description.
    /// </remarks>
    public class CardModel
    {
        /// <summary>
        /// Gets or sets the URL or path to the image displayed on the card.
        /// </summary>
        public required string Image { get; set; }

        /// <summary>
        /// Gets or sets the title text displayed on the card.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the description text displayed on the card.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the related book.
        /// </summary>
        public required int IdBook { get; set; }
    }
}
