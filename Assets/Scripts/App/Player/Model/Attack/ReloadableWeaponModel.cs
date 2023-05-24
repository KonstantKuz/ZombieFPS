using System;
using App.Modifiers;
using App.Player.Config.Attack;
using App.Unit.Model;
using App.Vibration;
using App.Weapon.Projectile.Data;
using Feofun.Modifiers.ParameterOwner;
using Feofun.Modifiers.Parameters;

namespace App.Player.Model.Attack
{
    public class ReloadableWeaponModel : ModifiableParameterOwner, IAttackModel
    {
        private readonly ReloadableWeaponConfig _config;
        private readonly FloatModifiableParameter _damage; 
        private readonly FloatModifiableParameter _fireRate; 
        private readonly FloatModifiableParameter _reloadTime;   
        private readonly FloatModifiableParameter _control;     
        private readonly FloatModifiableParameter _accuracy;
        private readonly FloatModifiableParameter _clipSize;
    
        public string Name  { get; }
        public float AttackInterval => 1 / FireRate;
        public float AttackDistance { get; }
        public float HitDamage => FullDamage / _config.ShotCount;
        public float ProjectileSpeed { get; }
        public float DamageRadius { get; }  
        public VibrationType Vibration { get; }
        public int ShotCount { get; }
        
        public float ReloadTime => _reloadTime.Value;
        public float Control => _control.Value;
        public float Accuracy => _accuracy.Value;
        public int ClipSize => (int) Math.Round(_clipSize.Value, MidpointRounding.AwayFromZero);

        public float FullDamage => _damage.Value; 
        public float FireRate => _fireRate.Value;
        
        public ReloadableWeaponModel(string name, ReloadableWeaponConfig config)
        {
            _config = config;
            Name = name;
            AttackDistance = config.Distance;
            ProjectileSpeed = config.ProjectileSpeed;
            DamageRadius = config.DamageRadius;
            Vibration = config.Vibration;
            ShotCount = config.ShotCount;
            var parameterCreator = new FloatModifiableCreateHelper();
            _damage = parameterCreator.CreateWithRange(ParameterNames.DAMAGE, config.Damage, this);
            _fireRate = parameterCreator.CreateWithRange(ParameterNames.FIRE_RATE, config.FireRate, this);  
            _reloadTime = parameterCreator.CreateWithRange(ParameterNames.RELOAD_TIME, config.ReloadTime, this);     
            _control = parameterCreator.CreateWithRange(ParameterNames.CONTROL, config.Control, this);  
            _accuracy = parameterCreator.CreateWithRange(ParameterNames.ACCURACY, config.Accuracy, this);    
            _clipSize = parameterCreator.CreateWithRange(ParameterNames.CLIP_SIZE, config.ClipSize, this);
        }

        public static ReloadableWeaponModel FromConfig(ReloadableWeaponConfig config)
        {
            return new ReloadableWeaponModel(config.Id, config);
        }

        public ProjectileParams CreateProjectileParams()
        {
            return new ProjectileParams()
            {
                Speed = ProjectileSpeed,
                HitRadius = DamageRadius,
                MaxDistance = AttackDistance,
            };
        }

        public float GetControlInvertedPercent() => (100f - Control) / 100f;

        public float GetAccuracyInvertedPercent() => (100f - Accuracy) / 100f;
        
    }
}