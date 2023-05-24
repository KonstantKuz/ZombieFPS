using JetBrains.Annotations;

namespace App.Unit.Component.Target
{
    public class CachedTargetProvider : ITargetProvider
    {
        private readonly ITargetProvider _providerImpl;
        private ITarget _target;

        public CachedTargetProvider(ITargetProvider targetProvider)
        {
            _providerImpl = targetProvider;
        }
        
        [CanBeNull] 
        public ITarget Target
        {
            get
            {
                if (_target == null)
                {
                    Target = _providerImpl.Target;
                }

                return _target;
            }
            private set
            {
                if (_target == value) return;
                if (_target != null)
                {
                    _target.OnTargetInvalid -= ClearTarget;
                }
                _target = value;
                if (_target != null)
                {
                    _target.OnTargetInvalid += ClearTarget;
                }
            }
        }

        private void ClearTarget() => Target = null;
    }
}