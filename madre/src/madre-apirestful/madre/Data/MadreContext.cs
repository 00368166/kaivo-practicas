using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace madre.Data
{
    public class MadreContext : DbContext
    {
        public DbSet<ComputadoraHija> ComputadorasHijas { get; set; }
        public DbSet<RegistroPrograma> RegistrosProgramas { get; set; }
        public DbSet<Error> Errores { get; set; }
        public DbSet<ErrorRegistro> ErroresRegistros { get; set; }

        public MadreContext(DbContextOptions<MadreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComputadoraHija>()
                .HasKey(c => c.Ip);

            modelBuilder.Entity<ComputadoraHija>()
                .HasMany(c => c.RegistrosProgramas)
                .WithOne(r => r.ComputadoraHija)
                .HasForeignKey(r => r.ComputadoraHijaIp);

            modelBuilder.Entity<RegistroPrograma>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RegistroPrograma>()
                .HasOne(r => r.Error)
                .WithMany(e => e.RegistrosProgramas)
                .HasForeignKey(r => r.ErrorId);

            modelBuilder.Entity<Error>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<ErrorRegistro>()
                .HasKey(er => er.Id);

            modelBuilder.Entity<ErrorRegistro>()
                .HasOne(er => er.RegistroPrograma)
                .WithMany(r => r.ErroresRegistros)
                .HasForeignKey(er => er.RegistroProgramaId);

            modelBuilder.Entity<ErrorRegistro>()
                .HasOne(er => er.Error)
                .WithMany(e => e.ErroresRegistros)
                .HasForeignKey(er => er.ErrorId);

            // Datos pregenerados
            modelBuilder.Entity<ComputadoraHija>().HasData(
                new ComputadoraHija { Ip = "192.168.1.210:7050", Nombre = "Computadora 1", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.211:7051", Nombre = "Computadora 2", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.212:7052", Nombre = "Computadora 3", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.213:7053", Nombre = "Computadora 4", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.214:7054", Nombre = "Computadora 5", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.215:7055", Nombre = "Computadora 6", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.216:7056", Nombre = "Computadora 7", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.217:7057", Nombre = "Computadora 8", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.218:7058", Nombre = "Computadora 9", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.219:7059", Nombre = "Computadora 10", Status = false, Permiso = false },
                new ComputadoraHija { Ip = "192.168.1.220:7060", Nombre = "Computadora 11", Status = false, Permiso = false }
                // Agrega más computadoras aquí
            );

            modelBuilder.Entity<Error>().HasData(
                new Error { Id = 1, ErrorCode = "ERR001", Descripcion = "Error 1: causa impredecible" },
                new Error { Id = 2, ErrorCode = "ERR002", Descripcion = "Error 2: causa imposible" },
                new Error { Id = 3, ErrorCode = "ERR003", Descripcion = "Error 3: causa improvable" }
                // Agrega más errores aquí
            );

            // Agrega más datos pregenerados aquí

            base.OnModelCreating(modelBuilder);
        }
    }

    public class ComputadoraHija
    {
        [MaxLength(50)] // Limitar la longitud a 255 caracteres
        public string Ip { get; set; }

        public string Nombre { get; set; }
        public bool Status { get; set; }
        public bool Permiso { get; set; }

        public ICollection<RegistroPrograma> RegistrosProgramas { get; set; }
    }

    public class RegistroPrograma
    {
        public int Id { get; set; }
        public string NombrePrograma { get; set; }
        public int Pid { get; set; }
        public string Usuario { get; set; }
        public string Ventana { get; set; }
        public string TituloVentana { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }

        public string ComputadoraHijaIp { get; set; }
        public ComputadoraHija ComputadoraHija { get; set; }

        public int? ErrorId { get; set; }
        public Error Error { get; set; }

        public ICollection<ErrorRegistro> ErroresRegistros { get; set; }
    }

    public class Error
    {
        public int Id { get; set; }
        public string ErrorCode { get; set; }
        public string Descripcion { get; set; }

        public ICollection<RegistroPrograma> RegistrosProgramas { get; set; }
        public ICollection<ErrorRegistro> ErroresRegistros { get; set; }
    }

    public class ErrorRegistro
    {
        public int Id { get; set; }

        public int RegistroProgramaId { get; set; }
        public RegistroPrograma RegistroPrograma { get; set; }

        public int ErrorId { get; set; }
        public Error Error { get; set; }
    }
}
