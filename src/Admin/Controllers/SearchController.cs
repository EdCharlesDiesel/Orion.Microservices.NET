using Microsoft.AspNetCore.Mvc;
using Orion.Admin.Areas.API;
using Orion.Admin.Models;
using Orion.DataAccess.Postgres.AllFeatures;
using Orion.DataAccess.Postgres.Entities;

namespace Orion.Admin.Controllers
{
    public class SearchController(
        IBusinessOwnerService service,
        IFeatureManager featureManager)
        : Controller
    {
        private readonly IBusinessOwnerService _service = service ?? throw new ArgumentNullException("service", "service is null.");
        private readonly IFeatureManager _featureManager = featureManager ?? throw new ArgumentNullException("featureManager", "featureManager is null.");

        // GET: Search
        public ActionResult Index()
        {
            // if (_featureManager.Search == false)
            // {
            //     return NotFound();
            // }
            //
            // var model = new SearchViewModel();
            //
            // if (_featureManager.SearchByBirthBusinessProvince)
            // {
            //     return View("IndexProvinceSearch", model);
            // }
            //
            // return View(model);
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Index(SearchViewModel model)
        {
            // if (model == null)
            // {
            //     throw new InvalidOperationException("Argument cannot be null.");
            // }
            //
            // if (_featureManager.Search == false)
            // {
            //     return NotFound();
            // }
            //
            // // var results = _Service.Search(model.FirstName, model.LastName);
            //
            // IList<BusinessOwner>? results = null;
            //
            // if (_featureManager.SearchByBirthBusinessProvince)
            // {
            //     results = _service.Search(
            //         model.FirstName, model.LastName,
            //         model.BirthProvince, model.BusinessProvince);
            // }
            // else
            // {
            //     results = _service.Search(
            //         model.FirstName, model.LastName);
            // }
            //
            // var modelToReturn = new SearchViewModel();
            //
            // modelToReturn.FirstName = model.FirstName;
            // modelToReturn.LastName = model.LastName;
            //
            // if (results != null)
            // {
            //     Adapt(results, modelToReturn.Results);
            // }
            //
            // if (_featureManager.SearchByBirthBusinessProvince)
            // {
            //     return View("IndexProvinceSearch", modelToReturn);
            // }
            //
            // return View(modelToReturn);
            throw new NotImplementedException();
        }

        private void Adapt(IList<BusinessOwner> fromValues, List<SearchResultRow> toValues)
        {
            // if (fromValues == null)
            //     throw new ArgumentNullException("fromValues", "fromValues is null.");
            // if (toValues == null)
            //     throw new ArgumentNullException("toValues", "toValues is null.");
            //
            // var adapter = new BusinessOwnerToSearchResultRowAdapter();
            //
            // SearchResultRow toValue;
            //
            // foreach (var fromValue in fromValues)
            // {
            //     toValue = new SearchResultRow();
            //
            //     adapter.Adapt(fromValue, toValue);
            //
            //     toValues.Add(toValue);
            // }
            throw new NotImplementedException();
        }
    }
}