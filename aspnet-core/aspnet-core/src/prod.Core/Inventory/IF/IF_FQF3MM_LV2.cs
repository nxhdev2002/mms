using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_FQF3MM_LV2")]
    public class IF_FQF3MM_LV2 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxFileIdLength = 50;

        public const int MaxFileDescriptionLength = 200;

        public const int MaxContentLength = 5000;

        public const int MaxStatusLength = 20;

        public const int MaxFilePathLength = 200;

        [StringLength(MaxFileIdLength)]
        public virtual string FileId { get; set; }

        [StringLength(MaxFileDescriptionLength)]
        public virtual string FileDescription { get; set; }

        [StringLength(MaxContentLength)]
        public virtual string Content { get; set; }

        public virtual DateTime? InterfaceDate { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual DateTime? StatusDateTime { get; set; }

        [StringLength(MaxFilePathLength)]
        public virtual string FilePath { get; set; }
    }

}
