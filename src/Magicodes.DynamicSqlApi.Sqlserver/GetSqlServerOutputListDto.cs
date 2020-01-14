namespace Magicodes.DynamicSqlApi.SqlServer
{
    public class GetSqlServerOutputListDto
    {
        /// <summary>
        /// 指示列是出于浏览信息目的而额外添加的列，该列不会实际显示在结果集中。
        /// </summary>
        public bool is_hidden { get; set; }

        /// <summary>
        /// 包含列的名称（如果可以确定名称）。 否则，它将包含 NULL。
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 如果列允许 NULL，则包含值 1；如果列不允许 NULL，则包含 0；如果不能确定列是否允许 NULL，则为 1。
        /// </summary>
        public bool is_nullable { get; set; }

        /// <summary>
        /// 包含为列数据类型指定的名称和参数（例如，length、precision、scale）。 如果数据类型是用户定义的别名类型，则会在此处指定基本系统类型。 如果数据类型是 CLR 用户定义类型，则在此列中返回 NULL。
        /// </summary>
        public string system_type_name { get; set; }

        /// <summary>
        /// 列的最大长度（字节）。-1 = 的列数据类型为varchar （max) ， nvarchar （max) ， varbinary （max) ，或者xml。有关文本列， max_length值将是 16，或者设置的值sp_tableoption 'text in row' 。
        /// </summary>
        public short max_length { get; set; }

        /// <summary>
        /// 如果为基于数值的列，则为该列的精度。 否则，返回 0。
        /// </summary>
        public short precision { get; set; }

        /// <summary>
        /// 如果基于数值，则为列的小数位数。 否则，返回 0。
        /// </summary>
        public short scale { get; set; }
    }
}