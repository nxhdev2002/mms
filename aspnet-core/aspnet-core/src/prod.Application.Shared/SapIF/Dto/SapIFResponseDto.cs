using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prod.SapIF.Dto
{
    public class SapIFResponseDto<T>
    {
        public List<SapIFResponseSapDto<T>> SapResponse { get; set; }
        public string LegacyResponse { get; set; }
        public string Exception { get; set; }
        public SapIFResponseDto()
        {
            SapResponse = new List<SapIFResponseSapDto<T>>();
        }
    }
    public class SapIFResponseSapDto<T>
    {
        public string Exception { get; set; }
        public T Response { get; set; }
    }
}
