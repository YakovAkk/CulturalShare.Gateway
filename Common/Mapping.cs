using Omu.ValueInjecter;

namespace CulturalShare.GatewayCommon;

public static class Mapping
{
    private static readonly MapperInstance mapper;

    static Mapping()
    {
        mapper = new MapperInstance();
    }

    public static TDest MapTo<TDest>(this object source)
    {
        return mapper.Map<TDest>(source);
    }
}
