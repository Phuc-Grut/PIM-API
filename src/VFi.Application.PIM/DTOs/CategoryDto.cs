

using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace VFi.Application.PIM.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Web { get; set; }
        public string ParentCategoryName { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Keywords { get; set; }
        public string JsonData { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? GroupCategoryId { get; set; }
        public string SourceCode { get; set; }
        public string SourceLink { get; set; }
        public string ParentIds { get; set; }
        public int? Level { get; set; }
        public string GroupCategoryCode { get; set; }
        public string GroupCategoryName { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }

        public List<CategoryDto> Children { get; set; }
    }
    public class CategoryListBoxDto
    {
        public Guid Value { get; set; }
        public string Label { get; set; }
        public string? Key { get; set; }
        public bool Expanded { get; set; }
        public int? DisplayOrder { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public List<CategoryListBoxDto> Children { get; set; }
    }  
    public class CategoryComboboxDto
    {
        public Guid Value { get; set; }
        public string Label { get; set; }
        public string? Key { get; set; }
        public int? DisplayOrder { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
    public class CategoryQueryParams
    {
        public int? Status { get; set; }
        public string? GroupCategoryId { get; set; }
        public string? ParentCategoryId { get; set; }
    }
    public class CategoryParentDto
    {
        public Guid Id  { get; set; }
        public string Name { get; set; }
    }
    public class CategoryListViewDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Web { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string? ParentIds { get; set; }
        public Guid? GroupCategoryId { get; set; }
        public string? JsonData { get; set; }
        public JArray?  JsonObject { 
            get {
                //[{"name":"1","value":"1"},{"name":"2","value":"2"}]
                if (!string.IsNullOrEmpty(JsonData))
                return JArray.Parse(JsonData);
                else return null;
            } 
        }
        public string SourceCode { get; set; }
        public string SourceLink { get; set; }
        public int? Level { get; set; }
        public int DisplayOrder { get; set; }
        public int? Status { get; set; }

    }
}
