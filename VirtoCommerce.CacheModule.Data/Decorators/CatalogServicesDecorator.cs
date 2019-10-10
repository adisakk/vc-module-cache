using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;

namespace VirtoCommerce.CacheModule.Data.Decorators
{
    public sealed class CatalogServicesDecorator : ICachedServiceDecorator, IItemService, ICatalogSearchService, IPropertyService, ICategoryService, ICatalogService
    {
        private readonly IItemService _itemService;
        private readonly ICatalogSearchService _searchService;
        private readonly IPropertyService _propertyService;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly CacheManagerAdaptor _cacheManager;
        public const string RegionName = "Catalog-Cache-Region";

        public CatalogServicesDecorator(IItemService itemService, ICatalogSearchService searchService, IPropertyService propertyService, ICategoryService categoryService, ICatalogService catalogService, CacheManagerAdaptor cacheManager)
        {
            _itemService = itemService;
            _searchService = searchService;
            _propertyService = propertyService;
            _categoryService = categoryService;
            _catalogService = catalogService;
            _cacheManager = cacheManager;
        }

        #region ICachedServiceDecorator
        public void ClearCache()
        {
            _cacheManager.ClearRegion(RegionName);
        }
        #endregion

        #region IItemService members
        public void Create(CatalogProduct[] items)
        {
            _itemService.Create(items);
            ClearCache();
        }

        public CatalogProduct Create(CatalogProduct item)
        {
            var retVal = _itemService.Create(item);
            ClearCache();
            return retVal;
        }

        public void Delete(string[] itemIds)
        {
            _itemService.Delete(itemIds);
            ClearCache();
        }

        public CatalogProduct GetById(string itemId, ItemResponseGroup respGroup, string catalogId = null)
        {
            var cacheKey = GetCacheKey("ItemService.GetById", itemId, respGroup.ToString(), catalogId);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _itemService.GetById(itemId, respGroup, catalogId));
            return retVal;
        }

        public CatalogProduct[] GetByIds(string[] itemIds, ItemResponseGroup respGroup, string catalogId = null)
        {
            var cacheKey = GetCacheKey("ItemService.GetByIds", string.Join(", ", itemIds), respGroup.ToString(), catalogId);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _itemService.GetByIds(itemIds, respGroup, catalogId));
            return retVal;
        }

        public CatalogProduct GetByCode(string itemCode, ItemResponseGroup respGroup, string catalogId = null)
        {
            var cacheKey = GetCacheKey("ItemService.GetByCode", itemCode, respGroup.ToString(), catalogId);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _itemService.GetByCode(itemCode, respGroup, catalogId));
            return retVal;
        }

        public CatalogProduct[] GetByCodes(string[] itemCodes, ItemResponseGroup respGroup, string catalogId = null)
        {
            var cacheKey = GetCacheKey("ItemService.GetByCodes", string.Join(", ", itemCodes), respGroup.ToString(), catalogId);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _itemService.GetByCodes(itemCodes, respGroup, catalogId));
            return retVal;
        }

        public void Update(CatalogProduct[] items)
        {
            _itemService.Update(items);
            ClearCache();
        }
        #endregion

        #region ICatalogSearchService members
        public SearchResult Search(SearchCriteria criteria)
        {
            var cacheKey = GetCacheKey("CatalogSearchService.Search", criteria.GetCacheKey());
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _searchService.Search(criteria));
            return retVal;
        }
        #endregion

        #region IPropertyService members
        public Property GetById(string propertyId)
        {
            var cacheKey = GetCacheKey("PropertyService.GetById", string.Join(", ", propertyId));
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _propertyService.GetById(propertyId));
            return retVal;
        }

        public Property[] GetByIds(string[] propertyIds)
        {
            var cacheKey = GetCacheKey("PropertyService.GetByIds", string.Join(", ", propertyIds));
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _propertyService.GetByIds(propertyIds));
            return retVal;
        }

        public Property Create(Property property)
        {
            var retVal = _propertyService.Create(property);
            ClearCache();
            return retVal;
        }

        public void Update(Property[] properties)
        {
            _propertyService.Update(properties);
            ClearCache();
        }

        public Property[] GetAllCatalogProperties(string catalogId)
        {
            var cacheKey = GetCacheKey("PropertyService.GetAllCatalogProperties", catalogId);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _propertyService.GetAllCatalogProperties(catalogId));
            return retVal;
        }

        public Property[] GetAllProperties()
        {
            var cacheKey = GetCacheKey("PropertyService.GetAllProperties");
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _propertyService.GetAllProperties());
            return retVal;
        }

        public PropertyDictionaryValue[] SearchDictionaryValues(string propertyId, string keyword)
        {
            var cacheKey = GetCacheKey("PropertyService.SearchDictionaryValues", propertyId, keyword);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _propertyService.SearchDictionaryValues(propertyId, keyword));
            return retVal;
        }

        void IPropertyService.Delete(string[] propertyIds)
        {
            _propertyService.Delete(propertyIds);
            ClearCache();
        }
        #endregion

        #region ICategoryService Members
        public Category[] GetByIds(string[] categoryIds, CategoryResponseGroup responseGroup, string catalogId = null)
        {
            var cacheKey = GetCacheKey("CategoryService.GetByIds", string.Join(", ", categoryIds), responseGroup.ToString(), catalogId);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _categoryService.GetByIds(categoryIds, responseGroup, catalogId));
            return retVal;
        }

        public Category GetById(string categoryId, CategoryResponseGroup responseGroup, string catalogId = null)
        {
            var cacheKey = GetCacheKey("CategoryService.GetById", categoryId, responseGroup.ToString(), catalogId);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _categoryService.GetById(categoryId, responseGroup, catalogId));
            return retVal;
        }

        public void Create(Category[] categories)
        {
            _categoryService.Create(categories);
            ClearCache();
        }

        public Category Create(Category category)
        {
            var retVal = _categoryService.Create(category);
            ClearCache();
            return retVal;
        }

        void ICategoryService.Delete(string[] categoryIds)
        {
            _categoryService.Delete(categoryIds);
            ClearCache();
        }

        public void Update(Category[] categories)
        {
            _categoryService.Update(categories);
            ClearCache();
        }
        #endregion

        #region ICatalogService members
        public IEnumerable<Catalog> GetCatalogsList()
        {
            var cacheKey = GetCacheKey("CatalogService.GetCatalogsList");
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _catalogService.GetCatalogsList().ToArray());
            return retVal;
        }

        void ICatalogService.Delete(string[] catalogIds)
        {
            _catalogService.Delete(catalogIds);
            ClearCache();
        }

        Catalog ICatalogService.GetById(string catalogId)
        {
            var cacheKey = GetCacheKey("CatalogService.GetById", catalogId);
            var retVal = _cacheManager.Get(cacheKey, RegionName, () => _catalogService.GetById(catalogId));
            return retVal;
        }

        public Catalog Create(Catalog catalog)
        {
            var retVal = _catalogService.Create(catalog);
            ClearCache();
            return retVal;
        }

        public void Update(Catalog[] catalogs)
        {
            _catalogService.Update(catalogs);
            ClearCache();
        }
        #endregion

        private static string GetCacheKey(params string[] parameters)
        {
            return "Catalog-" + string.Join(", ", parameters);
        }
    }
}
