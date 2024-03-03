import { log } from 'console';
import { Component, ElementRef, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LotMakingTabletServiceProxy, PtsAdoBumperGetDataBumperInDto, PtsAdoPaintingDataServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';


@Component({
    selector: 'app-confirmpart-modal',
    templateUrl: './confirmpart-modal.component.html',
    styleUrls: ['./confirmpart-modal.component.less']
})
export class ConfirmPartAssyModalComponent extends AppComponentBase {
    @ViewChild("modal", { static: true }) modal: ModalDirective;
    dataBumperIn: PtsAdoBumperGetDataBumperInDto[] = [];

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() close = new EventEmitter();
    @Output() end = new EventEmitter();

    displayInfo;
    body;
    progressId;
    type;
    text;

    constructor(
        injector: Injector,
       private _service: PtsAdoPaintingDataServiceProxy
    ) {
        super(injector);
    }

    ngOnInit() {
    }
    ngAfterViewInit() {
    }

    show(t_progressId,t_displayInfo,t_body,v_type) {
        this.displayInfo = t_displayInfo;
        this.body = t_body;
        this.body = t_body;
        this.progressId = t_progressId;
        this.type = v_type;
        this.text = v_type == 'confirmRc' ? 'Xác nhận đã cấp part' : 'Có đồng ý bắn lại part'
        this.modal.show();

    }
    closeModal(): void {
        this.modal?.hide();
    }

    confirm_Ok(progressId) {
        //confirm
        if(this.type == 'confirmRc'){
            this._service .confirmPartBumperIn(progressId).pipe(finalize(() => {}))
            .subscribe((result) => {
                if(result){
                    this.closeModal();

                }
            });
        }
        //update status
        if(this.type == 'updateStt'){
            this._service.updateStatusBumperIn(progressId.toString()).pipe(finalize(() => {}))
                .subscribe((result) => {
                    if(result){
                        this.closeModal();

                    }
                });
        }
    }




}
