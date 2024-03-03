import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DrmStockPartExcelComponent } from './drmstockpartexcel.component';
import { ImportDrmStockPartExcelComponent } from './import-drmstockpartexcel.component';
import { ListErrorImportDrmStockPartExcelComponent } from './list-error-import-drmstockpartexcel-modal.component';
import { DrmStockPartExcelRoutingModule } from './drmstockpartexcel-routing.module';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_WORKINGTIME]: DrmStockPartExcelComponent
}

@NgModule({
    declarations: [
        DrmStockPartExcelComponent,
        ImportDrmStockPartExcelComponent,
        ListErrorImportDrmStockPartExcelComponent

    ],
    imports: [
        AppSharedModule, DrmStockPartExcelRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class DrmStockPartExcelModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
