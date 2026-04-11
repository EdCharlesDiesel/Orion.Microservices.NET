using System.Globalization;
using Orion.DataAccess.Postgres.Entities;

namespace Orion.API.HumanResources.Business;

public interface IPromotionService
{
    Task<bool> PromoteCalendarAsync(OrionCalendarEvent employee);
}