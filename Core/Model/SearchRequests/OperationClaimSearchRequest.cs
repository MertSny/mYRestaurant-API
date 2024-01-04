using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.SearchRequests
{
    public class OperationClaimSearchRequest
    {
        public string SearchText { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
