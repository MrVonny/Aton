using Aton.Application.AutoMapper;

namespace Aton.Services.Api.AutoMapper
{
    public class AutoMapperConfig
    {
        //public static MapperConfiguration RegisterMappings()
        //{
        //    return new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile(new DomainToViewModelMappingProfile());
        //        cfg.AddProfile(new ViewModelToDomainMappingProfile());
        //    });
        //}

        public static Type[] RegisterMappings()
        {
            return new Type[]
            {
                typeof(AppModelToAspModelProfile),
                typeof(AspModelToAppModelProfile)
            };
        }
    }
}
