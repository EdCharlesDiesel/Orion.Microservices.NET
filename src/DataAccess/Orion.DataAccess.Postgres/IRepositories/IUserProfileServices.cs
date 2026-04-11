using System;
using System.Threading.Tasks;

namespace Orion.Domain.IRepositories;
public interface IUserProfileServices:IRepository<UserProfile>
{
    Task<string> GetUserProfilesByDate(DateTime startDate, DateTime endDate);
    Task<string> GetUserProfilesByCountries(params string[] countries);
    Task<string> GetUserProfilesByCountriesAndDates(DateTime startDate, DateTime endDate, params string[] countries);
    Task<string> GetUserProfilesByIndicator(params string[] indicators);
    Task<UserProfile> Create(UserProfile userProfile);
}

public class UserProfile
{
    public object Id { get; set; }
}