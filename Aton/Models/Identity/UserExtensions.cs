using AutoMapper;

namespace Aton.Models.Identity;

public static class UserExtensions
{
    public static IndexUserModel ToIndexModel(this User user)
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<User, IndexUserModel>());
        var mapper = new Mapper(config);
        return mapper.Map<User, IndexUserModel>(user);
    }
    
    public static  IQueryable<User> OlderThan(this IQueryable<User> queryable, int age)
    {
        return queryable.Where(u =>
            u.Birthday.HasValue &&
            (u.Birthday.Value.AddYears(DateTime.Now.Year - u.Birthday.Value.Year) >
             DateTime.Now
                ? DateTime.Now.Year - u.Birthday.Value.Year - 1
                : DateTime.Now.Year - u.Birthday.Value.Year) > age);
    }
    
    public static IQueryable<IndexUserModel> ToIndexModel(this IQueryable<User> queryable)
    {
        return queryable.Select(u => new IndexUserModel
        {
            Admin = u.Admin,
            Id = u.Id,
            Login = u.Login,
            CreatedOn = u.CreatedOn,
            UserName = u.UserName
        });
    }
}