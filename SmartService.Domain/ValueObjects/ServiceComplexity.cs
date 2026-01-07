using SmartService.Domain.Exceptions;

namespace SmartService.Domain.ValueObjects
{
    public sealed class ServiceComplexity
    {
        public int Level { get; }

        private ServiceComplexity(int level)
        {
            if (level < 1 || level > 5)
                throw new DomainException("Service complexity must be between 1 and 5.");

            Level = level;
        }

        public static ServiceComplexity From(int level)
            => new(level);
    }
}