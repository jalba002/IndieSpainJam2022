public interface IShopDescription<T>
{
    string InitialValue(T data);
    string FinalValue(T data);
}