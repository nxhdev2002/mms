import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstCmmModelDto, MstCmmColorDto, MstCmmLotCodeGradeServiceProxy, MstCmmModelServiceProxy, MstColorDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { Color } from '@ag-grid-enterprise/all-modules';
import { result } from 'lodash-es';
import { VehicleComponent } from './vehicle.component';


@Component({
    selector: 'create-or-edit-vehicle-modal',
    templateUrl: './create-or-edit-vehicle-modal.component.html',
    styleUrls: ['./create-or-edit-vehicle-modal.component.less']
    })
export class CreateOrEditVehicleModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalModel', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstCmmModelDto = new CreateOrEditMstCmmModelDto();
    // eslint-disable-next-line @typescript-eslint/no-inferrable-types
    active: boolean = false;
    // eslint-disable-next-line @typescript-eslint/no-inferrable-types
    saving: boolean = false;
    _isActive: boolean;
    listColors: MstColorDto[] = [];
    listColorsByModel: MstColorDto = new MstColorDto();
    gradeId;
    // eslint-disable-next-line @typescript-eslint/no-inferrable-types
    checkBox: boolean = false;
    listColor: MstColorDto = new MstColorDto();
    grade;
    gradeName;
    constructor(
        injector: Injector,
        private _service: MstCmmLotCodeGradeServiceProxy,
        private _view: VehicleComponent

    ) {
        super(injector);
    }
    show(gradeId?: any): void {
        this.gradeId = gradeId;
        this.active = true;
        this._service.getColorDetailColorList(this.gradeId).subscribe((result) => {
            this.listColors = result;
            this.grade = result[0].grade;
            this.gradeName = result[0].gradeName;

        });


        this.modal.show();
    }

    save(): void {
        let list = [];
        let listIdColor;
        let gradeId;
        if(this.listColors.filter(x => x.checks == true && x.colorType == 'INT').length > 1)
        {
            this.notify.warn(this.l('Không được chọn quá 1 màu nội thất'));
            return;
        }
        this.listColors.forEach(e => {
            // eslint-disable-next-line eqeqeq
            if (e.checks == true){   
                gradeId = e.gradeId;
                list.push(e.id);
            }
        });
        listIdColor = list.join(',');
        this.saving = true;
        this._service.createOrEditGradeColor(listIdColor, gradeId)
            .pipe( finalize(() =>  this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modal?.hide();
                this.modalSave.emit(this.rowdata);
            });
        this.saving = false;
        this._view.searchDataLotCodeGrades(gradeId);
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    // eslint-disable-next-line @typescript-eslint/member-ordering
    @HostListener('document:keydown', ['$event'])
        onKeydownHandler(event: KeyboardEvent) {
            if (event.key === 'Escape') {
                this.close();
            }
        }
}
