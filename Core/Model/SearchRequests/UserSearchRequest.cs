﻿namespace Core.Model.SearchRequests
{
    public class UserSearchRequest
    {
        public int? AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SearchText { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
