using Coffer.Data;
using Coffer.Models;

namespace Coffer.Services;

public class Treasurer : ITreasurer
{
    public Ledger CreateLedger()
    {
        throw new NotImplementedException();
    }

    public bool DeleteLedger(Ledger ledger)
    {
        throw new NotImplementedException();
    }

    public Ledger GetLedger(Guid id)
    {
        throw new NotImplementedException();
    }

    public Ledger GetLedger(string name)
    {
        throw new NotImplementedException();
    }

    public List<Ledger> GetLedgers()
    {
        throw new NotImplementedException();
    }

    public List<Ledger> GetLedgers(AppUser owner)
    {
        throw new NotImplementedException();
    }

    public bool SaveLedger(Ledger ledger)
    {
        throw new NotImplementedException();
    }
}