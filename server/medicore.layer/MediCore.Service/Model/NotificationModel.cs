using dna.core.service.Models;
using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MediCore.Service.Model
{
    public class NotificationModel : WriteHistoryBaseModel, IModelBase
    {
        public const string APPOINTMENT_QUEUE_SCREEN = "AppointmentQueue";
        public const string DOCTOR_RATING_REQUEST = "DoctorRatingRequest";
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        //Related identifier
        public int ObjectId { get; set; }

        [Required]
        public string TargetScreen { get; set; }

        public string JsonData { get; set; }

        public bool IsRead { get; set; }

        //send to
        public int UserId { get; set; }

       
        public virtual UserModel User { get; set; }

        //not mapped
        public int TotalNotification { get; set; }
    }
}
