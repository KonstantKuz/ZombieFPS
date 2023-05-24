using App.Unit.Component.Death;
using Feofun.Components;
using Feofun.Components.ComponentMessage;

namespace App.MiniMap.Component
{
    public class UnitMapMarker : MiniMapMarker, IInitializable<Unit.Unit>, IMessageListener<UnitDeathComponentMessage>
    {
        public void Init(Unit.Unit data)
        {
            AddToMap();
        }

        public void OnMessage(UnitDeathComponentMessage msg)
        {
            RemoveFromMap();
        }
    }
}