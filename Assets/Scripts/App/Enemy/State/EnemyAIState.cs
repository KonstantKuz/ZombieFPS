namespace App.Enemy.State
{
    public enum EnemyAIState
    {
        NotInitialized,
        Idle,
        MoveToTarget, 
        Stopped,
        Attack,
    }
}