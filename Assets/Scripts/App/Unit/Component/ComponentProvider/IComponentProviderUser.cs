
namespace App.Unit.Component.ComponentProvider
{
    public interface IComponentProviderUser<T>
    {
        void SetProvider(IComponentProvider<T> componentProvider);
        
    }
}