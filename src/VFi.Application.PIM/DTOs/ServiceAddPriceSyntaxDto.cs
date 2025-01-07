using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.DTOs
{
    public class ServiceAddPriceSyntaxDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// Kiểu giá trị: 0-Số, 1-Tiền tệ, 2-Phần trăm, 3-Chữ, 4-Ngày
        /// </summary>
        public int TypeValue { get; set; }
        public string? Formula { get; set; }
    }

    public class ServiceAddPriceSyntaxCombobox: ListBoxDto
    {
        public string? Type { get; set; }
        public string? Description { get; set; }
    }
}
