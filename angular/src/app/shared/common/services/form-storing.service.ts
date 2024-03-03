import { Injectable } from '@angular/core';

class ColumnForSaveDto {
    field: string;
    width: number;
    position: number;
    constructor(_field, _width, _position) {
      this.field = _field;
      this.width = _width;
      this.position = _position;
    }
}

@Injectable({
  providedIn: 'root',
})
export class FormStoringService {

  constructor() {}

  set(key, data) {
    localStorage.setItem(key, JSON.stringify(data));
  }

  get(key) {
    const val = localStorage.getItem(key);
    return val ? JSON.parse(val) : '';
  }

  clear(key) {
    localStorage.removeItem(key);
  }

  dateComparator(date1: string, date2: string) {
    console.log("date1:" + date1 + " - date2: " + date2); 
    const date1Number = monthToComparableNumber(date1);
    const date2Number = monthToComparableNumber(date2);
    if (date1Number === null && date2Number === null) {
      return 0;
    }
    if (date1Number === null) {
      return -1;
    }
    if (date2Number === null) {
      return 1;
    }
    return date1Number - date2Number;
  }
  dateTimeComparator(date1: string, date2: string) {
    const date1Number = dateTimeToComparableNumber(date1);
    const date2Number = dateTimeToComparableNumber(date2);
    if (date1Number === null && date2Number === null) {
      return 0;
    }
    if (date1Number === null) {
      return -1;
    }
    if (date2Number === null) {
      return 1;
    }
    return date1Number - date2Number;
  }

  timeComparator(time1: string, time2: string) {
    const time1Number = timeToComparableNumber(time1);
    const time2Number = timeToComparableNumber(time2);
    if (time1Number === null && time2Number === null) {
      return 0;
    }
    if (time1Number === null) {
      return -1;
    }
    if (time2Number === null) {
      return 1;
    }
    return time1Number - time2Number;
  }

   decimalComparator(value1, value2) {
    var value1Number = Number.parseFloat(value1)*1000000000;
    var value2Number = Number.parseFloat(value2)*1000000000;

    return value1Number - value2Number;
  }
}


function monthToComparableNumber(date: string) {
    if (date === undefined || date === null || date.length !== 10) {
      return null;
    }
    const yearNumber = Number.parseInt(date.substring(6, 10));
    const monthNumber = Number.parseInt(date.substring(3, 5));
    const dayNumber = Number.parseInt(date.substring(0, 2));
    return yearNumber * 10000 + monthNumber * 100 + dayNumber;
  }

// eg DD/MM/YYYY HH:mm gets converted to YYYYMMDDHHmm
function dateTimeToComparableNumber(date: string) {
    if (date === undefined || date === null || date.length !== 16) {
      return null;
    }
    const minuteNumber = Number.parseInt(date.substring(14, 16));
    const hourNumber = Number.parseInt(date.substring(11, 13));
    const yearNumber = Number.parseInt(date.substring(6, 10));
    const monthNumber = Number.parseInt(date.substring(3, 5));
    const dayNumber = Number.parseInt(date.substring(0, 2));

    return yearNumber * 100000000 + monthNumber * 1000000 + dayNumber * 10000 + hourNumber * 100 + minuteNumber;
}


function timeToComparableNumber(time: string) {
    if (time === undefined || time === null || time.length !== 8) {
      return null;
    }
    const hourNumber = Number.parseInt(time.substring(0,2));

    const minuteNumber = Number.parseInt(time.substring(3,5));

    const secondNumber = Number.parseInt(time.substring(6,8));

    return hourNumber * 10000 + minuteNumber * 100 + secondNumber;
}
