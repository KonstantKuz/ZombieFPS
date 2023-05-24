namespace App.Unit.Model
{
    public interface IAttackModel
    { 
        string Name {  get; }
        float AttackInterval { get; }
        float AttackDistance { get; }
        float HitDamage { get; }
    }
}