using VFi.Domain.PIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.DTOs
{
    public class SortItemDto
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }
    }
}
