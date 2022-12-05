namespace Web6.Data.Models
{
    internal class ValidationConsts
    {
        /// <summary>
        /// 短字符默认长度
        /// </summary>
        public const int ShortStringMaxLangth = 10;

        /// <summary>
        /// 字符串默认长度
        /// </summary>
        public const int StringDefaultMaxLangth = 50;

        /// <summary>
        /// Url最大长度
        /// </summary>
        public const int UrlMaxLength = 225;

        /// <summary>
        /// 名称最大长度
        /// </summary>
        public const int NameMaxLength = 64;

        /// <summary>
        /// 邮箱地址最大长度
        /// </summary>
        public const int EmailAddressMaxLength = 256;

        /// <summary>
        /// 手机号码最大长度
        /// </summary>
        public const int PhoneNumberMaxLength = 15;

        /// <summary>
        /// 备注最大长度
        /// </summary>
        public const int RemarkMaxLength = 500;

        /// <summary>
        /// 描述最大长度
        /// </summary>
        public const int DescriptionMaxLength = 2000;

        /// <summary>
        /// 用户名最大长度
        /// </summary>
        public const int UserNameMaxLength = 256;

        /// <summary>
        /// 金额最大值
        /// </summary>
        public const int AmountMaxinum = 1000000;

        /// <summary>
        /// 金额最小值
        /// </summary>
        public const int AmountMininum = 0;

        /// <summary>
        /// 路径最大长度
        /// </summary>
        public const int PathMaxLength = 256;

        /// <summary>
        /// 身份证最大长度
        /// </summary>
        public const int IdCardMaxLength = 18;

        /// <summary>
        /// 学籍号最大长度
        /// </summary>
        public const int StudentCodeMaxLength = 25;

        /// <summary>
        /// 学号最大长度
        /// </summary>
        public const int StuNubMaxLength = 25;

        /// <summary>
        /// 住址最大长度
        /// </summary>
        public const int AddressMaxLength = 256;

        /// <summary>
        /// 代码、编号最大长度
        /// </summary>
        public const int CodeMaxLength = 32;

        /// <summary>
        /// 更长的代码、编号最大长度
        /// </summary>
        public const int LongCodeMaxLength = 50;

        /// <summary>
        /// IP地址最大长度
        /// </summary>
        public const int IpAddressMaxLength = 64;

        /// <summary>
        /// 异常最大长度
        /// </summary>
        public const int ExceptionMaxLength = 2000;


        /// <summary>
        /// 项目日志名称最大长度
        /// </summary>
        public const int LogProjectNameMaxLangth = 64;

        /// <summary>
        /// 原因最大长度
        /// </summary>
        public const int ReasonLength = 30;

        /// <summary>
        /// 密码最大长度
        /// </summary>
        public const int PasswordLength = 30;

        /// <summary>
        /// 身份证正则表达式
        /// </summary>
        public const string IdcardVerify = @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$";
    }
}