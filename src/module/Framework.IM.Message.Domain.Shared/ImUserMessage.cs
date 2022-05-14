using Dapper.Contrib.Extensions;
using Framework.IM.Message.Store;

namespace Framework.IM.Message.Domain.Shared
{
    [Table("ImUserMessage")]
    public class ImUserMessage : UserMessage
    {
        [ExplicitKey]
        public override string Id { get; set; }
    }
}
