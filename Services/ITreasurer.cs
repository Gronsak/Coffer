using System.Security.Claims;
using Coffer.Data;
using Coffer.Models;

namespace Coffer.Services;

public interface ITreasurer
{
    Task<Ledger?> CreateLedgerAsync(string name, AppUser owner, Currency defaultCurrency, string description = "", ShareType defaultType = ShareType.Shares);
    Task<List<Ledger>?> GetLedgersAsync();
    Task<List<Ledger>?> GetLedgersAsync(AppUser owner);
    Task<List<Ledger>?> GetLedgersByMemeberAsync(AppUser member);
    Task<List<Ledger>?> GetLedgersAsync(ClaimsPrincipal owner);
    Task<List<Ledger>?> GetLedgersByOwnerIdAsync(string ownerId);
    Task<List<Ledger>?> GetLedgersByMemeberAsync(ClaimsPrincipal member);
    Task<List<Ledger>?> GetLedgersByMemeberIdAsync(string memberId);
    Task<Ledger?> GetLedgerAsync(Guid ledgerId);
    Task<Ledger?> GetLedgerAsync(string name);
    Task<Ledger?> UpdateLedgerAsync(Ledger ledger);
    Task<bool> DeleteLedgerAsync(Ledger ledger);
    Task<List<Currency>?> GetCurrenciesAsync();
}