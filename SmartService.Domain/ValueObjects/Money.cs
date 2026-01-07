using SmartService.Domain.Exceptions;

namespace SmartService.Domain.ValueObjects
{
    public sealed class Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        private Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new DomainException("Amount cannot be negative.");

            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency = "USD")
            => new(amount, currency);
    }
}