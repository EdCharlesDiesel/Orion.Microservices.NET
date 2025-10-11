using Orion.Shared;

namespace Orion.Api.Repositories;

public interface IConferenceRepository
{
    int Add(ConferenceModel model);
    IEnumerable<ConferenceModel> GetAll();
    ConferenceModel? GetById(int id);
}