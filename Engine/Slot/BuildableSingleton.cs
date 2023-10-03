public abstract class BuildableSingleton : Buildable
{
    protected abstract Buildable _singleton { set; }

    private void Awake()
    {
        _singleton = this;
    }
}
