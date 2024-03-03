import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { GetNextEDInOutput, PtsAdoWeldingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-nextedin',
    templateUrl: './nextedin.component.html',
    styleUrls: ['./nextedin.component.less'],
})
export class NextEdInComponent implements OnInit {
    rowStorage: GetNextEDInOutput = new GetNextEDInOutput();
    dataNextEdIn: any[] = [];
    scanLocation: string = '';
    bodyNo: string = '';
    clearTimeLoadData;

    constructor(injector: Injector, private _service: PtsAdoWeldingServiceProxy) {}

    ngOnInit() {
        this.loadForm();
        console.log('ngOnInit');
    }

    ngAfterViewInit() {
        this.timeoutData();
        console.log('ngAfterViewInit');
    }

    ngOnDestroy(): void {
        clearTimeout(this.clearTimeLoadData);
    }

    timecount: number = 0;
    refeshPage: number = 600;
    timeoutData() {
        try {
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;

            this.getData();

        } catch (ex) {
            console.log('1: ' + ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    getData() {
        this._service.getNextEDIn().pipe(finalize(() => {}))
            .subscribe((result) => {
                try {
                    this.dataNextEdIn = result.items ?? [];
                    this.scanLocation = this.dataNextEdIn[0].scanLocation;
                    this.bodyNo = this.dataNextEdIn[0].bodyNo;

                    this.bindDataNextEdIn();

                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
                } catch (ex) {
                    console.log('2: ' + ex);

                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
                }
            });
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }

    loadForm() {
        var w = window.innerWidth;
        var h = window.innerHeight;
        var hItemTitle = (h / 10 - 1) / 2;
        var wScreen = w - (w / 100) * 1;
        var hTitle = (h / 100) * 10;
        var hScanLocation = (h / 100) * 40;
        var hBodyNo = h - hTitle - hScanLocation - 12;
        var wItemTitle = (w / 10) * 1.5;

        var box = document.querySelectorAll<HTMLElement>('.nextedin,' + '.nextedin .body');
        for (let i = 0; box[i]; i++) {
            (box[i] as HTMLElement).style.height = w + 'px';
            (box[i] as HTMLElement).style.height = h + 'px';
        }

        var title = document.querySelectorAll<HTMLElement>('.nextedin .body .title-header');
        for (let i = 0; title[i]; i++) {
            (title[i] as HTMLElement).style.height = hTitle + 'px';
            (title[i] as HTMLElement).style.width = wScreen + 'px';
        }

        var scanLocation = document.querySelectorAll<HTMLElement>('.nextedin .body .scanlocation');
        for (let i = 0; scanLocation[i]; i++) {
            (scanLocation[i] as HTMLElement).style.height = hScanLocation + 'px';
            (scanLocation[i] as HTMLElement).style.width = wScreen + 'px';
        }

        var body = document.querySelectorAll<HTMLElement>('.nextedin .body .bodyno');
        for (let i = 0; body[i]; i++) {
            (body[i] as HTMLElement).style.height = hBodyNo + 'px';
            (body[i] as HTMLElement).style.width = wScreen + 'px';
        }

        var itemTitle = document.querySelectorAll<HTMLElement>(
            '.nextedin .body .scanlocation .scanlocation-title,' + '.nextedin .body .bodyno .bodyno-title'
        );
        for (let i = 0; itemTitle[i]; i++) {
            (itemTitle[i] as HTMLElement).style.height = hItemTitle + 'px';
            (itemTitle[i] as HTMLElement).style.width = wItemTitle + 'px';
        }
    }

    bindDataNextEdIn() {
        //bind data
        this.dataNextEdIn.forEach((element) => {
            var o = element as GetNextEDInOutput;
            var backgroundcolor = o.bodyNo.substring(0, 1);
            var obj = document.getElementById('next_ed_in_bodyno');
            switch (backgroundcolor) {
                case 'K':
                    obj.style.backgroundColor = '#A30026';
                    break;
                case 'F':
                    obj.style.backgroundColor = '#FFFF99';
                    break;
                case 'C':
                    obj.style.backgroundColor = '#8BB2E1';
                    break;
                case 'V':
                    obj.style.backgroundColor = '#F9BD8D';
                    break;
                case 'I':
                    obj.style.backgroundColor = '#90CE50';
                    break;
            }
        });
    }
}
