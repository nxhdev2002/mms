using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using prod.Master.LogA.Bp2PartListGrade;

namespace prod.Master.LogA.ImportDto
{
    public class ImportMstLgaBp2PartListGradeDto
    {
        [StringLength(MstLgaBp2ProcessConsts.MaxGuidLength)]
        public virtual string Guid { get; set; }

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

        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
