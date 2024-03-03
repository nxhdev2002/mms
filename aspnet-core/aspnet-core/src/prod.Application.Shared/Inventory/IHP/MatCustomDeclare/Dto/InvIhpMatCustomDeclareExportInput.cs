using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.IHP.Dto
{
    public class InvIhpMatCustomDeclareExportInput
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

    public class InvIhpMatCustomDeclareDetailsExportInput
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
