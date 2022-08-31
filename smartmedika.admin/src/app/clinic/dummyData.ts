import { HospitalOpratingHoursModel } from '../model';

/**
 * Class for default data related to clinic
 */
export class DummyData{
    getDefaultOperatingHour(): Array<HospitalOpratingHoursModel>{
        return [
            new HospitalOpratingHoursModel(1, true, "09:00", "14:00"),
            new HospitalOpratingHoursModel(2, true, "09:00", "14:00"),
            new HospitalOpratingHoursModel(3, true, "09:00", "14:00"),
            new HospitalOpratingHoursModel(4, true, "09:00", "14:00"),
            new HospitalOpratingHoursModel(5, true, "09:00", "14:00"),
            new HospitalOpratingHoursModel(6, true, "09:00", "14:00"),
            new HospitalOpratingHoursModel(0, true, "09:00", "14:00")
        ];
    }
}