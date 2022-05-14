using Framework.IM.Message.Domain;
using Framework.IM.Message.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Framework.AspNetCore.SignalR.Application.Services
{
    public class ApplicationUserMessageManager : UserMessageManager<ImUserMessage>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ApplicationUserMessageManager(IHttpContextAccessor httpContextAccessor,IUserMessageStore<ImUserMessage> store) : base(store) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override CancellationToken CancellationToken=> _httpContextAccessor?.HttpContext.RequestAborted ?? CancellationToken.None;
    }
}
