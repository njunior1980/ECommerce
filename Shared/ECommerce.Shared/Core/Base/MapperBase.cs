namespace ECommerce.Shared.Core.Base;

public abstract class MapperBase<TSource, TDestiny>
{
    public virtual void Map(TSource source, TDestiny destiny)
    {

    }

    public virtual TDestiny Map(TSource source)
    {
        return default!;
    }
}