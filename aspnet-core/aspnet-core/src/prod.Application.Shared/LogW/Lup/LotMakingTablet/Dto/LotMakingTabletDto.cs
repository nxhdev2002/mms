using System;
using System.Collections.Generic;
using System.Text;

namespace prod.LogW.Lup.Dto
{
    public class LotMakingTabletDto
    {
        public virtual string SeqNo { get; set; }
        public virtual int? Id { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string Shift { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string PlanUnpackingStartDatetime { get; set; }
        public virtual string PlanUnpackingFinishDatetime { get; set; }
        public virtual string Tpm { get; set; }
        public virtual string Remarks { get; set; }
        public virtual DateTime? UpCalltime { get; set; }
        public virtual DateTime? UnpackingActualStartDatetime { get; set; }
        public virtual DateTime? UnpackingActualFinishDatetime { get; set; }
        public virtual string UpStatus { get; set; }
        public virtual DateTime? MakingFinishDatetime { get; set; }
        public virtual string MkStatus { get; set; }
        public virtual string Status { get; set; }
        public virtual int? MkModuleCount { get; set; }
        public virtual int? MaxLot { get; set; }
        public virtual string ScreenStatus { get; set; }

        //
        public virtual string CaseNo { get; set; }
        public virtual string SupplierNo { get; set; }

        // background

        
    }
}
