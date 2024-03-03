using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IHP
{
    [Table("InvIphMatCustomDeclareDetails")]
    public class InvIphMatCustomDeclareDetails : FullAuditedEntity<long>, IEntity<long>
    {
        public virtual int DHangMDDKID { get; set; }
        public virtual int DToKhaiMDID { get; set; }
        public virtual int SOTK { get; set; }
        public virtual DateTime? NGAY_DK { get; set; }
        public virtual string MA_LH { get; set; }
        public virtual string MA_HQ { get; set; }
        public virtual int STTHANG { get; set; }
        public virtual string MA_HANGKB { get; set; }
        public virtual string MA_PHU { get; set; }
        public virtual string TEN_HANG { get; set; }
        public virtual string PART_SPEC { get; set; }
        public virtual string PART_SIZE { get; set; }
        public virtual string NUOC_XX { get; set; }
        public virtual string TEN_NUOC_XX { get; set; }
        public virtual string MA_DVT { get; set; }
        public virtual decimal LUONG { get; set; }
        public virtual float DGIA_KB { get; set; }
        public virtual float DGIA_TT { get; set; }
        public virtual float TRIGIA_KB { get; set; }
        public virtual float TRIGIA_TT { get; set; }
        public virtual float TGKB_VND { get; set; }
        public virtual decimal TS_XNK { get; set; }
        public virtual decimal TS_TTDB { get; set; }
        public virtual decimal TS_VAT { get; set; }
        public virtual float THUE_XNK { get; set; }
        public virtual float THUE_TTDB { get; set; }
        public virtual float THUE_VAT { get; set; }
        public virtual string LOAI_HANG { get; set; }
        public virtual string TS_XNK_MA_BT { get; set; }
        public virtual float TRIGIA_HDTM { get; set; }
        public virtual float DGIA_HDTM { get; set; }
        public virtual char MA_NT_DGIA_HDTM { get; set; }
        public virtual char DVT_DGIA_HDTM { get; set; }
        public virtual string THUEKHAC_MA_AP_DUNG { get; set; }
        public virtual float TRIGIA_TT_S { get; set; }
        public virtual char DVT_DGIA_TT { get; set; }
        public virtual char MA_PL_TS_TNK { get; set; }
        public virtual string TEN_TS_TNK { get; set; }
        public virtual string THUEKHAC_TEN_KHOAN_MUC { get; set; }
        public virtual float THUEKHAC_TRGIA_TT { get; set; }
        public virtual string THUEKHAC_TEN_TS { get; set; }
        public virtual float THUEKHAC_SO_TIEN { get; set; }
    }
}
