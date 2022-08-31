import { NativeDateAdapter } from "@angular/material";
export class IndonesiaDateAdapter extends NativeDateAdapter {
    
        format(date: Date, displayFormat: Object): string {
    
            if (displayFormat == 'input') {
                const day = date.getDate();
                const month = date.getMonth() + 1;
                const year = date.getFullYear();
                //return `${day}-${month}-${year}`;
                return this._to2digit(month) + '/' + this._to2digit(day) + '/' + year;

            } else {                
                return date.toDateString();
            }
        }

        private _to2digit(n: number) {
            return ('00' + n).slice(-2);
        }         
    }

export  const IDN_DATE_FORMATS = {
        parse: {
          dateInput: 'dd-mm-YYYY', //{day: 'numeric', month: 'numeric', year: 'numeric'},
        },
        display: {
            dateInput: 'input',
            monthYearLabel: { year: 'numeric', month: 'numeric' },
            dateA11yLabel: { year: 'numeric', month: 'long', day: 'numeric' },
            monthYearA11yLabel: { year: 'numeric', month: 'long' },
        },
      };