import {
    Component,
    ContentChildren,
    QueryList,
    AfterContentInit
  } from "@angular/core";

  import TabComponent from "./tab.component";

  @Component({
    selector: "ngx-tabs",
    templateUrl: "./tabs.component.html",
    styleUrls: ['./tab.component.less']
  })
  export default class TabsComponent implements AfterContentInit {
    @ContentChildren(TabComponent) tabs: QueryList<TabComponent>;
    constructor() {
      console.log(this.tabs);
    }
    ngAfterContentInit() {
      console.log(this.tabs);
      const activeTabs = this.tabs.filter(tab => tab.active);

      if (activeTabs.length === 0) {
        this.selectTab(this.tabs.first);
      }
    }

    selectTab(tab: TabComponent) {
      this.tabs.toArray().forEach(tab => (tab.active = false));
      tab.active = true;
    }
  }
