namespace Framework.AspNetCore.Swagger
{
    public interface IXmlElementFinder
    {
        /// <summary>
        /// 判断类型是否存在
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        bool IsTypeExisted(string typeName);

        /// <summary>
        /// 根据枚举的字段名称拿到summary
        /// </summary>
        /// <param name="fildName"></param>
        /// <returns></returns>
        string GetEnumSummary(string fildName);
    }
}
