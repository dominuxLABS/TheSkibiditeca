using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.LoanModels {
    public class EditLoanModel {
        public Loan loan { get; set; }
        public List<Copy> copies { get; set; }
        public List<Book> books { get; set; }
    }
}
