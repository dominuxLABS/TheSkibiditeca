using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.LoanModels {
    public class CreateLoanModel {
        public Loan loan { get; set; }
        public List<Copy> copies { get; set; }
    }
}
