using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.InputParam
{
    /// <summary>
    /// Model for Firebase
    /// </summary>
    public class FirebaseModel
    {
        /// <summary>
        /// Firebase Token to Send
        /// </summary>
        public string DeviceToken { get; set; }

        /// <summary>
        /// Notification Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Notification Content
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Screen or Modal want to open on apps
        /// </summary>
        public string TargetScreen { get; set; }

        /// <summary>
        /// Optional data want to send
        /// </summary>
        public string JsonData { get; set; }
    }
}
