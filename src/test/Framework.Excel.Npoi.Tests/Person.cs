using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Excel.Npoi.Tests
{
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    /// <summary>
    /// 人员创建Dto
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        [Required(ErrorMessage = "身份证不能为空")]
        public string IdCard { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "性别不能为空")]
        public string Sex { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber { get; set; }

        /// <summary>
        /// 所属组织机构
        /// </summary>
        public string OrganizationUnit { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 所属岗位
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 是否委领导
        /// </summary>
        public string IsLeader { get; set; }

        /// <summary>
        /// 是否安排住宿
        /// </summary>
        public string IsStayed { get; set; }

        /// <summary>
        /// 房间类型
        /// </summary>
        public string RoomType { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }
    }
}
