using Xunit;

namespace Framework.Expressions.Tests
{
    public class MemberInfoHelperTest : ExpressionTest
    {
        [Fact]
        public void ReadMemberInfoTest()
        {
            var menber= _reflectionHelper.ReadMemberInfo<Person>(p => p.Name);
        }

        public class Person
        { 
            public string Name { get; set; }

            public int Age { get; set; }
        }
    }
}
