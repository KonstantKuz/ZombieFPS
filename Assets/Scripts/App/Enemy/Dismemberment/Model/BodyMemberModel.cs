using App.Enemy.Dismemberment.Config;

namespace App.Enemy.Dismemberment.Model
{
    public class BodyMemberModel
    {
        public float MainHealth { get; }
        public float ExtraHealth { get; }

        public BodyMemberModel(BodyMemberConfig config)
        {
            MainHealth = config.MainHealth;
            ExtraHealth = config.ExtraHealth;
        }
    }
}