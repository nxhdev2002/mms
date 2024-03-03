import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { MstCmmValuationClassDto, MstCmmValuationClassServiceProxy, MstGpsMaterialRegisterByShopDto, MstGpsMaterialRegisterByShopServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-gpsmaterialregisterbyshop-modal',
    templateUrl: './create-or-edit-gpsmaterialregisterbyshop-modal.component.html',
    })
export class CreateOrEditGpsMaterialRegisterByShopModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsMaterialRegisterByShop', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstGpsMaterialRegisterByShopDto = new MstGpsMaterialRegisterByShopDto();
    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    category;
    categoryList = [
        {value: 'Direct Materials' , label: "Direct Materials"},
        {value: 'Indirect Materials' , label: "Indirect Materials"},
    ];

    constructor(
        injector: Injector,
        private _service1: MstGpsMaterialRegisterByShopServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: MstGpsMaterialRegisterByShopDto): void {
        // this.getCategoryCbx()
        if (!rowdata) this.rowdata = new MstGpsMaterialRegisterByShopDto();
        else this.rowdata = rowdata;
        this.active = true;
        this.modal.show();
    }

    // changeActive(event){
    //     this._isActive = event.target.checked;
    // }
    save(): void {
        this.saving = true;
        this._service1.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modal?.hide();
                this.modalSave.emit(this.rowdata);
            });
        this.saving = false;
    }

    // getCategoryCbx() {
    //     this.categoryList = []
    //     this._service1.getCbxCategory().subscribe((result) => {
    //         result.forEach(e => this.categoryList.push({ value: e.name, label: e.name }));
    //     });
    // }

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
