

using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Application.PIM.DTOs
{
    public class ComboboxGuidStringDto
    {
        public Guid Value { get; set; }
        public string Label { get; set; }
    }
    public class ListIdDto
    {
        public Guid Id { get; set; }
    }
}
