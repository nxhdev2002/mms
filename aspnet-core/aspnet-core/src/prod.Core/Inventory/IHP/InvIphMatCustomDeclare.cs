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
    [Table("InvIphMatCustomDeclare")]
    public class InvIphMatCustomDeclare : FullAuditedEntity<long>, IEntity<long>
    
    {
        public virtual int DToKhaiMDID { get; set; }
        public virtual int SOTK { get; set; }
        public virtual string SOTK_DAU_TIEN { get; set; }
        public virtual int SOTK_NHANH { get; set; }
        public virtual int SOTK_TONG { get; set; }
        public virtual string MA_LH { get; set; }
        public virtual string MA_HQ { get; set; }
        public virtual string TEN_HQ { get; set; }
        public virtual int NAMDK { get; set; }
        public virtual DateTime? NGAY_DK { get; set; }
        public virtual string MA_DV { get; set; }
        public virtual string MA_BC_DV { get; set; }
        public virtual string DIA_CHI_DV { get; set; }
        public virtual string SO_DT_DV { get; set; }
        public virtual string MA_DVUT { get; set; }
        public virtual string DV_DT { get; set; }
        public virtual string MA_BC_DT { get; set; }
        public virtual string MA_PTVT { get; set; }
        public virtual string TEN_PTVT { get; set; }
        public virtual DateTime? NGAYDEN { get; set; }
        public virtual string VAN_DON { get; set; }
        public virtual string MA_CK { get; set; }
        public virtual string TEN_CK { get; set; }
        public virtual string MA_CANGNN { get; set; }
        public virtual string CANGNN { get; set; }
        public virtual string SO_HD { get; set; }
        public virtual float TYGIA_USD { get; set; }
        public virtual DateTime? NGAY_HD { get; set; }
        public virtual string NUOC_XK { get; set; }
        public virtual string NUOC_NK { get; set; }
        public virtual string MA_GH { get; set; }
        public virtual int SOHANG { get; set; }
        public virtual string MA_PTTT { get; set; }
        public virtual string MA_NT { get; set; }
        public virtual float TYGIA_VND { get; set; }
        public virtual string MA_NT_TY_GIA_VND { get; set; }
        public virtual float TONGTGKB { get; set; }
        public virtual int TR_LUONG { get; set; }
        public virtual string DVT_TR_LUONG { get; set; }
        public virtual float PHI_BH { get; set; }
        public virtual string MA_NT_PHI_BH { get; set; }
        public virtual float PHI_VC { get; set; }
        public virtual string MA_NT_PHI_VC { get; set; }
        public virtual int SO_KIEN { get; set; }
        public virtual string DVT_KIEN { get; set; }
        public virtual int SO_CONTAINER { get; set; }
        public virtual string MA_HDTM { get; set; }
        public virtual int SO_TN_HDTM { get; set; }
        public virtual string SO_HDTM { get; set; }
        public virtual DateTime? NGAY_HDTM { get; set; }
        public virtual string MA_PL_GIA_HDTM { get; set; }
        public virtual float TONGTG_HDTM { get; set; }
        public virtual DateTime? NGAY_VANDON { get; set; }
        public virtual string Ten_DV_L1 { get; set; }
        public virtual string Ten_DV_L2 { get; set; }
        public virtual float THUE_VAT { get; set; }
        public virtual float THUE_XNK { get; set; }
        public virtual int TongTienThue { get; set; }
        public virtual string MA_NGHIEP_VU { get; set; }
    }
}
