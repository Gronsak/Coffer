using Coffer.Data;
using Coffer.Models;

namespace Coffer.Services;

public interface ITreasurer
{
    Ledger CreateLedger();
    List<Ledger> GetLedgers();
    List<Ledger> GetLedgers(AppUser owner);
    Ledger GetLedger(Guid id);
    Ledger GetLedger(string name);
    bool SaveLedger(Ledger ledger);
    bool DeleteLedger(Ledger ledger);
}