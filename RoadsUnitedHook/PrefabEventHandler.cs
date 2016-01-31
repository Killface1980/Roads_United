namespace RoadsUnited.Hook
{
    public delegate void PrefabEventHandler<P>(P prefab) where P : PrefabInfo;
}