namespace VirtualCreditCard.Core.Service.Interfaces
{
    public interface IStorageService
    {
        void Set<T>(string key, T value);
        T? TryGet<T>(string key) where T : class;
    }
}
