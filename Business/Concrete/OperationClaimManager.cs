using AutoMapper;
using Business.Abstract;
using Business.BusinessAspect.LogAspect;
using Business.BusinessAspect.Performance;
using Business.Constansts;
using Business.ValidationRules.FluentValidationDto;
using Core.Aspects.Autofac.Validation;
using Core.Entites.Concrete;
using Core.Enums;
using Core.Helpers;
using Core.Model.SearchRequests;
using Core.Utulities.Result;
using DataAccess.Abstract;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IMapper _mapper;
        private readonly IOperationClaimDal _operationClaimDal;

        public OperationClaimManager(IMapper mapper, IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(OperationClaimDtoValidator))]
        [LogAspect]
        [PerformanceAspect(1)]
        public async Task<IDataResult<OperationClaimDto>> AddAsync(OperationClaimDto dto, UserContext currentUser)
        {
            var data = _mapper.Map<OperationClaim>(dto);
            data.CreatedBy = currentUser.SystemUserId;
            data.CreatedDate = DateTime.Now;
            data.IsDeleted = false;
            var result = await _operationClaimDal.AddAsync(data);
            if (result == 0)
                return new ErrorDataResult<OperationClaimDto>(dto, Messages.CreatedFailure);

            return new SuccessDataResult<OperationClaimDto>(dto, Messages.CreatedSuccess);
        }

        [LogAspect]
        [PerformanceAspect(1)]
        public async Task<IResult> Delete(int id, UserContext currentUser)
        {
            var operationClaim = _operationClaimDal.Get(p => p.Id == id);
            if (operationClaim == null)
                return new ErrorResult(Messages.NotFound);

            operationClaim.IsDeleted = true;
            operationClaim.UpdatedBy = currentUser.SystemUserId;
            operationClaim.UpdatedDate = DateTime.Now;

            var result = await _operationClaimDal.UpdateFieldsSave(operationClaim, p => p.IsDeleted, p => p.UpdatedBy, p => p.UpdatedDate);

            if (result == 0)
                return new ErrorResult(Messages.DeletedFailure);

            return new SuccessResult(Messages.DeletedSuccess);
        }

        [LogAspect]
        [PerformanceAspect(1)]
        public async Task<IDataResult<OperationClaimDto>> GetById(int id)
        {
            var predicate = PredicateBuilder.True<OperationClaim>();
            predicate = predicate.And(p => p.Id == id);
            var includes = new List<string>
            {

            };
            var data = (await _operationClaimDal.Search(predicate, true, includes)).FirstOrDefault();
            var result = _mapper.Map<OperationClaimDto>(data);

            return new SuccessDataResult<OperationClaimDto>(result);
        }

        [LogAspect]
        [PerformanceAspect(1)]
        public async Task<IDataResult<IEnumerable<OperationClaimDto>>> Search(OperationClaimSearchRequest request)
        {
            var predicate = GetExpression(request);
            var includes = new List<string>
            {

            };
            var data = await _operationClaimDal.Search(predicate, true, includes);
            var result = _mapper.Map<IEnumerable<OperationClaimDto>>(data);

            return new SuccessDataResult<IEnumerable<OperationClaimDto>>(result);
        }

        [LogAspect]
        [PerformanceAspect(1)]
        public async Task<IDataResult<PagedResult<OperationClaimDto>>> SearchWithPagination(FilterRequest<OrderByEnums.DefaultOrderBy, OperationClaimSearchRequest> request)
        {
            var filter = request;

            var predicate = GetExpression(filter.filter);

            var includes = new List<string>
            {

            };

            Expression<Func<OperationClaim, object>> orderSelector = null;
            orderSelector = x => x.CreatedDate;

            var data = await _operationClaimDal.Search(filter.Page, filter.PageSize, predicate, orderSelector, filter.orderType == OrderType.ASC, true, includes);

            var result = _mapper.Map<PagedResult<OperationClaim>, PagedResult<OperationClaimDto>>(data);

            return new SuccessDataResult<PagedResult<OperationClaimDto>>(result);
        }

        [ValidationAspect(typeof(OperationClaimDtoValidator))]
        [LogAspect]
        [PerformanceAspect(1)]
        public async Task<IResult> UpdateAsync(OperationClaimDto dto, UserContext currentUser)
        {
            var isExist = _operationClaimDal.Get(p => p.Id == dto.Id);
            if (isExist == null)
                return new ErrorResult(Messages.NotFound);

            var data = _mapper.Map(dto, isExist);

            data.UpdatedBy = currentUser.SystemUserId;
            data.UpdatedDate = DateTime.Now;
            data.IsDeleted = false;

            var result = await _operationClaimDal.UpdateAsync(data);
            if (result == 0)
                return new ErrorResult(Messages.UpdatedFailure);

            return new SuccessResult(Messages.UpdatedSuccess);
        }

        private Expression<Func<OperationClaim, bool>> GetExpression(OperationClaimSearchRequest request)
        {
            var predicate = PredicateBuilder.True<OperationClaim>();

            if (!string.IsNullOrEmpty(request.SearchText))
                predicate = predicate.And(p => p.Name.ToUpper().Contains(request.SearchText.ToUpper()));

            return predicate;
        }
    }
}
