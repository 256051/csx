using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Framework.Expressions.Tests
{
    public class ReflectionHelperTest : ExpressionTest
    {
        [Fact]
        public void PropertySetTest()
        {

            Action<Person, string, string> expr = (p, name, age) =>
            {
                p.Name = name;
                p.Age = age;
            };

            var p = new Person();
            var personExpr = Expression.Parameter(typeof(Person), "p");
            var nameExpr = Expression.Parameter(typeof(string), "name");
            var ageExpr = Expression.Parameter(typeof(string), "age");
            var personNameExpr=Expression.Property(personExpr, nameof(Person.Name));
            var personAgeExpr = Expression.Property(personExpr, nameof(Person.Age));

            var body=Expression.Block(
                Expression.Assign(personNameExpr, Expression.Convert(nameExpr, typeof(string))), 
                Expression.Assign(personAgeExpr, Expression.Convert(ageExpr, typeof(string))));
            var func=Expression.Lambda<Action<Person, string, string>>(body,new ParameterExpression[] { personExpr, nameExpr, ageExpr });
            func.Compile()(p, "张三", "20");
        }

        [Fact]
        public void PropertySetTest1()
        {
            var p = new Person();
            _reflectionHelper.PropertySetValue(p, nameof(Person.Name), "张三");
            _reflectionHelper.PropertySetValue(p, nameof(Person.Name), "张三");
            _reflectionHelper.PropertySetValue(p, nameof(Person.Name), "张三");
            _reflectionHelper.PropertySetValue(p, nameof(Person.Age), "11");
            p.Age.ShouldBe("11");
            p.Name.ShouldBe("张三");
        }

        [Fact]
        public void PropertyGetTest1()
        {
            var p = new Person();
            p.Name = "张三";
            p.Age = "11";
            p.Sex = 666;
            _reflectionHelper.PropertyGetValue(p, nameof(Person.Age)).ShouldBe("11");
            _reflectionHelper.PropertyGetValue(p, nameof(Person.Sex)).ShouldBe(666);
            _reflectionHelper.PropertyGetValue(p, nameof(Person.Name)).ShouldBe("张三");
        }

        public class Person
        { 
            public string Name { get; set; }

            public string Age { get; set; }

            public int Sex { get; set; }
        }
    }
}
