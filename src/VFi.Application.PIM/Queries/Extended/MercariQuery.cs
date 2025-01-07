using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.NetDevPack.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VFi.Application.PIM.DTOs.MercariDto;

namespace VFi.Application.PIM.Queries
{
    public class MercariItemQuery : IQuery<MercariProductPublish>
    {
        public MercariItemQuery(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
    public class MercariHomeItemQuery : IQuery<IList<ProductListView>>
    {
        public MercariHomeItemQuery(int take)
        {
            Take = take;
        }

        public int Take { get; set; }
    }
    public class MercariSearchQuery : IQuery<PagingResponse<ProductListView>>
    {
        public MercariSearchQuery(String keyword, int? categoryId)
        {
            Keyword = keyword;
            CategoryId = categoryId;
        }

        public MercariSearchQuery(String keyword, int? categoryId, int pageSize, int pageIndex)
        {
            Keyword = keyword;
            CategoryId = categoryId;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public String Keyword { get; set; }
        public int? CategoryId { get; set; }
        public int PageSize { get; set; } = 120;
        public int PageIndex { get; set; } = 1;
    }
    public class MercariQueryHandler : IQueryHandler<MercariItemQuery, MercariProductPublish>, IQueryHandler<MercariHomeItemQuery, IList<ProductListView>>, IQueryHandler<MercariSearchQuery, PagingResponse<ProductListView>>
    {
        private readonly IMercariRepository _mercariRepository;

        public MercariQueryHandler(IMercariRepository mercariRepository)
        {
            _mercariRepository = mercariRepository;
        }



        public async Task<MercariProductPublish> Handle(MercariItemQuery request, CancellationToken cancellationToken)
        {
            var result = new MercariProductPublish();
            var strdata = await _mercariRepository.GetItem(request.ItemId);
            var meritem= JsonConvert.DeserializeObject<MercariDto.ProductItem>(strdata);

            result.Code = meritem.Data.Id;
            result.Name = meritem.Data.Name;
            result.Price = meritem.Data.Price;
            result.FullDescription = meritem.Data.Description;
            result.Image = meritem.Data.Thumbnails.Any() ? meritem.Data.Thumbnails.First() : "";
            result.Images = meritem.Data.Thumbnails;
            result.Currency = "JPY";
            result.TaxRate = 0;
            result.IsTaxExempt = false;
            result.Origin = "Japan";
            result.SourceLink = $"https://jp.mercari.com/item/{meritem.Data.Id}";
            result.SourceCode = meritem.Data.Id;
            result.Images = meritem.Data.Photos;
            result.Condition =result.ConvertCondition(meritem.Data.ItemCondition.Id);
            result.StockQuantity = 1;

            result.IsStocking= meritem.Data.Status== "trading"? false:true;

            result.CanReturn = false;
            result.Brand = meritem.Data.ItemBrand!=null? meritem.Data.ItemBrand.SubName:"";
            if (meritem.Data.ItemSize != null)
            {
                result.Attributes.Add(new ProductAttribute(meritem.Data.ItemSize.Id.ToString(), "Size","Size", meritem.Data.ItemSize.Name));
            }
            if (meritem.Data.Colors.Any())
            {
                result.Attributes.Add(new ProductAttribute("", "Color", "Color", string.Join(',', meritem.Data.Colors)));
            }
            if (meritem.Data.ItemAttributes.Any())
            {
                foreach(var item in meritem.Data.ItemAttributes)
                {
                    var prodAttr = new ProductAttribute();
                    prodAttr.Id= item.Id;
                    prodAttr.Code = item.Text;
                    prodAttr.Name = item.Text;
                    prodAttr.Value = string.Join(',',item.Values.Select(x=>x.Text).ToList());
                    result.Attributes.Add(prodAttr);
                }
            }
            result.IsFreeDomesticShipping = meritem.Data.ShippingPayer != null && meritem.Data.ShippingPayer.Code== "seller" ? true:false;
            if (meritem.Data.ItemCategory != null)
            {
                var categoryId = meritem.Data.ItemCategory.Id.ToString();
                var category = meritem.Data.ItemCategory.Name;
                var parentcategoryId = meritem.Data.ItemCategory.ParentCategoryId.ToString();
                var parentcategory = meritem.Data.ItemCategory.ParentCategoryName;
                var rootcategoryId = meritem.Data.ItemCategory.RootCategoryId.ToString();
                var rootcategory = meritem.Data.ItemCategory.RootCategoryName;
                result.OriginCategoryId= categoryId; 
                result.OriginCategory = category;
                result.OriginCategoryIdPath = $"{rootcategoryId},{parentcategoryId},{categoryId}";
                result.OriginCategoryPath = $"{rootcategory},{parentcategory},{category}";
            }

            return result;
        }

        public async Task<IList<ProductListView>> Handle(MercariHomeItemQuery request, CancellationToken cancellationToken)
        {
            var result = new List<ProductListView>();
            var strdata = await _mercariRepository.GetHomeItem(request.Take);
            var merdata = JsonConvert.DeserializeObject<MercariDto.ProductList>(strdata);
            if (merdata.Result.Equals("OK"))
            {
                foreach (var merItem in merdata.Data)
                {
                    var productItem = new ProductListView();
                    productItem.Name = merItem.Name;
                    productItem.Code = merItem.Id;
                    productItem.Price = merItem.Price;
                    productItem.Image = merItem.Thumbnails.Any() ? merItem.Thumbnails.First() : "";
                    productItem.Currency = "JPY";
                    productItem.TaxRate = 0;
                    productItem.IsTaxExempt = false;
                    productItem.Origin = "Japan";
                    productItem.SourceLink = $"https://jp.mercari.com/item/{merItem.Id}";
                    productItem.SourceCode = merItem.Id;
                    result.Add(productItem);
                }
            }

            return result;
        }

        public async Task<PagingResponse<ProductListView>> Handle(MercariSearchQuery request, CancellationToken cancellationToken)
        {
            var items = new List<ProductListView>();
            var result = new PagingResponse<ProductListView>();
            var filter = new Dictionary<string, object>();
            if(request.CategoryId.HasValue)  filter.Add("categoryId", request.CategoryId.Value);
            var strdata = await _mercariRepository.Filter(request.Keyword, filter,request.PageIndex);
            var merdata = JsonConvert.DeserializeObject<MercariDto.ProductListSearch>(strdata); 
            foreach (var merItem in merdata.Items)
            {
                var productItem = new ProductListView();
                productItem.Name = merItem.Name;
                productItem.Code = merItem.Id;
                productItem.Price = merItem.Price;
                productItem.Image = merItem.Thumbnails.Any() ? merItem.Thumbnails.First() : "";
                productItem.Currency = "JPY";
                productItem.TaxRate = 0;
                productItem.IsTaxExempt = false;
                productItem.Origin = "Japan";
                productItem.SourceLink = $"https://jp.mercari.com/item/{merItem.Id}";
                productItem.SourceCode = merItem.Id;
                items.Add(productItem);
            }
            result.Items = items;
            result.PageIndex = request.PageIndex;
            result.PageSize = 120;
            result.Total = merdata.Meta.NumFound; result.Count = merdata.Meta.NumFound;
            result.Successful();
            return result;
        }
    }
}
