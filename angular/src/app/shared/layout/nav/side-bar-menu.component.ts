import { PermissionCheckerService } from 'abp-ng2-module';
import { Injector, Component, OnInit, ViewEncapsulation, AfterViewInit, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef, ElementRef, Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AppMenu } from './app-menu';
import { AppNavigationService } from './app-navigation.service';
import { NavigationEnd, NavigationCancel, Router } from '@angular/router';
import { filter, finalize } from 'rxjs/operators';
import { FormattedStringValueExtracter } from '@shared/helpers/FormattedStringValueExtracter';
import * as objectPath from 'object-path';
import { AppMenuItem } from './app-menu-item';
import { MenuComponent, DrawerComponent, ToggleComponent, ScrollComponent } from '@metronic/app/kt/components';


//tmss
import { BusyIfDirective } from './../../../../shared/utils/busy-if.directive';
//import { DistrictPriorityComponent } from './../../../master/cram/district-priority/district-priority.component';
import { EventBusService } from '@app/shared/services/event-bus.service';
import { TABS } from '@app/shared/constants/tab-keys';
import { MenuOptions } from '@metronic/app/core/_base/layout/directives/menu.directive';






@Component({
    templateUrl: './side-bar-menu.component.html',
    selector: 'side-bar-menu',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideBarMenuComponent extends AppComponentBase implements OnInit, AfterViewInit {
    @Input() iconMenu = false;
    @Input() menuClass = 'menu menu-column menu-rounded menu-sub-indention px-3';

    //#region -- References
    @ViewChild('selectPart', { static: false }) selectPart: any;
    @ViewChild('sidebar') sidebar: BusyIfDirective;
    //#endregion

    isLoading: boolean = false;
    // interval;

    currentTenantId: number | undefined;

    menu: AppMenu = null;
    startYear: Date = new Date(new Date().getFullYear(), 0, 1);
    currentRouteUrl = '';
    insideTm: any;
    outsideTm: any;
    item;

    parameters: { reportType?: number, categoryType?: number, viewChildName?: string, type?: string } = { reportType: 0, categoryType: 0, viewChildName: '', type: '' };

    showFilterModalTabs: Array<any> = [];
    tabCodeAndModalIdDict: {};

    menuOptions: MenuOptions = {
        // vertical scroll
        scroll: null,

        // submenu setup
        submenu: {
            desktop: {
                default: 'dropdown',
                state: {
                    body: 'kt-aside--minimize',
                    mode: 'dropdown'
                }
            },
            tablet: 'accordion', // menu set to accordion in tablet mode
            mobile: 'accordion' // menu set to accordion in mobile mode
        },

        // accordion setup
        accordion: {
            expandAll: false // allow having multiple expanded accordions in the menu
        }
    };

    reportTypes: { label: string, value: number | string }[] = [
        { label: this.l('XLSX'), value: 'XLSX' },
        { label: this.l('DOCX'), value: 'DOCX' },
        { label: this.l('PDF'), value: 'PDF' },
        { label: this.l('RTF'), value: 'RTF' },
    ];

    constructor(
        injector: Injector,
        private router: Router,
        public permission: PermissionCheckerService,
        private _appNavigationService: AppNavigationService,
        // private crmReportApi: CrmReportApiService,
        private cd: ChangeDetectorRef,
        private eventBus: EventBusService) {
        super(injector);
    }

    ngOnInit() {
        this.currentTenantId = abp.session.tenantId!;
        this.menu = this._appNavigationService.getMenu();

        this.currentRouteUrl = this.router.url.split(/[?#]/)[0];

        this.router.events
            .pipe(filter(event => event instanceof NavigationEnd || event instanceof NavigationCancel))
            .subscribe(() => this.currentRouteUrl = this.router.url.split(/[?#]/)[0]);

        this.showFilterModalTabs = []
        this.tabCodeAndModalIdDict = {}
    }

    ngAfterViewInit(): void {
        this.scrollToCurrentMenuElement();
    }

    reinitializeMenu(): void {
        this.menu = this._appNavigationService.getMenu();
        this.currentRouteUrl = this.router.url.split(/[?#]/)[0];

        setTimeout(() => {
            MenuComponent.reinitialization();
            DrawerComponent.reinitialization();
            ToggleComponent.reinitialization();
            ScrollComponent.reinitialization();
        }, 50);
    }

    showMenuItem(menuItem): boolean {
        return this._appNavigationService.showMenuItem(menuItem);
    }

    isMenuItemIsActive(item): boolean {
        if (item.items.length) {
            return this.isMenuRootItemIsActive(item);
        }

        if (!item.route) {
            return false;
        }

        let urlTree = this.router.parseUrl(this.currentRouteUrl.replace(/\/$/, ''));
        let urlString = '/' + urlTree.root.children.primary.segments.map(segment => segment.path).join('/');
        let exactMatch = urlString === item.route.replace(/\/$/, '');
        if (!exactMatch && item.routeTemplates) {
            for (let i = 0; i < item.routeTemplates.length; i++) {
                let result = new FormattedStringValueExtracter().Extract(urlString, item.routeTemplates[i]);
                if (result.IsMatch) {
                    return true;
                }
            }
        }
        return exactMatch;
    }

    isMenuRootItemIsActive(item): boolean {
        let result = false;

        for (const subItem of item.items) {
            result = this.isMenuItemIsActive(subItem);
            if (result) {
                return true;
            }
        }

        return false;
    }

    scrollToCurrentMenuElement(): void {
        const path = location.pathname;
        const menuItem = document.querySelector('a[href=\'' + path + '\']');
        if (menuItem) {
            menuItem.scrollIntoView({ block: 'center' });
        }
    }

    getItemCssClasses(item: AppMenuItem, parentItem: AppMenuItem) {
        let classes = 'menu-item';

        if (item.items.length) {
            if (!this.iconMenu) { classes += ' menu-accordion'; }
            else {
                if (parentItem == null) { classes += ' menu-dropdown'; }
                else { classes += ' menu-accordion'; }
            }
        }
        // custom class for menu item
        const customClass = objectPath.get(item, 'custom-class');
        if (customClass) { classes += ' ' + customClass; }
        if (this.iconMenu && parentItem == null) { classes += ' pb-3'; }
        if (!this.iconMenu && this.isMenuItemIsActive(item)) { classes += ' show'; }

        return classes;
    }

    getSubMenuItemCssClass(item: AppMenuItem, parentItem: AppMenuItem): string {
        let classes = 'menu-sub';

        if (!this.iconMenu) { classes += ' menu-sub-accordion'; }
        else {
            if (parentItem == null) { classes += ' menu-sub-dropdown px-1 py-4'; }
            else { classes += ' menu-sub-accordion'; }
        }

        return classes;
    }


    // printReport(params: any, key: string) {
    //     if (typeof this.reportApi[`${key}Api`] !== 'function') return;
    //     this.isLoading = true;
    //     // this.setLoading();
    //     this.reportApi[`${key}Api`].call(this.reportApi, params)?.pipe(finalize(() => {
    //         this.cd.markForCheck();
    //         this.cd.detectChanges();
    //         this.isLoading = false;
    //         this.cd.detectChanges();
    //         // setTimeout(()=>{
    //         //     this.isLoading = false;
    //         //     this.removeFadeOut(document.getElementsByClassName("overlay ng-trigger ng-trigger-fadeIn ng-star-inserted")[0],500);
    //         //     // document.getElementById("sidebar-load").attributes.getNamedItem("ng-reflect-busy-if").value = "false";
    //         //     // document.getElementsByClassName("overlay ng-trigger ng-trigger-fadeIn ng-star-inserted")[0]?.remove();
    //         //     //(document.getElementsByClassName("modal-content")[0] as HTMLElement).click();
    //         // },50)
    //     })).subscribe(() => {
    //         this.isLoading = false;
    //     }, () => this.isLoading = false, () => this.isLoading = false);

    // }

    // printCrmReport(params: any, key: string) {
    //     if (typeof this.crmReportApi[`${key}Api`] !== 'function') return;
    //     this.isLoading = true;
    //     this.crmReportApi[`${key}Api`].call(this.crmReportApi, params)?.pipe(finalize(() => {
    //         this.cd.markForCheck();
    //         this.cd.detectChanges();
    //         this.isLoading = false;
    //         this.cd.detectChanges();
    //         // setTimeout(()=>{
    //         //     this.isLoading = false;
    //         //     this.removeFadeOut(document.getElementsByClassName("overlay ng-trigger ng-trigger-fadeIn ng-star-inserted")[0],500);
    //         //     // document.getElementById("sidebar-load").attributes.getNamedItem("ng-reflect-busy-if").value = "false";
    //         //     // document.getElementsByClassName("overlay ng-trigger ng-trigger-fadeIn ng-star-inserted")[0]?.remove();
    //         //     // document.getElementById("sidebar-load").click();
    //         //     // (document.getElementsByClassName("modal-content")[0] as HTMLElement).click();
    //         // },50)
    //     })).subscribe(() => {
    //         this.isLoading = false;
    //     }, () => this.isLoading = false, () => this.isLoading = false);
    // }

    // removeFadeOut(el, speed) {
    //     var seconds = speed / 1000;
    //     if (el) {
    //         el!.style.transition = "opacity " + seconds + "s ease";

    //         el!.style.opacity = 0;
    //         setTimeout(function () {
    //             el?.parentNode?.removeChild(el);
    //         }, speed);
    //     }

    // }

    // setLoading(){
    //     var loading = (document.getElementsByTagName("ngx-spinner")[0] as HTMLElement);

    //     var innerHtml = `<div _ngcontent-ucd-c47="" class="overlay ng-tns-c47-6 ng-trigger ng-trigger-fadeIn ng-star-inserted" style="background-color: rgba(51, 51, 51, 0.8); z-index: 99999; position: absolute; opacity: 1;"><div _ngcontent-ucd-c47="" class="ng-tns-c47-6 la-2x la-ball-clip-rotate ng-star-inserted" style="color: rgb(91, 167, 234);"><div _ngcontent-ucd-c47="" class="ng-tns-c47-6 ng-star-inserted"></div><!--bindings={
    //         "ng-reflect-ng-for-of": "0"
    //       }--></div><!--bindings={
    //         "ng-reflect-ng-if": "true"
    //       }--><!--bindings={
    //         "ng-reflect-ng-if": null
    //       }--><div _ngcontent-ucd-c47="" class="loading-text ng-tns-c47-6" style="z-index: 99999;"></div></div>`;
    //       loading.innerHTML = innerHtml;
    // }



    /**
     * Open the selected component. Display in placeholder of dynamic tab,
     * or open a dedicated tab, or open a specific tab.
     *
     * @param event
     * @param item
     */
    openComponent(event, item: AppMenuItem) {
        // if (item.route.startsWith('SERVICE_REPORT') || item.name.startsWith('SALE_REPORT') || item.route.startsWith('CRM_REPORT') || item.route.startsWith('CRAM_IMPORT') || item.route.startsWith('NPS_EXPORT')) {
        //     if (item.parameters && item.parameters['viewChildName']) {
        //         this.parameters = item.parameters;
        //         setTimeout(() => {
        //             this[item.parameters['viewChildName']].show();
        //         });
        //     }
        //     else this[item.name.charAt(0).toLowerCase() + item.name.slice(1)].show();
        //     return;
        // }
        // const functionCode = (item.parameters && item.parameters.functionCode) || item.route; // origin
        const functionCode = item.route;
        if (!functionCode) { return; }

        // Only emit event for TABS
        if (Object.values(TABS).indexOf(functionCode) < 0) { return; }
        event.stopPropagation();
        // TODO: Review Code. Replace this.eventBus.
        // TODO:
        // - 1st Show Modal Filter Tabs
        // - 2nd Show a new browser tab
        // - 3rd emit event 'openComponent' to immediatly show tab

        // if (functionCode === TABS.SERVICE_QUOTATION_AGGREGATE_APPOINTMENT) {
        //     window.open('/app/main/aggregate-appointment', '_blank');
        // } else if (functionCode === TABS.SERVICES_REPAIR_PROGRESS_GENERAL_REPAIR) {
        //     localStorage.removeItem(StorageKeys.Open_General_Repair);
        //     window.open('/screens/gj', StorageKeys.General_Repair_Opened).focus();
        switch(functionCode) {
            // case TABS.ASSY_SPS_ASSEMBLYDATASCREENA1:
            //     window.open('/screens/assemblydatascreen?screen_code=A1', 'ASSY_SPS_ASSEMBLYDATASCREENA1').focus();
            //     break;
            // case TABS.ASSY_SPS_ASSEMBLYDATASCREENA2:
            //     window.open('/screens/assemblydatascreen?screen_code=A2', 'ASSY_SPS_ASSEMBLYDATASCREENA2').focus();
            //     break;
            /*LogA */
            //Lds
            // case TABS.LOGA_LDS_LOTDIRECTSUPPLYANDONA1:
            //     window.open("/screens/lotdirectsupplyandon?pline=A1", 'LOGA_LDS_LOTDIRECTSUPPLYANDONA1').focus();
            //     break;
            // case TABS.LOGA_LDS_LOTDIRECTSUPPLYANDONA2:
            //     window.open("/screens/lotdirectsupplyandon?pline=A2", 'LOGA_LDS_LOTDIRECTSUPPLYANDONA2').focus();
            //     break;
            // case TABS.LOGA_LDS_LOTDIRECTSUPPLYJIRIRIKOKUA1:
            //     window.open("/screens/jiririkokuscreen?pline=A1", 'LOGA_LDS_LOTDIRECTSUPPLYJIRIRIKOKUA1').focus();
            //     break;
            // case TABS.LOGA_LDS_LOTDIRECTSUPPLYJIRIRIKOKUA2:
            //     window.open("/screens/jiririkokuscreen?pline=A2", 'LOGA_LDS_LOTDIRECTSUPPLYJIRIRIKOKUA2').focus();
            //     break;

            //BP2
            // case TABS.LOGA_BP2_BIGPARTPXPUP:
            //     window.open("/screens/bigpartpxpup", 'LOGA_BP2_BIGPARTPXPUP').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTJIRIRIKOKUA1NEW:
            //     window.open("/screens/LgaBp2ProgressMonitorScreen?pline=A1", 'LOGA_BP2_BIGPARTJIRIRIKOKUA1NEW').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTJIRIRIKOKUA2NEW:
            //     window.open("/screens/LgaBp2ProgressMonitorScreen?pline=A2", 'LOGA_BP2_BIGPARTJIRIRIKOKUA2NEW').focus();
            //     break;

            // case TABS.LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA1NEW:
            //     window.open("/screens/LgaBp2ProgressScreen?pline=A2", 'LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA1NEW').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA2NEW:
            //     window.open("/screens/LgaBp2ProgressScreen?pline=A2", 'LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA2NEW').focus();
            //     break;

            // case TABS.LOGA_BP2_BIGPARTJIRIRIKOKUA1:
            //     window.open("/screens/bigpartjiririkokuscreen?pline=A2", 'LOGA_BP2_BIGPARTJIRIRIKOKUA1').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTJIRIRIKOKUA2:
            //     window.open("/screens/bigpartjiririkokuscreen?pline=A2", 'LOGA_BP2_BIGPARTJIRIRIKOKUA2').focus();
            //     break;

            // case TABS.LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA1:
            //     window.open("/screens/bigpartdirectdeliveryprogressandon?pline=A1", 'LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA1').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA2:
            //     window.open("/screens/bigpartdirectdeliveryprogressandon?pline=A2", 'LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA2').focus();
            //     break;

            // case TABS.LOGA_BP2_BIGPARTTABLETA1:
            //     window.open("/screens/bigparttablet2?prod_line=A1&ecar_id=1", 'LOGA_BP2_BIGPARTTABLETA1').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTTABLETA2:
            //     window.open("/screens/bigparttablet2?prod_line=A2&ecar_id=3", 'LOGA_BP2_BIGPARTTABLETA2').focus();
            //     break;

            //EKB
            // case TABS.LOGA_EKB_PROGRESSSCREEN:
            //     window.open("/screens/progressscreen", 'LOGA_EKB_PROGRESSSCREEN').focus();
            //     break;
            // case TABS.LOGA_EKB_EKANBAN:
            //     window.open("/app/main/loga/ekb/ekanban", 'LOGA_EKB_EKANBAN').focus();
            //     break;
            // case TABS.LOGA_EKB_EKANBANPROGRESSSCREENA1:
            //     window.open("/screens/ekanbanprogressscreen?pline=A2", 'LOGA_EKB_EKANBANPROGRESSSCREENA1').focus();
            //     break;
            // case TABS.LOGA_EKB_EKANBANPROGRESSSCREENA2:
            //     window.open("/screens/ekanbanprogressscreen?pline=A2", 'LOGA_EKB_EKANBANPROGRESSSCREENA2').focus();
            //     break;

            // LogW
            // case TABS.LGW_MWH_LOGW_ADO_CALLINGLIGHTW1:
            //     window.open("/screens/callinglight?prod_line=W1", 'LGW_MWH_LOGW_ADO_CALLINGLIGHTW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_ADO_CALLINGLIGHTW2:
            //     window.open("/screens/callinglight?prod_line=W2", 'LGW_MWH_LOGW_ADO_CALLINGLIGHTW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_ADO_CALLINGLIGHTW3:
            //     window.open("/screens/callinglight?prod_line=W3", 'LGW_MWH_LOGW_ADO_CALLINGLIGHTW3').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_ADO_CALLINGLIGHTIP:
            //     window.open("/screens/callinglight?prod_line=IP", 'LGW_MWH_LOGW_ADO_CALLINGLIGHTIP').focus();
            //     break;
            // // Mwh
            // case TABS.LGW_MWH_LOGW_MWH_MWHSMALLPART:
            //     window.open("/screens/pxpsmallpartinput", 'LGW_MWH_LOGW_MWH_MWHSMALLPART').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_MWH_MWHBIGPART:
            //     window.open("/screens/pxpbigpartinput", 'LGW_MWH_LOGW_MWH_MWHBIGPART').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_MWH_STOCKATWH:
            //     window.open("/screens/stockatwh", 'LGW_MWH_LOGW_MWH_STOCKATWH').focus();
            //     break;

            //Lup
            // case TABS.LGW_MWH_LOGW_PUP_LOTUNPACKINGW1:
            //     window.open("/screens/lotupackingandon?Line=W1", 'LGW_MWH_LOGW_PUP_LOTUNPACKINGW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PUP_LOTUNPACKINGW2:
            //     window.open("/screens/lotupackingandon?Line=W2", 'LGW_MWH_LOGW_PUP_LOTUNPACKINGW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PUP_LOTUNPACKINGW3:
            //     window.open("/screens/lotupackingandon?Line=W3", 'LGW_MWH_LOGW_PUP_LOTUNPACKINGW3').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_LUP_CONTMODULE:
            //     window.open("/app/main/logw/lup/contmodule", 'LGW_MWH_LOGW_LUP_CONTMODULE').focus();
            //     break;

            //Dvn
            // case TABS.LGW_MWH_LOGW_DVN_DEVANNINGSCREEN:
            //     window.open("/screens/devanningscreen", 'LGW_MWH_LOGW_DVN_DEVANNINGSCREEN').focus();
            //     break;

            //Pup
            // case TABS.LGW_MWH_LOGW_PUP_MODULEUNPACKINGANDON:
            //     window.open("/screens/moduleunpackingandon", 'LGW_MWH_LOGW_PUP_MODULEUNPACKINGANDON').focus();
            //     break;
            //Pik
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETUBW1:
            //     window.open("/screens/pickingtabletandon?PickingPosition=UB_W1&TabletId=PIK_LW_UB_W1_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETUBW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETSMW1:
            //     window.open("/screens/pickingtabletandon?PickingPosition=SM_W1&TabletId=PIK_LW_SM_W1_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETSMW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETUBW2:
            //     window.open("/screens/pickingtabletandon?PickingPosition=UB_W2&TabletId=PIK_LW_UB_W2_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETUBW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETSMW2:
            //     window.open("/screens/pickingtabletandon?PickingPosition=SM_W2&TabletId=PIK_LW_SM_W2_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETSMW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETUBW3:
            //     window.open("/screens/pickingtabletandon?PickingPosition=UB_W3&TabletId=PIK_LW_UB_W3_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETUBW3').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETSMW3:
            //     window.open("/screens/pickingtabletandon?PickingPosition=SM_W3&TabletId=PIK_LW_SM_W3_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETSMW3').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGMONITORW1:
            //     window.open("/screens/pickingmonitoringscreen?pline=W1", 'LGW_MWH_LOGW_PIK_PICKINGMONITORW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGMONITORW2:
            //     window.open("/screens/pickingmonitoringscreen?pline=W2", 'LGW_MWH_LOGW_PIK_PICKINGMONITORW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGMONITORW3:
            //     window.open("/screens/pickingmonitoringscreen?pline=W3", 'LGW_MWH_LOGW_PIK_PICKINGMONITORW3').focus();
            //     break;

            /*Painting Andon*/
            // case TABS.PTA_ADO_CCRMONITOR:
            //     window.open("/screens/ccrmonitor", 'PTA_ADO_CCRMONITOR').focus();
            //     break;
            // case TABS.PTA_ADO_LINEREALTIMECONTROL:
            //     window.open("/screens/linerealtimecontrols", 'PTA_ADO_LINEREALTIMECONTROL').focus();
            //     break;
            // case TABS.PTA_ADO_DELAYCONTROLSCREEN:
            //     window.open("/screens/delaycontrolscreen",'PTA_ADO_DELAYCONTROLSCREEN').focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERCLRINDICATOR:
            //     window.open("/screens/bumpergetdataclrindicator",'PTA_ADO_BUMPERCLRINDICATOR' ).focus();
            //     break;
            // case TABS.PTA_ADO_NEXTEDIN:
            //     window.open("/screens/nextedin" ,'PTA_ADO_NEXTEDIN' ).focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERGETDATASMALLSUBASSY:
            //     window.open("/screens/bumpergetdatasmallsubassy", 'PTA_ADO_BUMPERGETDATASMALLSUBASSY' ).focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERIN:
            //     window.open("/screens/bumperin" , 'PTA_ADO_BUMPERIN' ).focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERSUBASSYA1:
            //     window.open("/screens/bumpersubassy?pline=A1", 'PTA_ADO_BUMPERSUBASSYA1' ).focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERSUBASSYA2:
            //     window.open("/screens/bumpersubassy?pline=A2", 'PTA_ADO_BUMPERSUBASSYA2' ).focus();
            //     break;

            //Welding Andon
            // case TABS.WEL_ADO_PROCESSINSTRUCTION:
            //     window.open("/screens/processinstruction?Line=W1&Screen=BODY_EC" ,'WEL_ADO_PROCESSINSTRUCTION').focus();
            //     break;
            // case TABS.WEL_ADO_PUNCHQUEUEINDICATOR:
            //     window.open("/app/main/welding/andon/punchqueueindicator",'WEL_ADO_PUNCHQUEUEINDICATOR' ).focus();
            //     break;


            default:
                this.eventBus.emit({
                    type: 'openComponent',
                    functionCode: functionCode,
                    tabHeader: this.l(item.name),
                    params: item.parameters
                });
                break;
        }

    }
}
