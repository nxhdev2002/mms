using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.LogA.Bp2PartListGrade.Dto
{
    public class MstLgaBp2PartListGradeDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual int? PartListId { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual int? UsageQty { get; set; }
        public virtual string PikLocType { get; set; }
        public virtual string PikAddress { get; set; }
        public virtual string PikAddressDisplay { get; set; }
        public virtual string DelLocType { get; set; }

        public virtual string DelAddress { get; set; }
        public virtual string DelAddressDisplay { get; set; }
        public virtual int? Sorting { get; set; }

        public virtual string Remark { get; set; }
        public virtual string IsActive { get; set; }

        public int A1 { get; set; }
        public int A2 { get; set; }
        public int A3 { get; set; }
        public int A4 { get; set; }
        public int A5 { get; set; }
        public int A6 { get; set; }
        public int A7 { get; set; }
        public int A8 { get; set; }
        public int A9 { get; set; }
        public int A10 { get; set; }


    }

    public class CreateOrEditMstLgaBp2PartListGradeDto : EntityDto<long?>
    {
        [StringLength(MstLgaBp2PartListGradeConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? PartListId { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxPikLocTypeLength)]
        public virtual string PikLocType { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxPikAddressLength)]
        public virtual string PikAddress { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxPikAddressDisplayLength)]
        public virtual string PikAddressDisplay { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxDelLocTypeLength)]
        public virtual string DelLocType { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxDelAddressLength)]
        public virtual string DelAddress { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxDelAddressDisplayLength)]
        public virtual string DelAddressDisplay { get; set; }

        public virtual int? Sorting { get; set; }

        [StringLength(MstLgaBp2PartListGradeConsts.MaxRemarkLength)]
        public virtual string Remark { get; set; }


        [StringLength(MstLgaBp2PartListGradeConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }


    }

    public class GetMstLgaBp2PartListGradeInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Grade { get; set; }

        public virtual string Model { get; set; }

        public virtual string PartName { get; set; }


    }


    public class GetMstLgaBp2PartListGradeExportInput 
    {

        public virtual string Model { get; set; }


    }
}
