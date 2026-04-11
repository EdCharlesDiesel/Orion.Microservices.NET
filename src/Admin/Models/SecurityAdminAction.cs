using Microsoft.AspNetCore.Mvc.Rendering;

namespace Orion.Admin.Models
{
    public class SecurityAdminAction
    {
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> BusinessOwners { get; set; }

        public List<SelectListItem> Permissions { get; set; }

        public string UserId { get; set; }
        public string RoleId { get; set; }

        public string BusinessOwnerId { get; set; }
        public string Permission { get; set; }
        public List<ClaimViewModel> Claims { get; set; }
        public string SubscriptionType { get; set; }
        public List<SelectListItem> SubscriptionTypes { get; set; }

    }
}
