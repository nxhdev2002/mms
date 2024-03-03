using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmEngineMasterExportInput
    {

        public virtual string MaterialCode { get; set; }

        public virtual string TransmissionType { get; set; }

        public virtual string EngineModel { get; set; }

        public virtual string EngineType { get; set; }

    }

}