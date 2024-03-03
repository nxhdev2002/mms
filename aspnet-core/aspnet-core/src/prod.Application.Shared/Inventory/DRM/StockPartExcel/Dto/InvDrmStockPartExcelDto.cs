using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.DRM.StockPartExcel.Dto
{
    public class InvDrmStockPartExcelDto : EntityDto<long?>
    {
        public virtual string Model { get; set; }

        public virtual string Grade { get; set; }
        public virtual string SupplierNo { get; set; }

        public virtual string ItemCode { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartCode { get; set; }

        public virtual long? DrmMaterialId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string GradeName { get; set; }

        public virtual int? Use { get; set; }

        public virtual int? Press { get; set; }

        public virtual int? IhpOh { get; set; }

        public virtual int? PressBroken { get; set; }

        public virtual int? Hand { get; set; }

        public virtual int? HandOh { get; set; }

        public virtual int? HandBroken { get; set; }

        public virtual int? MaterialIn { get; set; }

        public virtual int? MaterialInAddition { get; set; }

        public virtual string Shift { get; set; }
    }

    public class GetInvDrmStockPartExcelInput : PagedAndSortedResultRequestDto
    {

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

    }


    public class InvDrmStockPartExcelImportDto
    {
        public virtual long? ROW_NO { get; set; }

        public virtual string Guid { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartCode { get; set; }

        public virtual int? Qty { get; set; }

        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        //
        public virtual string Model { get; set; }

        public virtual string GradeName { get; set; }

        public virtual int? UsePress { get; set; }

        public virtual int? Press { get; set; }

        public virtual int? IhpOh { get; set; }

        public virtual int? PressBroken { get; set; }

        public virtual int? Hand { get; set; }

        public virtual int? HandOh { get; set; }

        public virtual int? HandBroken { get; set; }

        public virtual int? MaterialIn { get; set; }

        public virtual int? MaterialInAddition { get; set; }

        public virtual int? QtyLastStock { get; set; }

        public virtual int? IhpOhLastStock { get; set; }

        public virtual int? HandOhLastStock { get; set; }

        public virtual string Shift { get; set; }

        public virtual string ErrorDescription { get; set; }

    }


    public class InvDrmStockPartExcelDetailDto 
    {
        public virtual string KeyRow
        {
            get { return string.Format("{0}_{1}_{2}_{3}_{4}", Model, PartNo, Grade, PartCode, ItemOrder);  }
            set { }
        }
        public virtual string Model { get; set; }
        public virtual string PartNo { get; set; } 
        public virtual string Grade { get; set; }
        public virtual string PartCode { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual string ItemOrder { get; set; }

        public virtual int? N01_Ca1 { get; set; }
        public virtual int? N01_Ca2 { get; set; }
        public virtual int? N01_Ca3 { get; set; }
        public virtual int? N02_Ca1 { get; set; }
        public virtual int? N02_Ca2 { get; set; }
        public virtual int? N02_Ca3 { get; set; }
        public virtual int? N03_Ca1 { get; set; }
        public virtual int? N03_Ca2 { get; set; }
        public virtual int? N03_Ca3 { get; set; }
        public virtual int? N04_Ca1 { get; set; }
        public virtual int? N04_Ca2 { get; set; }
        public virtual int? N04_Ca3 { get; set; }
        public virtual int? N05_Ca1 { get; set; }
        public virtual int? N05_Ca2 { get; set; }
        public virtual int? N05_Ca3 { get; set; }
        public virtual int? N06_Ca1 { get; set; }
        public virtual int? N06_Ca2 { get; set; }
        public virtual int? N06_Ca3 { get; set; }
        public virtual int? N07_Ca1 { get; set; }
        public virtual int? N07_Ca2 { get; set; }
        public virtual int? N07_Ca3 { get; set; }
        public virtual int? N08_Ca1 { get; set; }
        public virtual int? N08_Ca2 { get; set; }
        public virtual int? N08_Ca3 { get; set; }
        public virtual int? N09_Ca1 { get; set; }
        public virtual int? N09_Ca2 { get; set; }
        public virtual int? N09_Ca3 { get; set; }
        public virtual int? N10_Ca1 { get; set; }
        public virtual int? N10_Ca2 { get; set; }
        public virtual int? N10_Ca3 { get; set; }
        public virtual int? N11_Ca1 { get; set; }
        public virtual int? N11_Ca2 { get; set; }
        public virtual int? N11_Ca3 { get; set; }
        public virtual int? N12_Ca1 { get; set; }
        public virtual int? N12_Ca2 { get; set; }
        public virtual int? N12_Ca3 { get; set; }
        public virtual int? N13_Ca1 { get; set; }
        public virtual int? N13_Ca2 { get; set; }
        public virtual int? N13_Ca3 { get; set; }
        public virtual int? N14_Ca1 { get; set; }
        public virtual int? N14_Ca2 { get; set; }
        public virtual int? N14_Ca3 { get; set; }
        public virtual int? N15_Ca1 { get; set; }
        public virtual int? N15_Ca2 { get; set; }
        public virtual int? N15_Ca3 { get; set; }
        public virtual int? N16_Ca1 { get; set; }
        public virtual int? N16_Ca2 { get; set; }
        public virtual int? N16_Ca3 { get; set; }
        public virtual int? N17_Ca1 { get; set; }
        public virtual int? N17_Ca2 { get; set; }
        public virtual int? N17_Ca3 { get; set; }
        public virtual int? N18_Ca1 { get; set; }
        public virtual int? N18_Ca2 { get; set; }
        public virtual int? N18_Ca3 { get; set; }
        public virtual int? N19_Ca1 { get; set; }
        public virtual int? N19_Ca2 { get; set; }
        public virtual int? N19_Ca3 { get; set; }
        public virtual int? N20_Ca1 { get; set; }
        public virtual int? N20_Ca2 { get; set; }
        public virtual int? N20_Ca3 { get; set; }
        public virtual int? N21_Ca1 { get; set; }
        public virtual int? N21_Ca2 { get; set; }
        public virtual int? N21_Ca3 { get; set; }
        public virtual int? N22_Ca1 { get; set; }
        public virtual int? N22_Ca2 { get; set; }
        public virtual int? N22_Ca3 { get; set; }
        public virtual int? N23_Ca1 { get; set; }
        public virtual int? N23_Ca2 { get; set; }
        public virtual int? N23_Ca3 { get; set; }
        public virtual int? N24_Ca1 { get; set; }
        public virtual int? N24_Ca2 { get; set; }
        public virtual int? N24_Ca3 { get; set; }
        public virtual int? N25_Ca1 { get; set; }
        public virtual int? N25_Ca2 { get; set; }
        public virtual int? N25_Ca3 { get; set; }
        public virtual int? N26_Ca1 { get; set; }
        public virtual int? N26_Ca2 { get; set; }
        public virtual int? N26_Ca3 { get; set; }
        public virtual int? N27_Ca1 { get; set; }
        public virtual int? N27_Ca2 { get; set; }
        public virtual int? N27_Ca3 { get; set; }
        public virtual int? N28_Ca1 { get; set; }
        public virtual int? N28_Ca2 { get; set; }
        public virtual int? N28_Ca3 { get; set; }
        public virtual int? N29_Ca1 { get; set; }
        public virtual int? N29_Ca2 { get; set; }
        public virtual int? N29_Ca3 { get; set; }
        public virtual int? N30_Ca1 { get; set; }
        public virtual int? N30_Ca2 { get; set; }
        public virtual int? N30_Ca3 { get; set; }
        public virtual int? N31_Ca1 { get; set; }
        public virtual int? N31_Ca2 { get; set; }
        public virtual int? N31_Ca3 { get; set; }

        public virtual string PartId { get; set; }
        public virtual string DrmPartListId { get; set; }


    }

    public class InvDrmStockPartDetailGridviewRowDto
    {
        public virtual string KeyRow
        {
            get { return string.Format("{0}_{1}_{2}_{3}", Model, PartNo, GradeName, PartCode); }
            set { }
        }
        public virtual string Model { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string GradeName { get; set; }
        public virtual string PartCode { get; set; }
        public virtual string ItemCode_8 { get; set; }    // == ItemOrder
        public virtual string ItemCode_6 { get; set; }    // == ItemOrder
        public virtual string ItemCode_3 { get; set; }    // == ItemOrder
        //public virtual DateTime? WorkingDate { get; set; }
        //public virtual string Shift { get; set; }
        public virtual string ColsId { get; set; }
        
    }

    public class GetShiftDto
    {
        public virtual int? Shift { get; set; }
    }

    public class InvDrmStockPartExcelDetailInputDto : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? WorkingDate { get; set; }

    }

    public class InvDrmStockPartExcelDetailSearchInputDto : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string Model { get; set; }
        public virtual string PartCode { get; set; } //bảng inhouse
        public virtual string Materialcode { get; set; } // dm 
        public virtual string PartNo { get; set; }
    }

    public class InvDrmStockPartStockUpdateTransDto
    {
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string PartId { get; set; }
        public virtual string DrmPartListId { get; set; }
        public virtual string ItemOrder { get; set; }
        public virtual string Shift { get; set; }
        public virtual string Value { get; set; } 
    }

    public class GetInvDrmStockPartExcelExportInput
    {
        public virtual DateTime? WorkingDate { get; set; }

    }
}
