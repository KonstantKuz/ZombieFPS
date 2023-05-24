namespace Feofun.World.Extesion
{
    public static class WorldExtenstion
    {
        public static bool IsPoolActivated(this World world) => world.PoolContainer.gameObject.activeInHierarchy;
    }
}