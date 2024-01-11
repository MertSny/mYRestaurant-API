namespace Core.Model.SearchRequests
{
    public class UserOperationClaimSearchRequest
    {
        public int? UserId { get; set; }

        public int? OperationClaimId { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}

