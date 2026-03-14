using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
        {
            throw new DomainException($"Amount cannot be negative {amount}");
        }

        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money money)
    {
        if (Currency != money.Currency)
            throw new DomainException("Cannot add money with different currencies");
        return new Money(Amount + money.Amount, Currency);
    }

    public Money Multiply(int qty) => new (Amount * qty, Currency);
    
    public static Money operator +(Money a, Money b) => a.Add(b);
    public static bool operator ==(Money a, Money b) => a.Equals(b);
    public static bool operator !=(Money a, Money b) => !a.Equals(b);

    public bool Equals(Money? other) =>
        other is not null && Amount == other.Amount && Currency == other.Currency;
    public override bool Equals(object? obj) => Equals(obj as Money);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Currency} {Amount:N2}";
}
