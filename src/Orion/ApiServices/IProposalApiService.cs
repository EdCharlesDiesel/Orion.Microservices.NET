using Orion.Shared;

namespace Orion.ApiServices
{
    public interface IProposalApiService
    {
        Task Add(ProposalModel model);
        Task<ProposalModel?> Approve(int proposalId);
        Task<IEnumerable<ProposalModel>?> GetAll(int conferenceId);
    }
}