import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'confirm-dialog',
    templateUrl: './confirm-dialog.component.html',
    styleUrls: ['./confirm-dialog.component.less']
})
export class ConfirmDialogComponent extends AppComponentBase implements OnInit {
    @ViewChild('dialogModel', { static: false }) modal!: ModalDirective;
    @Input() isConfirm: boolean = true;
    @Output() confirmYes: EventEmitter<any> = new EventEmitter<any>();
    @Output() confirmNo: EventEmitter<any> = new EventEmitter<any>();
    // @Output() signAnyway: EventEmitter<any> = new EventEmitter<any>();

    // documentData : any;
 
    Title:string = "";
    Content:string = "";
 

    // width:number = 400
    dialogWidth: Number = 400;
    // dialogHeight: string = '71%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };

    constructor(injector: Injector) {
        super(injector);
    }


    ngOnInit() {

    }
 
  
    show(_title?: any, _content?: any){
  
        if(_title) this.Title = _title;
        if(_content) this.Content = _content;
         
        this.modal.show();
    }

    submit(){
        this.confirmYes.emit(null)
        this.modal.hide();
    }
 
    Cancel(){
        this.confirmNo.emit(null)
        this.modal.hide();
    }
}
