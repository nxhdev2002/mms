using Abp.Application.Services.Dto;
using prod.Inventory.CPS.InvCpsPrvShipment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsRcvShipmentHeadersDto : EntityDto<long?>
    {

        public virtual string ReceiptNum { get; set; }

        public virtual DateTime? CreationTime { get; set; }

        public virtual string Productgroupname { get; set; }

        public virtual string SupplierName { get; set; }

    }

    public class InvCpsRcvShipmentLineDto :EntityDto<long?>
    {
        public virtual string PoNumber { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string ItemDescription { get; set; }
        public virtual Decimal QuantityShipped { get; set; }

        public virtual Decimal QuantityReceived { get; set; }
        public virtual string UnitOfMeasure { get; set; }

    }
        


    public class GetInvCpsRcvShipmentHeadersInput : PagedAndSortedResultRequestDto
    {

        public virtual string p_receipt_num { get; set; }

        public virtual long? p_invetory_group_id { get; set; }

        public virtual long? p_vendor_id { get; set; }

        public virtual DateTime? p_from_date { get; set; }

        public virtual DateTime? p_to_date { get; set; }

        public virtual string p_part_no { get; set; }

        public virtual string p_part_name { get; set; }

        public virtual string p_po_number { get; set; }

    }


    public class GetInvCpsShipmentHeadersInput 
    {

        public virtual string p_receipt_num { get; set; }

        public virtual long? p_invetory_group_id { get; set; }

        public virtual long? p_vendor_id { get; set; }

        public virtual DateTime? p_from_date { get; set; }

        public virtual DateTime? p_to_date { get; set; }
        public virtual string p_part_no { get; set; }
        public virtual string p_part_name { get; set; }

    }

    public class GetInvCpsRcvShipmentLineInput : PagedAndSortedResultRequestDto
    {

        public virtual long? p_id { get; set; }

    }

    public class ListInvCpsRcvShipmentHeadersDto
    {
        public List<GetInvCpsInventoryGroup> ListInventoryGroup { get; set; }
        public List<GetInvCpsSuppliers> ListSuppliers { get; set; }

    }


    public class GetInvCpsInventoryGroup
    {
        public long? Id { get; set; }
        public string Productgroupcode { get; set; }

        public string Productgroupname { get; set; }
    }

    public class GetInvCpsSuppliers
    {
        public long? Id { get; set; }
        public string SupplierName { get; set; }

        public string SupplierNumber { get; set; }
    }

}


