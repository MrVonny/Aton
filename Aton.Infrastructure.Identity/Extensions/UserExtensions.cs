namespace Aton.Infrastructure.Identity.Extensions;

public static class UserExtensions
{
    // public static IndexUserModel ToIndexModel(this Account account)
    // {
    //     var config = new MapperConfiguration(cfg => cfg.CreateMap<Account, IndexUserModel>());
    //     var mapper = new Mapper(config);
    //     return mapper.Map<Account, IndexUserModel>(account);
    // }
    //
    // public static  IQueryable<Account> OlderThan(this IQueryable<Account> queryable, int age)
    // {
    //     return queryable.Where(u =>
    //         u.Birthday.HasValue &&
    //         (u.Birthday.Value.AddYears(DateTime.Now.Year - u.Birthday.Value.Year) >
    //          DateTime.Now
    //             ? DateTime.Now.Year - u.Birthday.Value.Year - 1
    //             : DateTime.Now.Year - u.Birthday.Value.Year) > age);
    // } 
}