using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace HealthCareSystem.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
