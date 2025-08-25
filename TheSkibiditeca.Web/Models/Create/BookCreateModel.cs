using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.Create {
    public class BookCreateModel {
        public Book book { get; set; }
        public List<string> authors { get; set; }
        public int Copies { get; set; }
    }
}
