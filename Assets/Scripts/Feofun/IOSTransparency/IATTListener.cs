using System;

namespace Feofun.IOSTransparency
{
    public interface IATTListener
    { 
        event Action OnStatusReceived;
        void Init();
    }
}