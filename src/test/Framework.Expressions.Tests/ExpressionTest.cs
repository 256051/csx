using Framework.Test;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Expressions.Tests
{
    public class ExpressionTest: TestBase
    {
        protected IExpressionsReflectionHelper _reflectionHelper;

        public ExpressionTest()
        {
            _reflectionHelper = ServiceProvider.GetRequiredService<IExpressionsReflectionHelper>();
        }

        protected override void LoadModules()
        {
            ApplicationConfiguration.UseExpression();
        }
    }
}
