using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web6.Data.Models
{
    public class MajorInfo
    {
        public int Id { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 学年制
        /// </summary>
        [Required(ErrorMessage = "学年制不能为空")]
        public double YearSystem { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}