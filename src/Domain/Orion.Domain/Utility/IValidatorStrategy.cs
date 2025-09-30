namespace Orion.Domain.Utility
{
    public interface IValidatorStrategy<T>
    {
        bool IsValid(T validateThis);
    }
}
