namespace Feofun.Components
{
    public interface IInitializable<in T>
    {
        public void Init(T data);
    }
}