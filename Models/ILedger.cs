using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public interface ILedger
{
    Guid Id { get; set; }
    string Name { get; set; }
    DateTime Created { get; set; }
    DateTime LastUpdated { get; set; }
    List<IShare> Shares { get; set; }
    List<AppUser> Users { get; set; }
    ShareType DefaultShareType { get; set; }
    ICurrency DefaultCurrency { get; set; }
    bool CalculateShares();
    bool CalculateDebts();
    bool AddUser(AppUser user);
    bool AddShare(IShare share);
}
