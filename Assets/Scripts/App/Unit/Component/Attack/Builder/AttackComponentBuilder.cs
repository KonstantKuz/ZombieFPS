using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Animation;
using App.Unit.Component.Attack.Animation;
using App.Unit.Component.Attack.Condition;
using App.Unit.Component.Attack.Damager;
using App.Unit.Component.Attack.Timer;
using App.Unit.Component.Attack.WeaponWrapper;
using App.Unit.Component.ComponentProvider;
using Feofun.Components;
using Feofun.Extension;
using Feofun.Util.Timer;
using SuperMaxim.Core.Extensions;
using UnityEngine.Assertions;

namespace App.Unit.Component.Attack.Builder
{
    public class AttackComponentBuilder : IComponentBuilder<IAttackComponent>
    {
        private readonly AttackComponentInitData _initData;
        private readonly ComponentCollectionProvider<IAttackComponent> _componentCollectionProvider;

        public AttackComponentBuilder(AttackComponentInitData initData)
        {
            _initData = initData;
            _componentCollectionProvider = new ComponentCollectionProvider<IAttackComponent>();
        }

        public IComponentProvider<IAttackComponent> Build(params Type[] requiredTypes)
        {
            CheckForRequiredTypes(requiredTypes);
            
            var componentProvider = new ComponentProvider<IAttackComponent>();
            _componentCollectionProvider.Types.ForEach(type => componentProvider.Add(type, BuildComponent(type)));
            return componentProvider;
        }

        private void CheckForRequiredTypes(IEnumerable<Type> requiredTypes)
        {
            requiredTypes.ForEach(type => Assert.IsNotNull(_componentCollectionProvider.Get(type)));
        }

        private IAttackComponent BuildComponent(Type buildType)
        {
            return buildType != typeof(IAttackCondition) ? _componentCollectionProvider.Get(buildType).First() : BuildAttackCondition(buildType);
        }
        private IAttackComponent BuildAttackCondition(Type buildType)
        {
            var conditionComposite = new AttackConditionComposite();
            var conditions = _componentCollectionProvider.Get(buildType).Cast<IAttackCondition>().ToList();
            conditionComposite.AddConditions(conditions);
            return conditionComposite;
        }

        public void Register<T>(IAttackComponent component) where T : IAttackComponent
        {
            RegisterToProvider<T>(component);
        }

        public void Register(IAttackComponent component)
        {
            switch (component)
            {
                case IAttackAnimation _:
                    RegisterAnimation(component);
                    break;
                case ITimer _:
                    RegisterToProvider<AttackIntervalTimer>(component);
                    break;
                case IWeaponWrapper _:
                    RegisterToProvider<IWeaponWrapper>(component);
                    break;
                case IDamager _:
                    RegisterToProvider<IDamager>(component);
                    break;
                case IAttackCondition _:
                    RegisterToProvider<IAttackCondition>(component);
                    break;
                default:
                    RegisterUnknownClassComponent(component);
                    break;
            }
        }

        private void RegisterUnknownClassComponent(IAttackComponent component)
        {
            var methodInfo = GetType().GetMethod(nameof(RegisterToProvider), BindingFlags.NonPublic | BindingFlags.Instance);
            var method = methodInfo.MakeGenericMethod(component.GetType());
            method.Invoke(this, new object[] {component});
        }

        private void RegisterAnimation(IAttackComponent attackAnimation)
        {
            var animationComponent = _initData.AttackRoot.gameObject.HasComponentInChildren<AnimationEventHandler>()
                ? attackAnimation
                : new EmptyAttackAnimation();
            RegisterToProvider<IAttackAnimation>(animationComponent);
        }
        private void RegisterToProvider<T>(IAttackComponent component) where T : IAttackComponent
        {
            InitComponent(component);
            _componentCollectionProvider.Add<T>(component);
            if (typeof(T) == typeof(IAttackCondition)) {
                return;
            }
            if (component is IAttackCondition) {
                _componentCollectionProvider.Add<IAttackCondition>(component);
            }
        }

        private void InitComponent(IAttackComponent component)
        {
            if (component is IInitializable<AttackComponentInitData> initializable) {
                initializable.Init(_initData);
            }
        }
    }
    
}