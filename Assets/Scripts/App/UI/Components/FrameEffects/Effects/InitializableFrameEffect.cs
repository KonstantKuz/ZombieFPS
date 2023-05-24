using Feofun.UI.Components;

namespace App.UI.Components.FrameEffects.Effects
{
    public abstract class InitializableFrameEffect<TInitModel> : FrameEffect, IUiInitializable<TInitModel>
    {
        public abstract void Init(TInitModel model);
    }
}