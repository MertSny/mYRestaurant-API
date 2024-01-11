using Business.Abstract;
using Core.Helpers;
using Core.Model.SearchRequests;
using Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Core.Enums.OrderByEnums;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserOperationClaimController : BaseController
    {
        private readonly IUserOperationClaimService _userOperationClaimService;
        public UserOperationClaimController(IUserOperationClaimService userOperationClaimService)
        {
            _userOperationClaimService = userOperationClaimService;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] UserOperationClaimDto request)
        {
            var response = await _userOperationClaimService.AddAsync(request, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UserOperationClaimDto request)
        {
            var response = await _userOperationClaimService.UpdateAsync(request, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userOperationClaimService.Delete(id, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(UserOperationClaimSearchRequest request)
        {
            var result = await _userOperationClaimService.Search(request);
            return Ok(result);
        }

        [HttpPost("searchWithPagination")]
        public async Task<IActionResult> SearchWithPagination(UserOperationClaimSearchRequest request)
        {
            var filterRequest = new FilterRequest<DefaultOrderBy, UserOperationClaimSearchRequest>((DefaultOrderBy)0, request);
            filterRequest.Page = request.PageNo.HasValue ? request.PageNo.Value : 1;
            filterRequest.PageSize = request.PageSize.HasValue ? request.PageSize.Value : 10000;
            if (request.PageSize.HasValue)
            {
                if (request.PageSize == -1)
                {
                    filterRequest.PageSize = int.MaxValue;
                }
            }
            var response = await _userOperationClaimService.SearchWithPagination(filterRequest);
            return Ok(response);
        }
    }
}
