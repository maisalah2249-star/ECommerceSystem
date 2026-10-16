
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HealthCareSystem.Entities;
using HealthCareSystem.Data;

namespace HealthCareSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new HealthCareContext();

            try
            {
                Console.WriteLine($"Database: {context.Database.GetDbConnection().Database}");
                Console.WriteLine($"Server: {context.Database.GetDbConnection().DataSource}");

                if (!context.Patients.Any())
                {
                    context.Patients.AddRange(
                        new Patient
                        {
                            Name = "Mai Salah",
                            DateOfBirth = new DateTime(2000, 4, 15)
                        },
                        new Patient
                        {
                            Name = "Ahmed Mohammed",
                            DateOfBirth = new DateTime(1995, 9, 22)
                        },
                        new Patient
                        {
                            Name = "Nour Mohammed",
                            DateOfBirth = new DateTime(1988, 2, 10)
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.Doctors.Any())
                {
                    context.Doctors.AddRange(
                        new Doctor
                        {
                            Name = "Dr. Ahmed Hassan",
                            Specialization = "Cardiology"
                        },
                        new Doctor
                        {
                            Name = "Dr. Sara Mahmoud",
                            Specialization = "Pediatrics"
                        },
                        new Doctor
                        {
                            Name = "Dr. Fatma Salah",
                            Specialization = "Dermatology"
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.Appointments.Any())
                {
                    var patients = context.Patients
                        .OrderBy(p => p.Id)
                        .Take(3)
                        .ToList();

                    var doctors = context.Doctors
                        .OrderBy(d => d.Id)
                        .Take(3)
                        .ToList();

                    for (int i = 0; i < Math.Min(patients.Count, doctors.Count); i++)
                    {
                        context.Appointments.Add(new Appointment
                        {
                            PatientId = patients[i].Id,
                            DoctorId = doctors[i].Id,
                            AppointmentDate = DateTime.Today.AddHours(9 + (i * 2))
                        });
                    }

                    context.SaveChanges();
                }

                context.ChangeTracker.Clear();

                Console.WriteLine("\n========== DATABASE COUNTS ==========");
                Console.WriteLine($"Patients: {context.Patients.Count()}");
                Console.WriteLine($"Doctors: {context.Doctors.Count()}");
                Console.WriteLine($"Appointments: {context.Appointments.Count()}");

                Console.WriteLine("\n========== DOCTORS AND APPOINTMENTS ==========");

                var doctorsWithAppointments = context.Doctors
                    .Include(d => d.Appointments)
                        .ThenInclude(a => a.Patient)
                    .ToList();

                foreach (var doctor in doctorsWithAppointments)
                {
                    Console.WriteLine($"\nDoctor: {doctor.Name}");
                    Console.WriteLine($"Specialization: {doctor.Specialization}");

                    if (!doctor.Appointments.Any())
                    {
                        Console.WriteLine("No appointments.");
                    }

                    foreach (var appointment in doctor.Appointments)
                    {
                        Console.WriteLine(
                            $"Patient: {appointment.Patient?.Name ?? "Unknown Patient"} | Date: {appointment.AppointmentDate:g}");
                    }
                }

                Console.WriteLine("\n========== PATIENTS AND APPOINTMENTS ==========");

                var patientsWithAppointments = context.Patients
                    .Include(p => p.Appointments)
                        .ThenInclude(a => a.Doctor)
                    .ToList();

                foreach (var patient in patientsWithAppointments)
                {
                    Console.WriteLine($"\nPatient: {patient.Name}");
                    Console.WriteLine($"Date of Birth: {patient.DateOfBirth:d}");

                    if (!patient.Appointments.Any())
                    {
                        Console.WriteLine("No appointments.");
                    }

                    foreach (var appointment in patient.Appointments)
                    {
                        Console.WriteLine(
                            $"Doctor: {appointment.Doctor?.Name ?? "Unknown Doctor"} | Specialization: {appointment.Doctor?.Specialization ?? "Unknown"} | Date: {appointment.AppointmentDate:g}");
                    }
                }

                Console.WriteLine("\n========== ALL APPOINTMENTS ==========");

                var allAppointments = context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .OrderBy(a => a.AppointmentDate)
                    .ToList();

                foreach (var appointment in allAppointments)
                {
                    Console.WriteLine(
                        $"Appointment ID: {appointment.Id} | Patient: {appointment.Patient?.Name ?? "Unknown"} | Doctor: {appointment.Doctor?.Name ?? "Unknown"} | Date: {appointment.AppointmentDate:g}");
                }

                Console.WriteLine("\nHealthcare project completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong:");
                Console.WriteLine(ex);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
