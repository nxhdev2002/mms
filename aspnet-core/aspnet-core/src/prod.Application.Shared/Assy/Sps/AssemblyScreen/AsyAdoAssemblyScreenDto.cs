using Abp.Application.Services.Dto;
using System; 
namespace prod.Assy.Sps
{
 

	public class AsyAdoAssemblyScreenDataOutputDto 
	{

        public virtual string Line { get; set; }
        public virtual string Process { get; set; }
        public virtual string Model { get; set; }
        public virtual string Body { get; set; }
        public virtual string SeqNo { get; set; }
        public virtual string Grade { get; set; }
        public virtual string LotNo { get; set; }
        public virtual int? NoInLot { get; set; }
        public virtual string Color { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual int? NoInDate { get; set; }
        public virtual int? ProcessSeq { get; set; }
        public virtual string Dolly { get; set; }
        public virtual string ModelName { get; set; }
        public virtual string Message { get; set; }
        public virtual string LotInOrder { get; set; }
        public virtual string IsProject { get; set; }
        public virtual string IsTrd { get; set; }
		public virtual string RowNo { get; set; }

		



	}

	public class AsyAdoAssemblyScreenConfigOutputDto  
	{

		public virtual string ScreenCode { get; set; }
		public virtual string ScreenName { get; set; }
		public virtual string ProdLine { get; set; }
		public virtual string ProcessGroup { get; set; }
		public virtual string ProcessList { get; set; } 
		public virtual string HeaderList { get; set; }
		public virtual string IsShowHeader { get; set; }
		public virtual string HeaderColor1 { get; set; }
		public virtual string HeaderColor2 { get; set; }
		public virtual string LeftTitle { get; set; }
		public virtual string LeftTitleVn { get; set; }
		public virtual string isShowLeftTitle { get; set; }
		public virtual string isShowModel { get; set; }
		public virtual string isShowGrade { get; set; }
		public virtual string isShowSequence { get; set; }
		public virtual string isShowBody { get; set; }
		public virtual string isShowLotNo { get; set; }
		public virtual string isShowColor { get; set; }
		public virtual string isActive { get; set; } 


	}

}


