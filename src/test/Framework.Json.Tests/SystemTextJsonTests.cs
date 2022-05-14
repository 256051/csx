using Shouldly;
using Xunit;

namespace Framework.Json.Tests
{
    public class SystemTextJsonTests: JsonTest
    {
        [Fact]
        public void Test1()
        {
            var person = new Person();
            var textJson=_jsonSerializer.Serialize(person);
            var newPerson= _jsonSerializer.Deserialize<Person>(textJson);
            newPerson.Name.ShouldBe(person.Name);
            newPerson.Age.ShouldBe(person.Age);
            newPerson.CreateTime.ShouldBe(person.CreateTime);
            newPerson.ModifyTime.ShouldBe(person.ModifyTime);
            newPerson.IsMan.ShouldBe(person.IsMan);
            newPerson.Position.ShouldBe(person.Position);
            newPerson.Money.ShouldBe(person.Money);
            newPerson.High.ShouldBe(person.High);
            newPerson.Weight.ShouldBe(person.Weight);
        }
    }
}
