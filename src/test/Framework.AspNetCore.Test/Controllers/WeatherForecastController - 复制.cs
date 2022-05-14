using Framework.DDD.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Framework.AspNetCore.Test.Controllers
{

    /// <summary>
    /// 状态
    /// </summary>
    public enum State
    {
        /// <summary>
        /// 启用
        /// </summary>
        On=0,
        /// <summary>
        /// 关闭
        /// </summary>
        Off=1
    }

    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName="测试模块")]
    public class ApiTestController : ControllerBase
    {
        public class TestRequest: CreationAuditedAggregateRoot
        {
            /// <summary>
            /// 测试字段 长度不能大于10
            /// </summary>
            [MaxLength(10,ErrorMessage = "测试字段长度不能大于10")]
            public string Name { get; set; }

            /// <summary>
            /// 工作状态
            /// </summary>
            [Required(ErrorMessage = "工作状态不能为空")]
            public State WorkState { get; set; }
        }


        public class TestResponse
        {
            /// <summary>
            /// 响应
            /// </summary>
            public string Data { get; set; }

            /// <summary>
            /// 年龄
            /// </summary>
            public int Age { get; set; }
        }

        /// <summary>
        /// 测试Swagger获取方法
        /// </summary>
        [HttpPost]
        [Authorize]
        public TestResponse Get(TestRequest model)
        {
            return new TestResponse() { Data = "返回值", Age = 22 };
        }

        /// <summary>
        /// 测试Swaggerget请求注释
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="age">年龄</param>
        /// <returns></returns>
        [HttpGet]
        public TestResponse GetTest(string name,int age)
        {
            return new TestResponse() { Data = "返回值", Age = 22 };
        }

    }
}
