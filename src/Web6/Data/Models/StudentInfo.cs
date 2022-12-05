using System.ComponentModel.DataAnnotations;

namespace Web6.Data.Models
{
    public class StudentInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "学生姓名不能为空")]
        [MaxLength(ValidationConsts.StringDefaultMaxLangth, ErrorMessage = "名称字数超出最大限制,请修改!")]
        public string Name { get; set; }

        /// <summary>
        ///身份证
        /// </summary>
        [MaxLength(ValidationConsts.IdCardMaxLength, ErrorMessage = "身份证字数超出最大限制,请修改!")]
        public string IdCard { get; set; }

        /// <summary>
        /// 学籍号
        /// </summary>
        [MaxLength(ValidationConsts.StudentCodeMaxLength, ErrorMessage = "学籍号字数超出最大限制,请修改!")]
        public string StudentCode { get; set; }

        /// <summary>
        ///手机号码
        /// </summary>
        [MaxLength(ValidationConsts.PhoneNumberMaxLength, ErrorMessage = "手机号码字数超出最大限制,请修改!")]
        public string Phone { get; set; }

        /// <summary>
        ///民族
        /// </summary>
        [MaxLength(ValidationConsts.StringDefaultMaxLangth, ErrorMessage = "民族字数超出最大限制,请修改!")]
        public string Nation { get; set; }

        /// <summary>
        /// 监护人
        /// </summary>
        [MaxLength(ValidationConsts.StringDefaultMaxLangth, ErrorMessage = "监护字数超出最大限制,请修改!")]
        public string Guardian { get; set; }

        /// <summary>
        /// 监护人电话
        /// </summary>
        [MaxLength(ValidationConsts.PhoneNumberMaxLength, ErrorMessage = "监护人电话字数超出最大限制,请修改!")]
        public string GuardianPhone { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        [MaxLength(ValidationConsts.AddressMaxLength, ErrorMessage = "家庭地址字数超出最大限制,请修改!")]
        public string Address { get; set; }
    }
}
