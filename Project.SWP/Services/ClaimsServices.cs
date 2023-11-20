using Services.Service;

namespace Project.SWP.Services
{
    public class ClaimsServices : IClaimsServices
    {
        public ClaimsServices(IHttpContextAccessor httpContext)
        {
            var id = httpContext.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            _currentUser = id;
        }
        private string? _currentUser { get; }
        public string? GetCurrentUser() => _currentUser;
    }
}
