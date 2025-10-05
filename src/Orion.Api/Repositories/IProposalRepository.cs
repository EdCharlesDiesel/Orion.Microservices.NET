using Orion.Shared;

namespace Orion.Api.Repositories;

public interface IProposalRepository
{
    int Add(ProposalModel model);
    ProposalModel? Approve(int proposalId);
    IEnumerable<ProposalModel> GetAllForConference(int conferenceId);
    ProposalModel? GetOne(int id);
}