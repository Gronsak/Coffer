using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public interface ILedger
{
    Guid Id { get; set; }
    string Name { get; set; }
    DateTime Created { get; set; }
    DateTime LastUpdated { get; set; }
    List<Share> Shares { get; set; }
    List<AppUser> Users { get; set; }
    List<Cost> Costs { get; set; }
    ShareType DefaultShareType { get; set; }
    Currency DefaultCurrency { get; set; }
    bool CalculateShares();
    bool CalculateDebts();
    bool AddUser(AppUser user);
    bool AddShare(Share share);
}
