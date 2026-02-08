using System.Security.Claims;
using Coffer.Data;
using Coffer.Models;

namespace Coffer.Services;

public interface ITreasurer
{
    Task<bool> CreateLedgerAsync(string name, AppUser owner, Currency defaultCurrency, string description = "", ShareType defaultType = ShareType.Shares);
    Task<List<Ledger>> GetLedgersAsync();
    Task<List<Ledger>> GetLedgersAsync(AppUser owner);
    Task<List<Ledger>> GetLedgersByMemeberAsync(AppUser member);
    
    Task<List<Ledger>> GetLedgersAsync(ClaimsPrincipal owner);
    Task<List<Ledger>> GetLedgersByMemeberAsync(ClaimsPrincipal member);
    Task<Ledger?> GetLedgerAsync(Guid ledgerId);
    Task<Ledger?> GetLedgerAsync(string name);
    Task<bool> UpdateLedgerAsync(Ledger ledger);
    Task<bool> DeleteLedgerAsync(Ledger ledger);
}