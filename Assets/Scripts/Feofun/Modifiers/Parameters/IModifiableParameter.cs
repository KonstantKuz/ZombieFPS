namespace Feofun.Modifiers.Parameters
{
    public interface IModifiableParameter
    {
        string Name { get; }
        void Reset();
    }
}