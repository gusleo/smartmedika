using dna.core.libs.Extension;
using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Helper.ContentGenerator
{
    public static class ContentGenerator
    {

        public static Content BookingDoctor(HospitalAppointmentModel model, bool includeLinkDownload = true)
        {
            
            
            string linkdonwload = includeLinkDownload == false ? String.Empty : "Untuk update antrian secara real time, silahkan download aplikasi SmartMedika pada link berikut: http://smartmedika.com/download";            
           

            return new Content
            {
                Title = String.Format("Antrian di {0} {1} {2}", model.MedicalStaff.Title, model.MedicalStaff.FirstName, model.MedicalStaff.LastName),
                Body = String.Format("Antrian anda No. {0} dengan estimasi waktu tindakan {1} yang berlokasi di {2}. {3}", model.QueueNumber, 
                                 model.AppointmentDate.ToString("dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture), 
                                    HospitalInfo(model.Hospital), linkdonwload)
            };
        }

        public static Content CurrentAppointment(HospitalAppointmentModel model)
        {
            
            return new Content
            {
                Title = String.Format("Antrian di {0} {1} {2}", model.MedicalStaff.Title, model.MedicalStaff.FirstName, model.MedicalStaff.LastName),
                Body = String.Format("Antrian No. {0} sedang berjalan. Estimasi giliran anda {1}", model.QueueNumber, 
                                     model.AppointmentDate.ToString("dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture))
            };
        }

        
        private static string HospitalInfo(HospitalModel hospital)
        {
            string hospitalInfo = "";
            hospitalInfo = hospital.Name;
            /* if(hospital.Regency != null )
            {
                if(hospital.Regency.Region != null )
                {
                    hospitalInfo += String.Format("\n{0} {1} - {2}", hospital.Address, hospital.Regency.Name,
                        hospital.Regency.Region.Name);
                }
            }
            if ( !String.IsNullOrWhiteSpace(hospital.PrimaryPhone) )
            {
                hospitalInfo += String.Format("\nTelp: {0}", hospital.PrimaryPhone);
            } */

            return hospitalInfo;
        }

    }
}
