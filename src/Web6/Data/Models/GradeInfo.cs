using System.ComponentModel.DataAnnotations;

namespace Web6.Data.Models
{
    public class GradeInfo
    {
        public int Id { get; set; }

        /// <summary>
        /// 年级名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
    }
}