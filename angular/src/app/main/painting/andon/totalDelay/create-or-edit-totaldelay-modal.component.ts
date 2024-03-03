import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditPtsAdoTotalDelayDto, PtsAdoTotalDelayServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'create-or-edit-totaldelay-modal',
    templateUrl: './create-or-edit-totaldelay-modal.component.html',
    styleUrls: ['./create-or-edit-totaldelay-modal.component.less']
    })
export class CreateOrEditTotalDelayModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalTotalDelay', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickeredInAct' , {static: false}) datepickeredInAct!: BsDatepickerDirective;
    @ViewChild('datepickerStartRepair' , {static: false}) datepickerStartRepair!: BsDatepickerDirective;
    @ViewChild('datepickeraInPlanDate' , {static: false}) datepickeraInPlanDate!: BsDatepickerDirective;
    @ViewChild('datepickerRepairIn' , {static: false}) datepickerRepairIn!: BsDatepickerDirective;
    @ViewChild('datepickerLeadtime' , {static: false}) datepickerLeadtime!: BsDatepickerDirective;
    @ViewChild('datepickerLeadtimePlus' , {static: false}) datepickerLeadtimePlus!: BsDatepickerDirective;
    @ViewChild('datepickerEtd' , {static: false}) datepickerEtd!: BsDatepickerDirective;
    @ViewChild('datepickerRecoatIn' , {static: false}) datepickerRecoatIn!: BsDatepickerDirective;



    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditPtsAdoTotalDelayDto = new CreateOrEditPtsAdoTotalDelayDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _startRepair: any;
    _aInPlanDate: any;
    _edInAct: any;
    _repairIn: any;
    _leadtime: any;
    _leadtimePlus: any;
    _etd: any;
    _recoatIn: any;


    constructor(
        injector: Injector,
        private _mstwptTotalDelayService: PtsAdoTotalDelayServiceProxy,
        private _dateTimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditPtsAdoTotalDelayDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditPtsAdoTotalDelayDto();
        else this.rowdata = rowdata;
		const dateValuestartRepair =  this.rowdata.startRepair ? new Date(this.rowdata.startRepair?.toString()) : new Date();
		this.datepickerStartRepair?.bsValueChange.emit(dateValuestartRepair);

		const dateValueaInPlanDate =  this.rowdata.aInPlanDate ? new Date(this.rowdata.aInPlanDate?.toString()) : new Date();
		this.datepickeraInPlanDate?.bsValueChange.emit(dateValueaInPlanDate);

		const dateValueedInAct =  this.rowdata.edInAct ? new Date(this.rowdata.edInAct?.toString()) : new Date();
		this.datepickeredInAct?.bsValueChange.emit(dateValueedInAct);



		const dateValuerepairIn =  this.rowdata.repairIn ? new Date(this.rowdata.repairIn?.toString()) : new Date();
		this.datepickerRepairIn?.bsValueChange.emit(dateValuerepairIn);

		const dateValueleadtime =  this.rowdata.leadtime ? new Date(this.rowdata.leadtime?.toString()) : new Date();
		this.datepickerLeadtime?.bsValueChange.emit(dateValueleadtime);

		const dateValueleadtimePlus =  this.rowdata.leadtimePlus ? new Date(this.rowdata.leadtimePlus?.toString()) : new Date();
		this.datepickerLeadtimePlus?.bsValueChange.emit(dateValueleadtimePlus);

		const dateValueetd =  this.rowdata.etd ? new Date(this.rowdata.etd?.toString()) : new Date();
		this.datepickerEtd?.bsValueChange.emit(dateValueetd);

		const dateValuerecoatIn =  this.rowdata.recoatIn ? new Date(this.rowdata.recoatIn?.toString()) : new Date();
		this.datepickerRecoatIn?.bsValueChange.emit(dateValuerecoatIn);

        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.startRepair = this._dateTimeService.convertToDatetime(this._startRepair),
    this.rowdata.aInPlanDate = this._dateTimeService.convertToDatetime(this._aInPlanDate),
    this.rowdata.edInAct = this._dateTimeService.convertToDatetime(this._edInAct),
    this.rowdata.repairIn = this._dateTimeService.convertToDatetime(this._repairIn),
    this.rowdata.leadtime = this._dateTimeService.convertToDatetime(this._leadtime),
    this.rowdata.leadtimePlus = this._dateTimeService.convertToDatetime(this._etd),
    this.rowdata.etd = this._dateTimeService.convertToDatetime(this._etd),
    this.rowdata.recoatIn = this._dateTimeService.convertToDatetime(this._recoatIn),
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this._mstwptTotalDelayService.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modal?.hide();
            //this.modalSave.emit(this.rowdata);
        });
    this.saving = false;
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    @HostListener('document:keydown', ['$event'])
        onKeydownHandler(event: KeyboardEvent) {
            if (event.key === "Escape") {
                this.close();
            }
        }
}
