export class DateHelper{

    getDayName(day: number):string{
        let weeks:string[] = ["Minggu", "Senin", "Selasa", 'Rabu', "Kamis", "Jumat", "Sabtu"];
        return weeks[day];
    }
}