namespace Feofun.Components.ComponentMessage
{
    public interface IMessageListener<TMessage>
    {
        public void OnMessage(TMessage msg);
    }
}