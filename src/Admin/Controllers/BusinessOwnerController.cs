using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.Admin.Areas.API;
using Orion.DataAccess.Postgres.Entities;
using Orion.Domain.Constants;
using Orion.Domain.Utility;

namespace Orion.Admin.Controllers
{
    // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
    public class BusinessOwnerController : Controller
    {
        private const int IdForCreateNewBusinessowner = 0;
        private readonly IBusinessOwnerService _businessOwnerService;
        private readonly IValidatorStrategy<BusinessOwner> _validator;
        private readonly ITestDataUtility _testDataUtility;

        public BusinessOwnerController(
            IBusinessOwnerService businessOwnerService,
            IValidatorStrategy<BusinessOwner> validator,
            ITestDataUtility testDataUtility)
        {
            _businessOwnerService = businessOwnerService ?? throw new ArgumentNullException(nameof(businessOwnerService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _testDataUtility = testDataUtility ?? throw new ArgumentNullException(nameof(testDataUtility));
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var businessOwners = _businessOwnerService.GetBusinessOwners();
            return View(businessOwners);
        }

        [AllowAnonymous]
        [Route("/[controller]/[action]/{id}")]
        [Route("/businessOwner/{id}.aspx")]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var businessOwner = _businessOwnerService.GetBusinessOwnerById(id.Value);
            if (businessOwner == null)
                return NotFound();

            return View(businessOwner);
        }

        [AllowAnonymous]
        [Route("/businessOwner/{last:alpha}/{first:alpha}")]
        public IActionResult Details(string last, string first)
        {

            throw new NotImplementedException();
            // if (string.IsNullOrWhiteSpace(last) || string.IsNullOrWhiteSpace(first))
            //     return BadRequest();
            //
            // var businessOwner = _businessOwnerService.Search(first, last).FirstOrDefault();
            // if (businessOwner == null)
            //     return NotFound();
            //
            // return View("Details", businessOwner);
        }

        public IActionResult Create()
        {
            return RedirectToAction(nameof(Edit), new { id = IdForCreateNewBusinessowner });
        }

        // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
        [Authorize(Policy = SecurityConstants.PolicyNameEditBusinessOwner)]
        public IActionResult Edit(int? id)
        {
            // if (!id.HasValue)
            //     return BadRequest();
            //
            // BusinessOwner businessOwner;
            //
            // if (id.Value == IdForCreateNewBusinessowner)
            // {
            //     businessOwner = new BusinessOwner();
            //     businessOwner.AddTerm(
            //         BusinessOwnerConstants.BusinessOwner,
            //         default,
            //         default,
            //         0);
            // }
            // else
            // {
            //     businessOwner = _businessOwnerService.GetBusinessOwnerById(id.Value);
            // }
            //
            // if (businessOwner == null)
            //     return NotFound();
            //
            // return View(businessOwner);
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = SecurityConstants.PolicyNameEditBusinessOwner)]
        public IActionResult Edit(BusinessOwner businessOwner)
        {
            // if (_validator.IsValid(businessOwner))
            // {
            //     bool isCreateNew = businessOwner.Id == IdForCreateNewBusinessowner;
            //
            //     if (!isCreateNew)
            //     {
            //         var existing = _businessOwnerService.GetBusinessOwnerById(businessOwner.Id);
            //         if (existing == null)
            //         {
            //             return BadRequest(
            //                 $"Unknown businessOwner id '{businessOwner.Id}'.");
            //         }
            //     }
            //
            //     _businessOwnerService.Save(businessOwner);
            //
            //     return RedirectToAction(nameof(Edit), new { id = businessOwner.Id });
            // }
            //
            // return View(businessOwner);
            throw new NotImplementedException();
        }

        //[AllowAnonymous]
        public async Task<IActionResult> ResetDatabase()
        {
            await _testDataUtility.CreateBusinessOwnerTestData();
            return RedirectToAction(nameof(Index));
        }

        //[AllowAnonymous]
        public IActionResult VerifyDatabaseIsPopulated()
        {
            _testDataUtility.VerifyDatabaseIsPopulated();
            return RedirectToAction(nameof(Index));
        }
    }
}
