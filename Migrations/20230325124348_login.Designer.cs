﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using apoio_decisao_medica.Data;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230325124348_login")]
    partial class login
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("apoio_decisao_medica.Models.CatDoenca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TcatDoencas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.CatExame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TcatExames");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.CatSintoma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TcatSintomas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Doenca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CatDoencaId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CatDoencaId");

                    b.ToTable("Tdoencas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.DoencaExame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DoencaId")
                        .HasColumnType("int");

                    b.Property<int>("ExameId")
                        .HasColumnType("int");

                    b.Property<int>("Relevancia")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DoencaId");

                    b.HasIndex("ExameId");

                    b.ToTable("TdoencaExames");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.DoencaSintoma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DoencaId")
                        .HasColumnType("int");

                    b.Property<int>("Relevancia")
                        .HasColumnType("int");

                    b.Property<int>("SintomaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DoencaId");

                    b.HasIndex("SintomaId");

                    b.ToTable("TdoencaSintomas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Exame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CatExameId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CatExameId");

                    b.ToTable("Texames");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Hospital", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Thospitais");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Medico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Bi")
                        .HasColumnType("int");

                    b.Property<string>("Especialidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UtilizadorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tmedicos");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Processo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DataHoraAbertura")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataHoraFecho")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DoencaId")
                        .HasColumnType("int");

                    b.Property<int>("HospitalId")
                        .HasColumnType("int");

                    b.Property<int>("MedicoId")
                        .HasColumnType("int");

                    b.Property<int>("NumeroProcesso")
                        .HasColumnType("int");

                    b.Property<int>("UtenteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DoencaId");

                    b.HasIndex("HospitalId");

                    b.HasIndex("MedicoId");

                    b.HasIndex("UtenteId");

                    b.ToTable("Tprocessos");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.ProcessoExame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExameId")
                        .HasColumnType("int");

                    b.Property<int>("ProcessoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExameId");

                    b.HasIndex("ProcessoId");

                    b.ToTable("TprocessoExames");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.ProcessoSintoma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProcessoId")
                        .HasColumnType("int");

                    b.Property<int>("SintomaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProcessoId");

                    b.HasIndex("SintomaId");

                    b.ToTable("TprocessoSintomas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Sintoma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CatSintomaId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CatSintomaId");

                    b.ToTable("Tsintomas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Utente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cidade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataNascimento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genero")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumeroUtente")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tutentes");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Utilizador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMedico")
                        .HasColumnType("bit");

                    b.Property<int>("MedicoId")
                        .HasColumnType("int");

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MedicoId")
                        .IsUnique();

                    b.ToTable("Tutilizador");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Doenca", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.CatDoenca", "CatDoenca")
                        .WithMany("Doencas")
                        .HasForeignKey("CatDoencaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CatDoenca");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.DoencaExame", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.Doenca", "Doenca")
                        .WithMany("DoencaExames")
                        .HasForeignKey("DoencaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apoio_decisao_medica.Models.Exame", "Exame")
                        .WithMany("DoencaExames")
                        .HasForeignKey("ExameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doenca");

                    b.Navigation("Exame");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.DoencaSintoma", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.Doenca", "Doenca")
                        .WithMany("DoencaSintoma")
                        .HasForeignKey("DoencaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apoio_decisao_medica.Models.Sintoma", "Sintoma")
                        .WithMany("DoencaSintoma")
                        .HasForeignKey("SintomaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doenca");

                    b.Navigation("Sintoma");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Exame", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.CatExame", "CatExame")
                        .WithMany("Exames")
                        .HasForeignKey("CatExameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CatExame");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Processo", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.Doenca", "Doenca")
                        .WithMany("Processos")
                        .HasForeignKey("DoencaId");

                    b.HasOne("apoio_decisao_medica.Models.Hospital", "Hospital")
                        .WithMany("Processos")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apoio_decisao_medica.Models.Medico", "Medico")
                        .WithMany("Processos")
                        .HasForeignKey("MedicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apoio_decisao_medica.Models.Utente", "Utente")
                        .WithMany("Processos")
                        .HasForeignKey("UtenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doenca");

                    b.Navigation("Hospital");

                    b.Navigation("Medico");

                    b.Navigation("Utente");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.ProcessoExame", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.Exame", "Exame")
                        .WithMany("ProcessoExames")
                        .HasForeignKey("ExameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apoio_decisao_medica.Models.Processo", "Processo")
                        .WithMany()
                        .HasForeignKey("ProcessoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exame");

                    b.Navigation("Processo");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.ProcessoSintoma", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.Processo", "Processo")
                        .WithMany()
                        .HasForeignKey("ProcessoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apoio_decisao_medica.Models.Sintoma", "Sintoma")
                        .WithMany("ProcessoSintomas")
                        .HasForeignKey("SintomaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Processo");

                    b.Navigation("Sintoma");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Sintoma", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.CatSintoma", "CatSintoma")
                        .WithMany("Sintomas")
                        .HasForeignKey("CatSintomaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CatSintoma");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Utilizador", b =>
                {
                    b.HasOne("apoio_decisao_medica.Models.Medico", "Medico")
                        .WithOne("Utilizador")
                        .HasForeignKey("apoio_decisao_medica.Models.Utilizador", "MedicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medico");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.CatDoenca", b =>
                {
                    b.Navigation("Doencas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.CatExame", b =>
                {
                    b.Navigation("Exames");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.CatSintoma", b =>
                {
                    b.Navigation("Sintomas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Doenca", b =>
                {
                    b.Navigation("DoencaExames");

                    b.Navigation("DoencaSintoma");

                    b.Navigation("Processos");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Exame", b =>
                {
                    b.Navigation("DoencaExames");

                    b.Navigation("ProcessoExames");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Hospital", b =>
                {
                    b.Navigation("Processos");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Medico", b =>
                {
                    b.Navigation("Processos");

                    b.Navigation("Utilizador");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Sintoma", b =>
                {
                    b.Navigation("DoencaSintoma");

                    b.Navigation("ProcessoSintomas");
                });

            modelBuilder.Entity("apoio_decisao_medica.Models.Utente", b =>
                {
                    b.Navigation("Processos");
                });
#pragma warning restore 612, 618
        }
    }
}
