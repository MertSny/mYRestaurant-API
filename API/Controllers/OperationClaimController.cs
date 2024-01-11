using Business.Abstract;
using Core.Helpers;
using Core.Model.SearchRequests;
using Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Core.Enums.OrderByEnums;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperationClaimController : BaseController
    {
        private readonly IOperationClaimService _operationClaimService;
        public OperationClaimController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] OperationClaimDto request)
        {
            var response = await _operationClaimService.AddAsync(request, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] OperationClaimDto request)
        {
            var response = await _operationClaimService.UpdateAsync(request, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _operationClaimService.Delete(id, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(OperationClaimSearchRequest request)
        {
            var result = await _operationClaimService.Search(request);
            return Ok(result);
        }

        [HttpPost("searchWithPagination")]
        public async Task<IActionResult> SearchWithPagination(OperationClaimSearchRequest request)
        {
            var filterRequest = new FilterRequest<DefaultOrderBy, OperationClaimSearchRequest>((DefaultOrderBy)0, request);
            filterRequest.Page = request.PageNo.HasValue ? request.PageNo.Value : 1;
            filterRequest.PageSize = request.PageSize.HasValue ? request.PageSize.Value : 10000;
            if (request.PageSize.HasValue)
            {
                if (request.PageSize == -1)
                {
                    filterRequest.PageSize = int.MaxValue;
                }
            }
            var response = await _operationClaimService.SearchWithPagination(filterRequest);
            return Ok(response);
        }
    }
}
