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
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] UserDto request)
        {
            var response = await _userService.AddAsync(request, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UserDto request)
        {
            var response = await _userService.UpdateAsync(request, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.Delete(id, GetCurrentUser());
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(UserSearchRequest request)
        {
            var result = await _userService.Search(request);
            return Ok(result);
        }

        [HttpPost("searchWithPagination")]
        public async Task<IActionResult> SearchWithPagination(UserSearchRequest request)
        {
            var filterRequest = new FilterRequest<DefaultOrderBy, UserSearchRequest>((DefaultOrderBy)0, request);
            filterRequest.Page = request.PageNo.HasValue ? request.PageNo.Value : 1;
            filterRequest.PageSize = request.PageSize.HasValue ? request.PageSize.Value : 10000;
            if (request.PageSize.HasValue)
            {
                if (request.PageSize == -1)
                {
                    filterRequest.PageSize = int.MaxValue;
                }
            }
            var response = await _userService.SearchWithPagination(filterRequest);
            return Ok(response);
        }
    }
}
