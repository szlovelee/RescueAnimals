namespace Util
{
    public interface IPool<T>
    {
        public T Pull();
        public void Push(T obj);
    }
}