using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Helpers
{
    public class Response
    {
        public Response()
        {
            Message = "";
            ErrorCode = "";
            Url = "";
        }
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }

    public class Response<T> : Response
    {
        public T Data;
        public Response(T t)
        {
            this.Data = t;
        }
    }

    public class Request<T>
    {
        public T Data;
        public Request(T t)
        {
            this.Data = t;
        }
        public UserContext UserContext { get; set; }
    }

    public class UserContext
    {
        public UserContext()
        {
            Roles = new List<string>();
        }
        public int SystemUserId { get; set; }
        public string CurrentUserId { get; set; }
        public int CompanyId { get; set; }
        public int? BudgetCode { get; set; }
        public string ProxyUserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public class FilterRequest<TorderBy> where TorderBy : Enum
    {
        public TorderBy orderBy;

        public FilterRequest(TorderBy orderby)
        {
            this.orderBy = orderby;
        }

        [Required]
        [Range(1, 100)]
        [DefaultValue(1)]
        public int Page { get; set; } = 1;

        [Required]
        [Range(1, 20000)]
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10000;

        [DefaultValue(OrderType.DESC)]
        public OrderType orderType { get; set; }
    }

    public class FilterRequest<TorderBy, Tfilter> : FilterRequest<TorderBy> where TorderBy : Enum where Tfilter : class
    {
        public Tfilter filter;

        public FilterRequest(TorderBy orderby, Tfilter data) : base(orderby)
        {
            this.filter = data;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderType
    {
        ASC,
        DESC
    }
}
