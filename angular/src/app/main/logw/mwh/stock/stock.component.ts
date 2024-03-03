import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    StockServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './stock.component.html',
    styleUrls: ['./stock.component.less'],
})
export class StockComponent extends AppComponentBase implements OnInit {

    dataTS_Stock: any[] = [];
    dataHTML_Stock: any[] = [];
    dataRLL_Stock: any[] = [];
    dataFRL_Stock: any[] = [];
    dataOVF_Stock: any[] = [];
    dataROB_Stock: any[] = [];
    arrayRob: any[] = [];
    arrayOvf: any[] = [];
    count_DataFRL: number = 0;
    clearTimeLoadData;
    strdate: string = '';
    fn: CommonFunction = new CommonFunction();

    constructor(injector: Injector, private _service: StockServiceProxy) {
        super(injector);
    }

    ngOnInit(): void {
        this.stockGetDataLoadForm();
    }

    ngAfterViewInit() {
         console.log('ngAfterViewInit');
        this.timeoutData();
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
            this.bindTitleHeader();
            this.loadForm();
            this.stockGetData();
            this.fn.showtime('time_now_log');


        } catch (ex) {
            console.log('1: ' + ex);
            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 3000);
        }
    }

    //load data ts

    stockGetData() {
        this._service.stockGetData('1').pipe(finalize(() => {}))
            .subscribe((result) => {
                try{
                    console.log('get_Data_TS');
                    this.dataTS_Stock = result ?? [];
                    this.bindSM(this.dataTS_Stock[0]);
                    this.bindOVF(this.dataTS_Stock[1]);
                    this.bindROB(this.dataTS_Stock[2]);

                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);

                } catch(ex){
                    console.log('2: ' + ex);
                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
                }

            },(error) => {
                console.log(error);
                this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
            });
    }


    //load data form html
    stockGetDataLoadForm() {
        this._service.stockGetDataLoadForm().pipe(finalize(() => {}))
            .subscribe((result) => {
                try{
                    console.log('get_Data_Html');
                    this.dataHTML_Stock = result ?? [];

                   //
                    this.dataRLL_Stock = this.dataHTML_Stock[0].filter((a) => a.whLoc == 'On_Ground');
                    this.dataFRL_Stock = this.dataHTML_Stock[0].filter((b) => b.whLoc == 'FRL');
                    this.count_DataFRL = (this.dataFRL_Stock != null ? this.dataFRL_Stock.length : 0);

                    //
                    this.dataOVF_Stock = this.dataHTML_Stock[1];
                    //
                    var myArray = this.dataHTML_Stock[2].map(t=>t.laneName);
                    this.dataROB_Stock = myArray.filter((v, i, a) => a.indexOf(v) === i);

                    this.clearTimeLoadData = setTimeout(() => {
                        this.stockGetDataLoadForm();
                    }, 30000);

                } catch(ex){
                    console.log('2: ' + ex);
                    this.clearTimeLoadData = setTimeout(() => {
                        this.stockGetDataLoadForm();
                    }, 30000);
                }

            },(error) => {
                console.log(error);
                this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 30000);
            });
    }

    onlyUnique(value, index, self) {
        return self.indexOf(value) === index;
      }


    fornumbersRange(start: number, stop: number, step: number) {
        let numRange: number[] = [];
        for (let i = start; i <= stop; ) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }

    fornumbersRangeDesc(start: number, stop: number, step: number) {
        let numRangeDesc: number[] = [];
        for (let i = start; i >= stop; ) {
            numRangeDesc.push(i);
            i = i + step;
        }
        return numRangeDesc;
    }

    smLocation() {
        return this.dataRLL_Stock;
    }

    frlLocationFirst() {
        var data1 = this.dataFRL_Stock.slice(0, this.count_DataFRL / 2);
        return data1;
    }

    frlLocationLate() {
        var data2 = this.dataFRL_Stock.slice(this.count_DataFRL / 2, this.count_DataFRL);
        return data2;
    }

    ROB_location() {
        return this.dataROB_Stock;
    }

    OVF_location() {
        return this.dataOVF_Stock;
    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }

    loadForm() {
        let w = window.innerWidth;
        var h = window.innerHeight;

        //width
        var w_title = Math.floor((w / 30) * 100) / 100;

        var w_mainleft2 = Math.floor((w / 2.4) * 100) / 100;
        var w_mainleft3 = w - w_mainleft2;

        var w_mainright = w_mainleft3 - w_title;
        var w_ovf = Math.floor((w_mainright / 3) * 100) / 100;

        let cssBigPxp = document.querySelector<HTMLElement>('.BIG_PXP');
        for (let i = 0; cssBigPxp[i]; i++) {
            cssBigPxp[i].style.width = w + 'px';
            cssBigPxp[i].style.height = h + 'px';
        }

        let css_HEADER_MAIN = document.querySelectorAll<HTMLElement>('.HEADER,' + '.MAIN');
        for (let i = 0; css_HEADER_MAIN[i]; i++) {
            css_HEADER_MAIN[i].style.width = w + 'px';
        }

        let css_MAIN_LEFT_R = document.querySelectorAll<HTMLElement>('.MAIN .MAIN_LEFT.R');
        for (let i = 0; css_MAIN_LEFT_R[i]; i++) {
            css_MAIN_LEFT_R[i].style.width = w_mainleft2 + 'px';
        }

        let css_MAIN_LEFT_F = document.querySelectorAll<HTMLElement>('.MAIN .MAIN_LEFT.F');
        for (let i = 0; css_MAIN_LEFT_F[i]; i++) {
            css_MAIN_LEFT_F[i].style.width = w_mainleft3 + 'px';
        }

        let css_MAIN_RIGHT = document.querySelectorAll<HTMLElement>('.MAIN .MAIN_RIGHT');
        for (let i = 0; css_MAIN_RIGHT[i]; i++) {
            css_MAIN_RIGHT[i].style.width = w_mainright + 'px';
            css_MAIN_RIGHT[i].style.marginLeft = w_mainleft2 + w_title - 4 + 'px';
        }

        let css_OVERFLLOW_ROBBING = document.querySelectorAll<HTMLElement>(
            '.MAIN .MAIN_RIGHT .OVERFLLOW,' + '.MAIN .MAIN_RIGHT .ROBBING'
        );
        for (let i = 0; css_OVERFLLOW_ROBBING[i]; i++) {
            css_OVERFLLOW_ROBBING[i].style.width = w_ovf + 'px';
        }

        //RLL
        var rllcol = document.querySelectorAll<HTMLElement>('.ROLLER_MAIN .RLL_CONTENT').length + 0.5;
        var w_rllcol = Math.floor(((w_mainleft2 - w_title) / rllcol) * 100) / 100;

        let css_RLL_ITEM_RLL_TITLE = document.querySelectorAll<HTMLElement>(
            '.ROLLER .RLL_HEADER .RLL_ITEM,' + '.ROLLER .RLL_HEADER .RLL_TITLE'
        );
        for (let i = 0; css_RLL_ITEM_RLL_TITLE[i]; i++) {
            css_RLL_ITEM_RLL_TITLE[i].style.width = w_title - 1 + 'px';
        }

        let css_not_NEXTUP = document.querySelectorAll<HTMLElement>(
            '.ROLLER_MAIN .RLL_HEADER .RLL_TITLE.DATE span:not(.NEXTUP)'
        );
        for (let i = 0; css_not_NEXTUP[i]; i++) {
            css_not_NEXTUP[i].style.width = w_title / 2 + 4 + 'px';
        }

        let css_RLL_ITEM1 = document.querySelectorAll<HTMLElement>(
            '.ROLLER_MAIN .RLL_CONTENT .RLL_ITEM, ' +
                '.ROLLER_MAIN .RLL_CONTENT .RLL_TITLE, ' +
                '.ROLLER_MAIN .RLL_CONTENT .RLL_ITEM .MIN_MODULE, ' +
                '.ROLLER_MAIN .RLL_CONTENT .RLL_ITEM .MAX_MODULE, ' +
                '.ROLLER_MAIN .RLL_CONTENT .RLL_ITEM .CAPACITY_MODULE'
        );
        for (let i = 0; css_RLL_ITEM1[i]; i++) {
            css_RLL_ITEM1[i].style.width = w_rllcol - 1 + 'px';
        }

        let css_onflr = document.querySelectorAll<HTMLElement>('.ROLLER_MAIN .RLL_CONTENT .RLL_TITLE .onflr');
        for (let i = 0; css_onflr[i]; i++) {
            css_onflr[i].style.width = (w_rllcol - 1) / 4 + 'px';
        }

        let css_ROLLER_HEADER = document.querySelectorAll<HTMLElement>('.ROLLER .ROLLER_HEADER');
        for (let i = 0; css_ROLLER_HEADER[i]; i++) {
            css_ROLLER_HEADER[i].style.width = w_rllcol * (rllcol - 0.5) + 1 + 'px';
            css_ROLLER_HEADER[i].style.marginLeft = w_title - 1 + 'px';
        }

        //FRL
        var frlcol = document.querySelectorAll<HTMLElement>('.FRL_MAIN.frlmain1 .FRL_CONTENT').length;
        var w_frlcol = Math.floor(((w_mainleft3 - w_title - 4) / frlcol) * 100) / 100;

        let css_FRL_ITEM = document.querySelectorAll<HTMLElement>(
            '.FRL .FRL_HEADER .FRL_ITEM, ' + '.FRL .FRL_HEADER .FRL_TITLE'
        );
        for (let i = 0; css_FRL_ITEM[i]; i++) {
            css_FRL_ITEM[i].style.width = w_title - 1 + 'px';
        }

        let css_span_notNEXTUP = document.querySelectorAll<HTMLElement>(
            '.FRL .FRL_HEADER .FRL_TITLE.DATE span:not(.NEXTUP)'
        );
        for (let i = 0; css_span_notNEXTUP[i]; i++) {
            css_span_notNEXTUP[i].style.width = w_title / 2 + 4 + 'px';
        }

        let css_FRL_ITEM1 = document.querySelectorAll<HTMLElement>(
            '.FRL_MAIN .FRL_CONTENT .FRL_ITEM, ' +
                '.FRL_MAIN .FRL_CONTENT .FRL_TITLE, ' +
                '.FRL_MAIN .FRL_CONTENT .FRL_ITEM .MIN_MODULE, ' +
                '.FRL_MAIN .FRL_CONTENT .FRL_ITEM .MAX_MODULE, ' +
                '.FRL_MAIN .FRL_CONTENT .FRL_ITEM .CAPACITY_MODULE'
        );
        for (let i = 0; css_FRL_ITEM1[i]; i++) {
            css_FRL_ITEM1[i].style.width = w_frlcol - 1 + 'px';
        }

        let css_FRL_TITLE_onflr = document.querySelectorAll<HTMLElement>('.FRL_MAIN .FRL_CONTENT .FRL_TITLE .onflr');
        for (let i = 0; css_FRL_TITLE_onflr[i]; i++) {
            css_FRL_TITLE_onflr[i].style.width = (w_frlcol - 1) / 4 + 'px';
        }

        let css_FREELOCATION_HEADER1 = document.querySelectorAll<HTMLElement>('.FRL .FREELOCATION_HEADER');
        for (let i = 0; css_FREELOCATION_HEADER1[i]; i++) {
            css_FREELOCATION_HEADER1[i].style.width = w_mainleft3 - w_title - 3 + 'px';
            css_FREELOCATION_HEADER1[i].style.marginLeft = w_title - 1 + 'px';
        }

        //OVF
        var w_margin = 5;
        var outcol = document.querySelectorAll<HTMLElement>('.OVERFLLOW_MAIN .OVF_CONTENT').length;
        var w_outcol = Math.floor(((w_ovf * 2 - w_margin) / outcol) * 100) / 100;
        var w_outtitle = w_ovf * 2 - w_margin - outcol + 1;

        let css_OVERFLLOW_MAIN = document.querySelectorAll<HTMLElement>('.OUTSIDE .OVERFLLOW_MAIN');
        for (let i = 0; css_OVERFLLOW_MAIN[i]; i++) {
            css_OVERFLLOW_MAIN[i].style.marginLeft = w_margin + 6 + 'px';
        }

        let css_OVF_TITLE = document.querySelectorAll<HTMLElement>(
            '.OVERFLLOW_MAIN .OVF_CONTENT .OVF_ITEM, ' + '.OVERFLLOW_MAIN .OVF_CONTENT .OVF_TITLE'
        );
        for (let i = 0; css_OVF_TITLE[i]; i++) {
            css_OVF_TITLE[i].style.width = w_outcol - 2 + 'px';
        }

        let css_OVERFLLOW_HEADER1 = document.querySelectorAll<HTMLElement>('.OUTSIDE .OVERFLLOW_HEADER');
        for (let i = 0; css_OVERFLLOW_HEADER1[i]; i++) {
            css_OVERFLLOW_HEADER1[i].style.width = w_outtitle + 'px';
            css_OVERFLLOW_HEADER1[i].style.marginLeft = w_margin + 6 + 'px';
        }

        //ROB
        var w_margin = 5;
        var robcol = document.querySelectorAll<HTMLElement>('.ROBBING_MAIN .ROB_CONTENT').length;
        var w_robcol = Math.floor(((w_ovf - w_margin) / robcol) * 100) / 100;
        var w_robtitle = w_ovf - w_margin - (robcol + 1);

        let css_ROBBING_MAIN = document.querySelectorAll<HTMLElement>('.ROBBING .ROBBING_MAIN');
        for (let i = 0; css_ROBBING_MAIN[i]; i++) {
            css_ROBBING_MAIN[i].style.marginLeft = w_margin - 1 + 'px';
        }

        let css_ROB_ITEM = document.querySelectorAll<HTMLElement>(
            '.ROBBING_MAIN .ROB_CONTENT .ROB_ITEM, ' + '.ROBBING_MAIN .ROB_CONTENT .ROB_TITLE'
        );
        for (let i = 0; css_ROB_ITEM[i]; i++) {
            css_ROB_ITEM[i].style.width = w_robcol - 2 + 'px';
        }

        let css_ROBBING_HEADER1 = document.querySelectorAll<HTMLElement>('.ROBBING .ROBBING_HEADER');
        for (let i = 0; css_ROBBING_HEADER1[i]; i++) {
            css_ROBBING_HEADER1[i].style.width = w_robtitle + 2 + 'px';
            css_ROBBING_HEADER1[i].style.marginLeft = w_margin - 1 + 'px';
        }

        //height
        var h_header = Math.floor((h / 20) * 100) / 100;
        var h_title = Math.floor((h / 35) * 100) / 100;
        var h_main = h - h_header;
        var h_main_top = Math.floor((h_main / 4) * 100) / 100;
        var h_main_bot = h_main - h_main_top;

        let css_MAIN_LEFT = document.querySelectorAll<HTMLElement>('.MAIN,' + '.MAIN .MAIN_LEFT');
        for (let i = 0; css_MAIN_LEFT[i]; i++) {
            css_MAIN_LEFT[i].style.height = h_main + 'px';
        }

        let css_HEADER = document.querySelectorAll<HTMLElement>('.HEADER');
        for (let i = 0; css_HEADER[i]; i++) {
            css_HEADER[i].style.height = h_header - 2 + 'px';
            css_HEADER[i].style.lineHeight = h_header - 2 + 'px';
        }

        let css_left_img = document.querySelectorAll<HTMLElement>('.HEADER .remark.left img');
        for (let i = 0; css_left_img[i]; i++) {
            css_left_img[i].style.height = h_header - 2 + 'px';
            css_left_img[i].style.marginLeft = w_title - 8 + 'px';
        }

        let css_right_img = document.querySelectorAll<HTMLElement>('.HEADER .remark.right img');
        for (let i = 0; css_right_img[i]; i++) {
            css_right_img[i].style.height = h_header - 2 + 'px';
            css_right_img[i].style.marginRight = 5 + 'px';
        }

        let css_MAIN_RIGHT_TOP = document.querySelectorAll<HTMLElement>('.MAIN .MAIN_RIGHT .MAIN_RIGHT_TOP');
        for (let i = 0; css_MAIN_RIGHT_TOP[i]; i++) {
            css_MAIN_RIGHT_TOP[i].style.height = h_main_top + 'px';
            css_MAIN_RIGHT_TOP[i].style.marginTop = 5 + 'px';
        }

        let css_FRL = document.querySelectorAll<HTMLElement>('.MAIN .MAIN_RIGHT .FRL');
        for (let i = 0; css_FRL[i]; i++) {
            css_FRL[i].style.height = h_main_bot + 'px';
        }

        //RLL
        var h_3title = Math.floor((h / 36) * 100) / 100;
        var rllrow = document.querySelectorAll<HTMLElement>('.ROLLER_MAIN .RLL_HEADER .RLL_ITEM').length + 2;
        var h_rllrow = Math.floor(((h_main - h_3title * 1 - 2) / rllrow) * 100) / 100;

        let css_ROLLER_ROLLER_HEADER = document.querySelectorAll<HTMLElement>('.ROLLER .ROLLER_HEADER');
        for (let i = 0; css_ROLLER_ROLLER_HEADER[i]; i++) {
            css_ROLLER_ROLLER_HEADER[i].style.height = h_3title - 2 + 'px';
            css_ROLLER_ROLLER_HEADER[i].style.lineHeight = h_3title - 2 + 'px';
        }

        let css_RLL_ITEM_ALL = document.querySelectorAll<HTMLElement>(
            '.ROLLER_MAIN .RLL_CONTENT .RLL_ITEM,' +
                '.ROLLER_MAIN .RLL_CONTENT .RLL_TITLE:not(.DATE):not(.TYPE),' +
                '.ROLLER_MAIN .RLL_HEADER .RLL_ITEM,' +
                '.ROLLER_MAIN .RLL_HEADER .RLL_TITLE:not(.DATE):not(.TYPE)'
        );
        for (let i = 0; css_RLL_ITEM_ALL[i]; i++) {
            css_RLL_ITEM_ALL[i].style.height = h_rllrow + 'px';
            css_RLL_ITEM_ALL[i].style.lineHeight = h_rllrow + 'px';
        }

        let css_RLL_TITLE_DATE = document.querySelectorAll<HTMLElement>(
            '.ROLLER_MAIN .RLL_CONTENT .RLL_TITLE.DATE,' +
                '.ROLLER_MAIN .RLL_CONTENT .RLL_TITLE.TYPE,' +
                '.ROLLER_MAIN .RLL_HEADER .RLL_TITLE.DATE,' +
                '.ROLLER_MAIN .RLL_HEADER .RLL_TITLE.DATE span,' +
                '.ROLLER_MAIN .RLL_HEADER .RLL_TITLE.TYPE'
        );
        for (let i = 0; css_RLL_TITLE_DATE[i]; i++) {
            css_RLL_TITLE_DATE[i].style.height = h_3title - 1 + 'px';
            css_RLL_TITLE_DATE[i].style.lineHeight = h_3title - 1 + 'px';
        }

        let css_TITLE_DATE = document.querySelectorAll<HTMLElement>('.ROLLER_MAIN .RLL_HEADER .RLL_TITLE.DATE');
        for (let i = 0; css_TITLE_DATE[i]; i++) {
            css_TITLE_DATE[i].style.height = h_3title + 'px';
            css_TITLE_DATE[i].style.lineHeight = h_3title + 'px';
        }

        let css_TITLE_TYPE = document.querySelectorAll<HTMLElement>(
            '.ROLLER_MAIN .RLL_CONTENT .RLL_TITLE.TYPE, ' + '.ROLLER_MAIN .RLL_HEADER .RLL_TITLE.TYPE'
        );
        for (let i = 0; css_TITLE_TYPE[i]; i++) {
            css_TITLE_TYPE[i].style.lineHeight = h_3title - 1 + 'px';
        }

        //FRL

        var frlrow = document.querySelectorAll<HTMLElement>('.FRL_MAIN.frlmain1 .FRL_HEADER .FRL_ITEM').length * 2 + 8;
        var h_frlrow = Math.floor(((h_main - h_3title * 1 - 2 - h_main_top - 10) / frlrow) * 100) / 100;

        let css_FREELOCATION_HEADER = document.querySelectorAll<HTMLElement>('.FRL .FREELOCATION_HEADER');
        for (let i = 0; css_FREELOCATION_HEADER[i]; i++) {
            css_FREELOCATION_HEADER[i].style.height = h_3title + 3 + 'px';
            css_FREELOCATION_HEADER[i].style.lineHeight = h_3title + 3 + 'px';
        }

        let css_FREELOCATION_HEADER_NONE = document.querySelectorAll<HTMLElement>('.FRL .FREELOCATION_HEADER.NONE');
        for (let i = 0; css_FREELOCATION_HEADER_NONE[i]; i++) {
            css_FREELOCATION_HEADER_NONE[i].style.height = h_3title + 29 + 'px';
            css_FREELOCATION_HEADER_NONE[i].style.lineHeight = h_3title + 29 + 'px';
        }

        let css_FRL_ITEM_ALL = document.querySelectorAll<HTMLElement>(
            '.FRL_MAIN .FRL_CONTENT .FRL_ITEM' +
                ', .FRL_MAIN .FRL_CONTENT .FRL_TITLE:not(.DATE):not(.TYPE)' +
                ', .FRL_MAIN .FRL_HEADER .FRL_ITEM' +
                ', .FRL_MAIN .FRL_HEADER .FRL_TITLE:not(.DATE):not(.TYPE)'
        );
        for (let i = 0; css_FRL_ITEM_ALL[i]; i++) {
            css_FRL_ITEM_ALL[i].style.height = h_frlrow + 'px';
            css_FRL_ITEM_ALL[i].style.lineHeight = h_frlrow + 'px';
        }

        let css_FRL_TITLE_DATE = document.querySelectorAll<HTMLElement>(
            '.FRL_MAIN .FRL_CONTENT .FRL_TITLE.DATE, ' +
                '.FRL_MAIN .FRL_CONTENT .FRL_TITLE.TYPE, ' +
                '.FRL_MAIN .FRL_HEADER .FRL_TITLE.DATE, ' +
                '.FRL_MAIN .FRL_HEADER .FRL_TITLE.DATE span, ' +
                '.FRL_MAIN .FRL_HEADER .FRL_TITLE.TYPE'
        );
        for (let i = 0; css_FRL_TITLE_DATE[i]; i++) {
            css_FRL_TITLE_DATE[i].style.height = h_3title - 1 + 'px';
            css_FRL_TITLE_DATE[i].style.lineHeight = h_3title - 1 + 'px';
        }

        let css_FRL_TITLE_DATE1 = document.querySelectorAll<HTMLElement>('.FRL_MAIN .FRL_HEADER .FRL_TITLE.DATE');
        for (let i = 0; css_FRL_TITLE_DATE1[i]; i++) {
            css_FRL_TITLE_DATE1[i].style.height = h_3title + 'px';
            css_FRL_TITLE_DATE1[i].style.lineHeight = h_3title + 'px';
        }

        let cssFRL_TITLE_TITLE_TYPE = document.querySelectorAll<HTMLElement>(
            '.FRL_MAIN .FRL_CONTENT .FRL_TITLE.TYPE, ' + '.FRL_MAIN .FRL_HEADER .FRL_TITLE.TYPE'
        );
        for (let i = 0; cssFRL_TITLE_TITLE_TYPE[i]; i++) {
            cssFRL_TITLE_TITLE_TYPE[i].style.lineHeight = h_3title - 1 + 'px';
        }

        let css_FRL_BREAK_HEIGHT = document.querySelectorAll<HTMLElement>('.FRL_BREAK_HEIGHT');
        for (let i = 0; css_FRL_BREAK_HEIGHT[i]; i++) {
            css_FRL_BREAK_HEIGHT[i].style.height = h_main_top + 20 + 'px';
            css_FRL_BREAK_HEIGHT[i].style.width = 100 + '%';
        }

        //OVF
        var outrow =
            document.querySelectorAll<HTMLElement>('.OVERFLLOW_MAIN .OVF_CONTENT:first-child .OVF_ITEM').length + 1;
        var h_outrow = Math.floor(((h_main_top - h_title) / outrow) * 100) / 100;

        let css_OVERFLLOW_HEADER = document.querySelectorAll<HTMLElement>('.OUTSIDE .OVERFLLOW_HEADER');
        for (let i = 0; css_OVERFLLOW_HEADER[i]; i++) {
            css_OVERFLLOW_HEADER[i].style.height = h_title - 2 + 'px';
            css_OVERFLLOW_HEADER[i].style.lineHeight = h_title - 2 + 'px';
        }

        let css_OVF_ITEM = document.querySelectorAll<HTMLElement>(
            '.OVERFLLOW_MAIN .OVF_CONTENT .OVF_ITEM,' +
                '.OVERFLLOW_MAIN .OVF_CONTENT .OVF_TITLE,' +
                '.OVERFLLOW_MAIN .OVF_HEADER .OVF_ITEM,' +
                '.OVERFLLOW_MAIN .OVF_HEADER .OVF_TITLE'
        );
        for (let i = 0; css_OVF_ITEM[i]; i++) {
            css_OVF_ITEM[i].style.height = h_outrow - 1 + 'px';
            css_OVF_ITEM[i].style.lineHeight = h_outrow - 1 + 'px';
        }

        //ROB
        var robrow =
            document.querySelectorAll<HTMLElement>('.ROBBING_MAIN .ROB_CONTENT:first-child .ROB_ITEM').length + 1;
        var h_robrow = Math.floor(((h_main_top - h_title) / robrow) * 100) / 100;

        let css_ROBBING_HEADER = document.querySelectorAll<HTMLElement>('.ROBBING .ROBBING_HEADER');
        for (let i = 0; css_ROBBING_HEADER[i]; i++) {
            css_ROBBING_HEADER[i].style.height = h_title - 2 + 'px';
            css_ROBBING_HEADER[i].style.lineHeight = h_title - 2 + 'px';
        }

        let css_ROB_ITEM_h = document.querySelectorAll<HTMLElement>(
            '.ROBBING_MAIN .ROB_CONTENT .ROB_ITEM,' +
                '.ROBBING_MAIN .ROB_CONTENT .ROB_TITLE,' +
                '.ROBBING_MAIN .ROB_HEADER .ROB_ITEM,' +
                '.ROBBING_MAIN .ROB_HEADER .ROB_TITLE'
        );
        for (let i = 0; css_ROB_ITEM_h[i]; i++) {
            css_ROB_ITEM_h[i].style.height = h_robrow - 1 + 'px';
            css_ROB_ITEM_h[i].style.lineHeight = h_robrow - 1 + 'px';
        }

    }


    bindSM(jdata) {

        //clear case RLL
        let rm_active = document.querySelectorAll<HTMLElement>('.ROLLER .ROLLER_MAIN .RLL_CONTENT .RLL_ITEM');
        for (let i = 0; rm_active[i]; i++) {
            rm_active[i].classList.remove('active');
        }
        let html_rll = document.querySelectorAll<HTMLElement>('.ROLLER .ROLLER_MAIN .RLL_CONTENT .RLL_ITEM span')
        for (let i = 0; html_rll[i]; i++) {
            html_rll[i].innerHTML = '';
        }

        //clear case FRL
        let rm_frl_active = document.querySelectorAll<HTMLElement>('.FRL .FRL_MAIN .FRL_CONTENT .FRL_ITEM');
        for (let i = 0; rm_frl_active[i]; i++) {
            rm_frl_active[i].classList.remove('active');
        }
        let html_frl = document.querySelectorAll<HTMLElement>('.FRL .FRL_MAIN .FRL_CONTENT .FRL_ITEM span');
        for (let i = 0; html_frl[i]; i++) {
            html_frl[i].innerHTML = '';
        }


        if(jdata.length > 0){

            //bind RLL
            for (var i = 0; i < jdata.length; i++) {
                var casein = jdata[i].caseNo != null ? Number(jdata[i].caseNo.replace(jdata[i].casePrefix, '')) : 0;
                var objCase = document.querySelectorAll<HTMLElement>('.ROLLER .ROLLER_MAIN .RLL_CONTENT.RLL_' + jdata[i].casePrefix + ' .RLL_ITEM:not(.active)');

                var index = objCase.length;
                if (index > 0) {
                    objCase[index - 1].classList.add('active');
                    var objCaseSpan = document.querySelector('.ROLLER .ROLLER_MAIN .RLL_CONTENT.RLL_' + jdata[i].casePrefix + ' .RLL_ITEM.active span');
                    if(objCaseSpan != null) {objCaseSpan.innerHTML = casein.toString();}

                } else {
                    continue;
                }
            }

            //bind FRL
            for (var i = 0; i < jdata.length; i++) {
                var casein = jdata[i].caseNo != null ? Number(jdata[i].caseNo.replace(jdata[i].casePrefix, '')): 0;
                var objCase = document.querySelectorAll<HTMLElement>('.FRL .FRL_MAIN .FRL_CONTENT.FRL_' + jdata[i].casePrefix + ' .FRL_ITEM:not(.active)');

                var index = objCase.length;
                if (index > 0) {
                    objCase[index - 1].classList.add('active');
                    var objCaseSpan = document.querySelector('.FRL .FRL_MAIN .FRL_CONTENT.FRL_' + jdata[i].casePrefix + ' .FRL_ITEM.active span');
                    if(objCaseSpan != null){objCaseSpan.innerHTML = casein.toString();}

                } else {
                    continue;
                }
            }

            //this.checkmin_SM();
        }

    }

    bindOVF(jdata) {
        //clear case
        let ovf_active = document.querySelectorAll<HTMLElement>('.OUTSIDE .OVERFLLOW_MAIN .OVF_CONTENT .OVF_ITEM');
        for (let i = 0; ovf_active[i]; i++) {
            ovf_active[i].classList.remove('active');
        }
        var OVF_ITEM_span = document.querySelector('.OUTSIDE .OVERFLLOW_MAIN .OVF_CONTENT .OVF_ITEM span');
        if(OVF_ITEM_span != null){
            OVF_ITEM_span.innerHTML = '';
        }

        //COUNT CASENO
        var current = null;
        var cnt = 0;
        for(var i = 0;i < jdata.length; i++){
            if(jdata[i].caseNo != current){
                if(cnt > 0){
                    this.arrayOvf.push(current +  ' : ' + cnt);
                }
                current = jdata[i].caseNo;
                cnt = 1;
            } else {
                cnt++;
            }
        }
        if (cnt > 0) {
            this.arrayOvf.push(current +  ' : ' + cnt);
        }

        //bind data
        if(jdata){
        for (var i = 0; i < jdata.length; i++) {
            var objCase = document.querySelector<HTMLElement>('.OUTSIDE .OVERFLLOW_MAIN .OVF_CONTENT.OVF_' + jdata[i].columnAlias + ' .OVF_ITEM:not(.active)');
            if (objCase) {
                objCase.classList.add('active');
                var objCaseSpan = document.querySelector('.OUTSIDE .OVERFLLOW_MAIN .OVF_CONTENT.OVF_' + jdata[i].columnAlias + ' OVF_ITEM.active span');
                if(objCase != null){ objCase.innerHTML = this.arrayOvf[i];}

            } else {
                continue;
            }
        }
    }

    }

    bindROB(jdata) {
         //clear case
         let rm_rob_active = document.querySelectorAll<HTMLElement>('.ROBBING .ROBBING_MAIN .ROB_CONTENT .ROB_ITEM');
         for (let i = 0; rm_rob_active[i]; i++) {
             rm_rob_active[i].classList.remove('active');
         }

         var html_rob = document.querySelectorAll<HTMLElement>('.ROBBING .ROBBING_MAIN .ROB_CONTENT .ROB_ITEM span');
         for (let i = 0; html_rob[i]; i++) {
             html_rob[i].innerHTML = '';
         }

        if(jdata.length > 0){
            //COUNT CASENO
            var current = null;
            var cnt = 0;
            for(var i = 0;i < jdata.length; i++){
                if(jdata[i].caseNo != current){
                    if(cnt > 0){
                        this.arrayRob.push(current +  ' : ' + cnt);
                    }
                    current = jdata[i].caseNo;
                    cnt = 1;
                } else {
                    cnt++;
                }
            }
            if (cnt > 0) {
                this.arrayRob.push(current +  ' : ' + cnt);
            }

            //BIND DATA
            for (var i = 0; i < jdata.length; i++) {
                if (jdata[i].caseNo == null || jdata[i].caseNo == "") { continue; }
                var objCase = document.querySelector<HTMLElement>(".ROBBING .ROBBING_MAIN .ROB_CONTENT.ROB_" + jdata[i].laneName + " .ROB_ITEM:not(.active)");
                if(objCase){
                    objCase.classList.add('active')
                    var objCaseSpan = document.querySelector<HTMLElement>(".ROBBING .ROBBING_MAIN .ROB_CONTENT.ROB_" + jdata[i].laneName + " .ROB_ITEM.active span");
                    objCase.innerHTML = this.arrayRob[i];
                }
            }
        }
    }

    bindTitleHeader() {
        let cssTimetitle = document.querySelector('.BIG_PXP .HEADER .time');
        //TITLE HEADER
        if (this.strdate != strdatetmp) {
            var d = new Date();
            var strdatetmp =
                d.getDate() +
                '-' +
                this.getMonthEN(d.getMonth()).substring(0, 3) +
                '(' +
                this.getTime(d).replace(' : ', ':') +
                ')';
            if(cssTimetitle != null){cssTimetitle.innerHTML = strdatetmp;}
            this.strdate = strdatetmp;
        }
    }

    getTime(dt) {
        var strtime =
            ((dt.getHours() + '').length == 1 ? '0' + dt.getHours() : dt.getHours()) +
            ' : ' +
            ((dt.getMinutes() + '').length == 1 ? '0' + dt.getMinutes() : dt.getMinutes());
        return strtime;
    }

    getMonthEN(m) {
        switch (m) {
            case 0:
                return 'January';
            case 1:
                return 'February';
            case 2:
                return 'March';
            case 3:
                return 'April';
            case 4:
                return 'May';
            case 5:
                return 'June';
            case 6:
                return 'July';
            case 7:
                return 'August';
            case 8:
                return 'September';
            case 9:
                return 'October';
            case 10:
                return 'November';
            case 11:
                return 'December';
            default:
                return m;
        }
    }
}
