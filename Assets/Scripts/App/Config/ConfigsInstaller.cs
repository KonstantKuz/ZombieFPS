using App.Advertisment.Config;
using App.BattlePass.Config;
using App.Booster.Config;
using App.Config.Configs;
using App.Enemy.Config;
using App.Enemy.Dismemberment.Config;
using App.InteractableItems.Config;
using App.Items.Config;
using App.Level.Config;
using App.MainCamera.Config;
using App.MiniMap.Config;
using App.Player.Config;
using App.Player.Config.Animation;
using App.Player.Config.Attack;
using App.Player.Config.StateMachine;
using App.Vibration;
using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using UnityEngine;
using Zenject;

namespace App.Config
{
    public class ConfigsInstaller : MonoBehaviour
    {
        [SerializeField] private CameraShakeConfig _cameraShakeConfig;
        [SerializeField] private PromoRecordSettings _promoRecordSettings;    
        [SerializeField] private PlayerControllerConfig _playerControllerConfig;
        [SerializeField] private WalkAnimationConfigCollection _walkAnimationConfig;
        [SerializeField] private PlayerStateConfigCollection _playerStateConfigCollection;
        [SerializeField] private MiniMapConfig _miniMapConfig; 
        [SerializeField] private AdsConfig _adsConfig;  
        [SerializeField] private BoosterConfigCollection _boosterConfigCollection;

        public void Install(DiContainer container)
        {
            new ConfigLoader(container, new CsvConfigDeserializer())
                .RegisterSingle<LocalizationConfig>(ConfigName.LOCALIZATION)
                .RegisterSingle<EnemyBodyMembersConfig>(ConfigName.ENEMY_BODY_MEMBERS)     
                .RegisterSingle<LevelsConfig>(ConfigName.LEVELS)
                .RegisterSingle<ItemModifiersConfigCollection>(ConfigName.ITEM_MODIFIERS)  
                .RegisterSingleObjectConfig<ConstantsConfig>(ConfigName.CONSTANTS)
                .RegisterStringKeyedCollection<PlayerUnitConfig>(ConfigName.PLAYER_UNITS)
                .RegisterStringKeyedCollection<ReloadableWeaponConfig>(ConfigName.PLAYER_WEAPONS)
                .RegisterStringKeyedCollection<EnemyUnitConfig>(ConfigName.ENEMY_UNITS)   
                .RegisterStringKeyedCollection<ItemConfig>(ConfigName.ITEMS)        
                .RegisterStringKeyedCollection<StartingItemConfig>(ConfigName.STARTING_ITEMS)
                .RegisterStringKeyedCollection<FieldRestrictionsConfig>(ConfigName.FIELD_RESTRICTIONS)
                .RegisterCollection<VibrationType, VibrationConfig>(ConfigName.VIBRATION)
                .RegisterSingle<BattlePassConfigList>(ConfigName.BATTLE_PASS)
                .RegisterSingle<EnemySpawnConfig>(ConfigName.ENEMY_WAVES)
                .RegisterSingle<InteractableItemConfigs>(ConfigName.INTERACTABLE_ITEMS) 
                .RegisterSingleObjectConfig<PlayerControllerLoadableConfig>(ConfigName.PLAYER_CONTROLLER);

            container.Bind<CameraShakeConfig>().FromInstance(_cameraShakeConfig).AsSingle();
            container.Bind<PromoRecordSettings>().FromInstance(_promoRecordSettings).AsSingle();      
            container.Bind<PlayerControllerConfig>().FromInstance(_playerControllerConfig).AsSingle();
            container.QueueForInject(_playerControllerConfig);
            container.Bind<WalkAnimationConfigCollection>().FromInstance(_walkAnimationConfig).AsSingle();   
            container.Bind<PlayerStateConfigCollection>().FromInstance(_playerStateConfigCollection).AsSingle();
            container.Bind<MiniMapConfig>().FromInstance(_miniMapConfig).AsSingle(); 
            container.Bind<AdsConfig>().FromInstance(_adsConfig).AsSingle();     
            _boosterConfigCollection.BindConfigs(container);
        }
    }
}
