using System;
using BLL.Models;

namespace MVCWebApp.Models
{
	public class SortProductsViewModel
	{
        public ProductSortState SortTitle { get; set; } 
        public ProductSortState SortManufacturerCode { get; set; }
        public ProductSortState SortCategory { get; set; }
        public ProductSortState SortManufacturer { get; set; }
        public ProductSortState SortPrice { get; set; }
        public ProductSortState SortStockStatus { get; set; }
        public ProductSortState Current { get; set; }
        public ProductSortState InverseCurrent { get; set; }
        public bool Up { get; set; }

        public SortProductsViewModel(ProductSortState sortOrder)
		{
            SortTitle = ProductSortState.TitleAsc;
            SortManufacturerCode = ProductSortState.ManufacturerCodeAsc;
            SortCategory = ProductSortState.CategoryAsc;
            SortManufacturer = ProductSortState.ManufacturerAsc;
            SortPrice = ProductSortState.PriceAsc;
            SortStockStatus = ProductSortState.StockStatusAsc;

            Up = true;
            InverseCurrent = sortOrder;

            if (sortOrder == ProductSortState.TitleDesc || sortOrder == ProductSortState.ManufacturerCodeDesc || sortOrder == ProductSortState.CategoryDesc
                || sortOrder == ProductSortState.ManufacturerDesc || sortOrder == ProductSortState.PriceDesc || sortOrder == ProductSortState.StockStatusDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case ProductSortState.TitleAsc:
                    Current = SortTitle = ProductSortState.TitleDesc;
                    break;
                case ProductSortState.TitleDesc:
                    Current = SortTitle = ProductSortState.TitleAsc;
                    break;
                case ProductSortState.ManufacturerCodeAsc:
                    Current = SortManufacturerCode = ProductSortState.ManufacturerCodeDesc;
                    break;
                case ProductSortState.ManufacturerCodeDesc:
                    Current = SortManufacturerCode = ProductSortState.ManufacturerCodeAsc;
                    break;
                case ProductSortState.CategoryAsc:
                    Current = SortCategory = ProductSortState.CategoryDesc;
                    break;
                case ProductSortState.CategoryDesc:
                    Current = SortCategory = ProductSortState.CategoryAsc;
                    break;
                case ProductSortState.ManufacturerAsc:
                    Current = SortManufacturer = ProductSortState.ManufacturerDesc;
                    break;
                case ProductSortState.ManufacturerDesc:
                    Current = SortManufacturer = ProductSortState.ManufacturerAsc;
                    break;
                case ProductSortState.PriceAsc:
                    Current = SortPrice = ProductSortState.PriceDesc;
                    break;
                case ProductSortState.PriceDesc:
                    Current = SortPrice = ProductSortState.PriceAsc;
                    break;
                case ProductSortState.StockStatusAsc:
                    Current = SortStockStatus = ProductSortState.StockStatusDesc;
                    break;
                case ProductSortState.StockStatusDesc:
                    Current = SortStockStatus = ProductSortState.StockStatusAsc;
                    break;
                default:
                    Current = ProductSortState.Default;
                    break;
            }
        }
	}
}

