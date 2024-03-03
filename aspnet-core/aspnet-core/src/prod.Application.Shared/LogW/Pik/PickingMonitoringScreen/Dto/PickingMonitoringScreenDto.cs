using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Pik.Dto
{
	public class PickingMonitoringScreenDto 
	{
		public virtual string PickingPosition { get; set; }

		public virtual string PickingTabletId { get; set; }

		public virtual int? LogicSequenceNo { get; set; }

		public virtual string Process { get; set; }

		public virtual string LogicSequence { get; set; }

		public virtual int? PickingCycle { get; set; }

		public virtual string IsPushed { get; set; }

		public virtual int? TaktTime { get; set; }

		public virtual DateTime? TaktStartTime { get; set; }

		public virtual DateTime? CurrentTime { get; set; }

		public virtual DateTime? StartTime { get; set; }

		public virtual DateTime? FinishTime { get; set; }

		public virtual int? SeqNo { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual string Shift { get; set; }

		public virtual string Model { get; set; }

		public virtual string LotCode { get; set; }

		public virtual string CanCallUnpacking { get; set; }

		//
		public virtual string ScreenTitle { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual int? CurrentTaktTime { get; set; }

		public virtual int? TaktCountDown { get; set; }

		public virtual int? TotalStop { get; set; }

		public virtual int? TotalDelay { get; set; }

		public virtual string IsPickingDelay { get; set; }

		public virtual string IsAGVAndWDelay { get; set; }

		public virtual string IsLeaderSupport { get; set; }

		public virtual string IsFinished { get; set; }

		public virtual double DelayTime { get; set; }

		public virtual string IsCurrent { get; set; }

		public virtual string IsDelay { get; set; }

		public virtual string IsCallLeader { get; set; }

		public virtual string ProcessCode { get; set; }

		public virtual string ProcessGroup { get; set; }

		public virtual string IsActive { get; set; }

		public virtual int? PkSmTotal { get; set; }

		public virtual int? PkUbTotal { get; set; }

		public virtual int? AgvTotal { get; set; }

		public virtual int? WTotal { get; set; }





	}	
}


