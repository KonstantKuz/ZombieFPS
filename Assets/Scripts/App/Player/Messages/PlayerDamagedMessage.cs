namespace App.Player.Messages
{
    public readonly struct PlayerDamagedMessage
    {
        public readonly string AttackName;

        public PlayerDamagedMessage(string attackName)
        {
            AttackName = attackName;
        }
    }
}