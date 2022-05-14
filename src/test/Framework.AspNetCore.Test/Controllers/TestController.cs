using Microsoft.AspNetCore.Mvc;

namespace Framework.AspNetCore.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
      
        /// <summary>
        /// 测试Swaggerget请求注释
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="age">年龄</param>
        /// <returns></returns>
        [HttpPost]
        [Route("module")]
        public void TestNoModule(string name,int age)
        {
            
        }

    }
}
