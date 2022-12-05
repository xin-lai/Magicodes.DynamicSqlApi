using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Linq;

namespace Web6.Data.Models
{
    public class ClassInfo
    {
        public int Id { get; set; }
        /// <summary>
        ///班级名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 所属专业id
        /// </summary>
        [Required(ErrorMessage = "所属专业不能为空")]
        public int MajorId { get; set; }

        /// <summary>
        /// 专业信息
        /// </summary>
        [ForeignKey("MajorId")]
        public virtual MajorInfo Major { get; set; }

        /// <summary>
        /// 所属年级id
        /// </summary>
        public int GradeId { get; set; }

        /// <summary>
        /// 开班日期
        /// </summary>
        [Display(Name = "开班日期")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 年级信息
        /// </summary>
        [ForeignKey("GradeId")]
        public virtual GradeInfo Grade { get; set; }

    }
}
