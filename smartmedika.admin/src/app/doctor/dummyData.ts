import { DoctorModel, HospitalStaffOperatingHoursModel} from '../model';

export class DummyData {
    getAllDoctor(): Array<DoctorModel> {
        return [
            { id: 1, firstName: 'Prof. ', lastName: 'Hendra Santosa', specialist: 'Anak', noSip: '123456789',
                email: 'hendrasantosa@gamil.com', noTelp: '0817895654123', alamat: 'Jl. Danau Toab Gg. Kerinci No. 3',
                city: 'Denpasar', expired: '1 Januari 2018', status: 0 },
            { id: 2, firstName: 'Dr. ', lastName: 'Upadana', specialist: 'Kandungan', noSip: '5478963210',
                email: 'drupadana@gamil.com', noTelp: '08123456897', alamat: 'Jl. Gunung Rinjani Gg. Kelapa No. 10',
                city: 'Denpasar', expired: '23 Maret 2018', status: 1 },
            { id: 3, firstName: 'Dr. ', lastName: 'Suyasa', specialist: 'Ortopedi', noSip: '25897456310',
                email: 'drsuyasa@gamil.com', noTelp: '08155556887910', alamat: 'Jl. Raya IB. Mantra Gg. Kalimutu No. 20',
                city: 'Gianyar', expired: '30 Nopember 2018', status: 1 },
            { id: 4, firstName: 'dr. ', lastName: 'Wirama', specialist: 'Umum', noSip: '888899974110',
                email: 'wiramadokter@gamil.com', noTelp: '08166678992112', alamat: 'Jl. Dewi Madri Gg. Telaga waja No. 25',
                city: 'Denpasar', expired: '5 April 2018', status: 0 },
            { id: 5, firstName: 'Dr. ', lastName: 'Wahyu', specialist: 'Kandungan', noSip: '7758955642230',
                email: 'wahyudokter@gamil.com', noTelp: '0819167895462', alamat: 'Jl. Narakusuma Gg. No. 50',
                city: 'Denpasar', expired: '5 Juni 2018', status: 1 },
            { id: 6, firstName: 'Dr. ', lastName: 'Surya Mahendra', specialist: 'Anak', noSip: '1313145789654',
                email: 'suryamdokter@gamil.com', noTelp: '08185213654987', alamat: 'Jl. Batuyang Gg. Kaswari No. 3',
                city: 'Gianyar', expired: '13 Mei 2018', status: 1 }
        ];
    }
    getDefaultOperatingHour(): Array<HospitalStaffOperatingHoursModel>{
        return [
            new HospitalStaffOperatingHoursModel(1, true, "09:00", "14:00"),
            new HospitalStaffOperatingHoursModel(2, true, "09:00", "14:00"),
            new HospitalStaffOperatingHoursModel(3, true, "09:00", "14:00"),
            new HospitalStaffOperatingHoursModel(4, true, "09:00", "14:00"),
            new HospitalStaffOperatingHoursModel(5, true, "09:00", "14:00"),
            new HospitalStaffOperatingHoursModel(6, true, "09:00", "14:00"),
            new HospitalStaffOperatingHoursModel(0, true, "09:00", "14:00")
        ];
    }

}
