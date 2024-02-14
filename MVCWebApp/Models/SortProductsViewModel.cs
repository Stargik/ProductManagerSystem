using System;
using BLL.Models;

namespace MVCWebApp.Models
{
	public class SortProductsViewModel
	{
        public ProductSortState SortTitle { get; set; } 
        public ProductSortState SortManufacturerCode { get; set; }    
        public ProductSortState SortPrice { get; set; }   
        public ProductSortState Current { get; set; }
        public ProductSortState InverseCurrent { get; set; }
        public bool Up { get; set; }

        public SortProductsViewModel(ProductSortState sortOrder)
		{
            SortTitle = ProductSortState.TitleAsc;
            SortManufacturerCode = ProductSortState.ManufacturerCodeAsc;
            SortPrice = ProductSortState.PriceAsc;
            Up = true;
            InverseCurrent = sortOrder;

            if (sortOrder == ProductSortState.TitleDesc || sortOrder == ProductSortState.ManufacturerCodeDesc || sortOrder == ProductSortState.PriceDesc)
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
                case ProductSortState.PriceAsc:
                    Current = SortPrice = ProductSortState.PriceDesc;
                    break;
                case ProductSortState.PriceDesc:
                    Current = SortTitle = ProductSortState.PriceAsc;
                    break;
                default:
                    Current = ProductSortState.Default;
                    break;
            }
        }
	}
}

