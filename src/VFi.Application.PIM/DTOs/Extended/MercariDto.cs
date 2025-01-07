using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VFi.Application.PIM.DTOs.MercariDto;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Application.PIM.DTOs
{
    public class MercariDto
    {
        public class MercariProductPublish : ProductPublish
        {
            public MercariProductPublish()
            {
            }

            public string OriginCategoryId { get; set; }
            public string OriginCategory { get; set; }
            public string OriginCategoryIdPath { get; set; }
            public string OriginCategoryPath { get; set; }
            public bool IsFreeDomesticShipping { get; set; } = false;
            public decimal? DomesticShippingFee { get; set; }
            public int ConvertCondition(int merCondition)
            {
                /// <summary>
                /// New = 0, Refurbished = 10,20 = LikeNew,  Used = 30, Damaged = 40
                /// </summary>
                /// 
                switch (merCondition)
                {
                    case 1:
                        return 0;
                    case 2:
                        return 20;
                    case 3:
                        return 30;
                    case 4:
                        return 30;
                    case 5:
                        return 30;
                    case 6:
                        return 40;
                    default:
                        return 0;
                }
            }
            public void Convert(string content)
            {
                var meritem = JsonConvert.DeserializeObject<MercariDto.ProductItem>(content);

                this.Code = meritem.Data.Id;
                this.Name = meritem.Data.Name;
                this.Price = meritem.Data.Price;
                this.FullDescription = meritem.Data.Description;
                this.Image = meritem.Data.Thumbnails.Any() ? meritem.Data.Thumbnails.First() : "";
                this.Images = meritem.Data.Thumbnails;
                this.Currency = "JPY";
                this.TaxRate = 0;
                this.IsTaxExempt = false;
                this.Origin = "Japan";
                this.SourceLink = $"https://jp.mercari.com/item/{meritem.Data.Id}";
                this.SourceCode = meritem.Data.Id;
                this.Images = meritem.Data.Photos;
                this.Condition = this.ConvertCondition(meritem.Data.ItemCondition.Id);
                this.StockQuantity = 1;

                this.IsStocking = meritem.Data.Status == "trading" ? false : true;

                this.CanReturn = false;
                this.Brand = meritem.Data.ItemBrand != null ? meritem.Data.ItemBrand.SubName : "";
                if (meritem.Data.ItemSize != null)
                {
                    this.Attributes.Add(new ProductAttribute(meritem.Data.ItemSize.Id.ToString(), "Size", "Size", meritem.Data.ItemSize.Name));
                }
                if (meritem.Data.Colors.Any())
                {
                    this.Attributes.Add(new ProductAttribute("", "Color", "Color", string.Join(',', meritem.Data.Colors)));
                }
                if (meritem.Data.ItemAttributes.Any())
                {
                    foreach (var item in meritem.Data.ItemAttributes)
                    {
                        var prodAttr = new ProductAttribute();
                        prodAttr.Id = item.Id;
                        prodAttr.Code = item.Text;
                        prodAttr.Name = item.Text;
                        prodAttr.Value = string.Join(',', item.Values.Select(x => x.Text).ToList());
                        this.Attributes.Add(prodAttr);
                    }
                }
                this.IsFreeDomesticShipping = meritem.Data.ShippingPayer != null && meritem.Data.ShippingPayer.Code == "seller" ? true : false;
                if (meritem.Data.ItemCategory != null)
                {
                    var categoryId = meritem.Data.ItemCategory.Id.ToString();
                    var category = meritem.Data.ItemCategory.Name;
                    var parentcategoryId = meritem.Data.ItemCategory.ParentCategoryId.ToString();
                    var parentcategory = meritem.Data.ItemCategory.ParentCategoryName;
                    var rootcategoryId = meritem.Data.ItemCategory.RootCategoryId.ToString();
                    var rootcategory = meritem.Data.ItemCategory.RootCategoryName;
                    this.OriginCategoryId = categoryId;
                    this.OriginCategory = category;
                    this.OriginCategoryIdPath = $"{rootcategoryId},{parentcategoryId},{categoryId}";
                    this.OriginCategoryPath = $"{rootcategory},{parentcategory},{category}";
                }
            }
        }
        public class ProductItem
        {
            public string Result { get; set; }
            public DetailItem Data { get; set; }
            public Meta Meta { get; set; }
        }
        public class ProductList
        {
            public string Result { get; set; }
            public Meta Meta { get; set; }
            public List<ListItem> Data { get; set; }
        }
        public class ProductListSearch
        {
            public Meta Meta { get; set; }
            public List<ListItem> Items { get; set; }
        }
        public class DetailItem
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("seller")]
            public Seller Seller { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("price")]
            public long Price { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("photos")]
            public List<string> Photos { get; set; } = new List<string>();

            [JsonProperty("thumbnails")]
            public List<string> Thumbnails { get; set; } = new List<string>();

            [JsonProperty("item_category")]
            public ItemCategory ItemCategory { get; set; }

            [JsonProperty("item_condition")]
            public ItemCondition ItemCondition { get; set; }

            [JsonProperty("item_size")]
            public ItemSize ItemSize { get; set; }

            [JsonProperty("item_brand")]
            public ItemBrand ItemBrand { get; set; }

            [JsonProperty("colors")]
            public List<string> Colors { get; set; } = new List<string>();

            [JsonProperty("shipping_payer")]
            public ShippingPayer ShippingPayer { get; set; }

            [JsonProperty("shipping_method")]
            public ShippingMethod ShippingMethod { get; set; }

            [JsonProperty("shipping_from_area")]
            public ShippingFromArea ShippingFromArea { get; set; }

            [JsonProperty("shipping_duration")]
            public ShippingDuration ShippingDuration { get; set; }

            [JsonProperty("shipping_class")]
            public ShippingClass ShippingClass { get; set; }

            [JsonProperty("num_likes")]
            public int NumLikes { get; set; }

            [JsonProperty("num_comments")]
            public int NumComments { get; set; }
            //[JsonProperty("comments")]
            //public List<string> Comments { get; set; } = new List<string>();

            [JsonProperty("updated")]
            public long Updated { get; set; }

            [JsonProperty("created")]
            public long Created { get; set; }

            [JsonProperty("pager_id")]
            public long PagerId { get; set; }

            [JsonProperty("liked")]
            public bool Liked { get; set; }

            [JsonProperty("is_dynamic_shipping_fee")]
            public bool is_dynamic_shipping_fee { get; set; }

            [JsonProperty("is_shop_item")]
            public string is_shop_item { get; set; }

            [JsonProperty("is_anonymous_shipping")]
            public bool is_anonymous_shipping { get; set; }

            [JsonProperty("is_web_visible")]
            public bool is_web_visible { get; set; }

            [JsonProperty("item_attributes")]
            public List<ItemAttribute> ItemAttributes { get; set; }     =   new List<ItemAttribute> { };
            

            [JsonProperty("is_offerable")]
            public bool is_offerable { get; set; }
            [JsonProperty("is_organizational_user")]
            public bool is_organizational_user { get; set; }

            [JsonProperty("is_stock_item")]
            public bool is_stock_item { get; set; }

            [JsonProperty("is_cancelable")]
            public bool is_cancelable { get; set; }
            [JsonProperty("shipped_by_worker")]
            public bool shipped_by_worker { get; set; }

            [JsonProperty("additional_services")]
            public List<string> additional_services { get; set; } = new List<string>();
            [JsonProperty("has_additional_service")]
            public bool has_additional_service { get; set; }

            [JsonProperty("has_like_list")]
            public bool has_like_list { get; set; }
            [JsonProperty("is_offerable_v2")]
            public bool is_offerable_v2 { get; set; }
            [JsonProperty("is_dismissed")]
            public bool is_dismissed { get; set; }


        }


        public class ItemCondition
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
        public class ListItem
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("sellerId")]
            public string SellerId { get; set; }

            [JsonProperty("buyerId")]
            public string BuyerId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("price")]
            public long Price { get; set; }

            [JsonProperty("created")]
            public long Created { get; set; }

            [JsonProperty("updated")]
            public long Updated { get; set; }

            [JsonProperty("thumbnails")]
            public List<string> Thumbnails { get; set; } = new List<string>();

            [JsonProperty("itemType")]
            public string ItemType { get; set; }

            [JsonProperty("itemConditionId")]
            public int ItemConditionId { get; set; }

            [JsonProperty("shippingPayerId")]
            public int ShippingPayerId { get; set; }

            [JsonProperty("itemSizes", NullValueHandling = NullValueHandling.Ignore)]
            public List<ItemSize> ItemSizes { get; set; } = new List<ItemSize>();

            [JsonProperty("item_brand", NullValueHandling = NullValueHandling.Ignore)]
            public ItemBrand ItemBrand { get; set; } = new ItemBrand();

            [JsonProperty("shopName")]
            public string ShopName { get; set; }

            [JsonProperty("itemSize", NullValueHandling = NullValueHandling.Ignore)]
            public ItemSize ItemSize { get; set; } = new ItemSize();

        }
        public class ItemSize
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
        public class Buyer
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
        public class Seller
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("photo_url")]
            public string PhotoUrl { get; set; }

            [JsonProperty("photo_thumbnail_url")]
            public string PhotoThumbnailUrl { get; set; }

            [JsonProperty("register_sms_confirmation")]
            public string RegisterSmsConfirmation { get; set; }

            [JsonProperty("register_sms_confirmation_at")]
            public DateTime RegisterSmsConfirmationAt { get; set; }

            [JsonProperty("created")]
            public long Created { get; set; }

            [JsonProperty("num_sell_items")]
            public int NumSellItems { get; set; }

            [JsonProperty("ratings")]
            public Ratings Ratings { get; set; }

            [JsonProperty("num_ratings")]
            public int NumRatings { get; set; }

            [JsonProperty("score")]
            public int Score { get; set; }

            [JsonProperty("is_official")]
            public bool IsOfficial { get; set; }

            [JsonProperty("quick_shipper")]
            public bool QuickShipper { get; set; }

            [JsonProperty("star_rating_score")]
            public long StarRatingScore { get; set; }
        }
        public class Ratings
        {
            [JsonProperty("good")]
            public int Good { get; set; }

            [JsonProperty("normal")]
            public int Normal { get; set; }

            [JsonProperty("bad")]
            public int Bad { get; set; }
        }
        public class ItemBrand
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("sub_name")]
            public string SubName { get; set; }
        }
        public class ItemCategory
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("display_order")]
            public long DisplayOrder { get; set; }

            [JsonProperty("parent_category_id")]
            public long ParentCategoryId { get; set; }

            [JsonProperty("parent_category_name")]
            public string ParentCategoryName { get; set; }

            [JsonProperty("root_category_id")]
            public long RootCategoryId { get; set; }

            [JsonProperty("root_category_name")]
            public string RootCategoryName { get; set; }
        }
        public class Meta
        {
            [JsonProperty("has_next")]
            public bool HasNext { get; set; }

            [JsonProperty("next_page")]
            public long NextPage { get; set; }

            [JsonProperty("sort")]
            public string Sort { get; set; }

            [JsonProperty("order")]
            public string Order { get; set; }

            [JsonProperty("requested")]
            public long Requested { get; set; }

            [JsonProperty("exec_time")]
            public double ExecTime { get; set; }

            [JsonProperty("nextPageToken")]
            public string NextPageToken { get; set; }

            [JsonProperty("previousPageToken")]
            public string PreviousPageToken { get; set; }

            [JsonProperty("numFound")]
            public long NumFound { get; set; }
        }

        public class ShippingPayer
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }
        }
        public class ShippingMethod
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("is_deprecated")]
            public string IsDeprecatedode { get; set; }
        }
        public class ShippingFromArea
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
        public class ShippingDuration
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("min_days")]
            public int MinDays { get; set; }
            [JsonProperty("max_days")]
            public int MaxDays { get; set; }
        }
        public class ShippingClass
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("fee")]
            public int Fee { get; set; }

            [JsonProperty("icon_id")]
            public int IconId { get; set; }

            [JsonProperty("pickup_fee")]
            public int PickupFee { get; set; }
            [JsonProperty("total_fee")]
            public int TotalFee { get; set; }
            [JsonProperty("is_pickup")]
            public bool IsPickup { get; set; }
        }
        public class ItemAttribute
        {
            [JsonProperty("deep_facet_filterable")]
            public bool DeepFacetFilterable { get; set; }
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("values")]
            public List<ItemValue> Values {get;set; }   = new List<ItemValue>();
        }
        public class ItemValue
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("text")]
            public string Text { get; set; }

        }
}
}
