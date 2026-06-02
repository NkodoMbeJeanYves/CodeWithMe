using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWithMe.Core.Migrations
{
    /// <inheritdoc />
    public partial class EduDomainTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EduAnneesAcademiques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateDebut = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Statut = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, defaultValue: "en_preparation")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypePeriode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduAnneesAcademiques", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduApprenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NumeroInscription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prenom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateNaissance = table.Column<DateOnly>(type: "date", nullable: false),
                    LieuNaissance = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Genre = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nationalite = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhotoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ville = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pays = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "actif")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduApprenants", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduCampus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nom = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ville = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelephoneDirecteur = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Principal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduCampus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduCreneauxHoraires",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeureDebut = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HeureFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    DureeMinutes = table.Column<int>(type: "int", nullable: false),
                    Ordre = table.Column<int>(type: "int", nullable: false),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduCreneauxHoraires", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduCycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeFormation = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Ordre = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduCycles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduDeliberations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PeriodeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClasseOuPromotionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClasseOuPromotionLibelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Session = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateDeliberation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    President = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompensationActivee = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PvUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SignePar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateSigne = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DatePublication = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduDeliberations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduEnseignants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Matricule = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prenom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Genre = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateNaissance = table.Column<DateOnly>(type: "date", nullable: true),
                    Adresse = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhotoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "actif")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeContrat = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateEntree = table.Column<DateOnly>(type: "date", nullable: false),
                    DateSortie = table.Column<DateOnly>(type: "date", nullable: true),
                    NiveauDiplome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChargeHoraireMax = table.Column<int>(type: "int", nullable: true),
                    TauxHoraire = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduEnseignants", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduEvenementsCalendrier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Titre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateDebut = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduEvenementsCalendrier", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduModelesMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Evenement = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sujet = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Corps = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduModelesMessage", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduParametresAbsenteisme",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SeuilAlerte = table.Column<int>(type: "int", nullable: false),
                    DelaiSaisieHeures = table.Column<int>(type: "int", nullable: false),
                    AbsenceExamenNote0 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotificationParent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotificationDelaiHeures = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduParametresAbsenteisme", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduPeriodesInscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOuverture = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateCloture = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ouverte = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CapaciteMax = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduPeriodesInscription", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduSessionsExamen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PeriodeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateDebut = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduSessionsExamen", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduPeriodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateDebut = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduPeriodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduPeriodes_EduAnneesAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalTable: "EduAnneesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduMoyennesGenerales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PeriodeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Moyenne = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Rang = table.Column<int>(type: "int", nullable: true),
                    TotalApprenants = table.Column<int>(type: "int", nullable: true),
                    Mention = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EctsAcquis = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    EctsTotal = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    Validee = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "en_cours")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduMoyennesGenerales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduMoyennesGenerales_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduPiecesJustificatives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FichierUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Commentaire = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UploadedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ValidatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduPiecesJustificatives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduPiecesJustificatives_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduTuteurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prenom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LienParente = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelephoneSecondaire = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Profession = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccesPortail = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduTuteurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduTuteurs_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduSalles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CampusId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nom = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Capacite = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "disponible")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Batiment = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Etage = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduSalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduSalles_EduCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "EduCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduFilieres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SystemeLMD = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DureeAnnees = table.Column<int>(type: "int", nullable: false),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduFilieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduFilieres_EduCycles_CycleId",
                        column: x => x.CycleId,
                        principalTable: "EduCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduDeliberationLignes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DeliberationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MoyenneGenerale = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    EctsAcquis = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    SemestreValide = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Decision = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mention = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Commentaire = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CasSpecial = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ModifieeManuel = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduDeliberationLignes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduDeliberationLignes_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduDeliberationLignes_EduDeliberations_DeliberationId",
                        column: x => x.DeliberationId,
                        principalTable: "EduDeliberations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduDeliberationMembres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeliberationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Membre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduDeliberationMembres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduDeliberationMembres_EduDeliberations_DeliberationId",
                        column: x => x.DeliberationId,
                        principalTable: "EduDeliberations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduAbsencesEnseignants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnseignantRemplacantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateDebut = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Motif = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RemplacementPrevu = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduAbsencesEnseignants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduAbsencesEnseignants_EduEnseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduAbsencesEnseignants_EduEnseignants_EnseignantRemplacantId",
                        column: x => x.EnseignantRemplacantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduEnseignantSpecialites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduEnseignantSpecialites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduEnseignantSpecialites_EduEnseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduIndisponibilites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateDebut = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HeureDebut = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    HeureFin = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    Motif = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduIndisponibilites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduIndisponibilites_EduEnseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduBulletins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PeriodeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "bulletin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "brouillon")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MoyenneGenerale = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MoyenneClasse = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Rang = table.Column<int>(type: "int", nullable: true),
                    TotalApprenants = table.Column<int>(type: "int", nullable: true),
                    Mention = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Decision = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AppreciationGenerale = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AppreciationProfPrincipal = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BilanAbsTotal = table.Column<int>(type: "int", nullable: false),
                    BilanAbsJustifiees = table.Column<int>(type: "int", nullable: false),
                    BilanAbsInjustifiees = table.Column<int>(type: "int", nullable: false),
                    SignePar = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateSigne = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DatePublication = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PdfUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduBulletins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduBulletins_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduBulletins_EduPeriodes_PeriodeId",
                        column: x => x.PeriodeId,
                        principalTable: "EduPeriodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduSalleEquipements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SalleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduSalleEquipements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduSalleEquipements_EduSalles_SalleId",
                        column: x => x.SalleId,
                        principalTable: "EduSalles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduNiveaux",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FiliereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ordre = table.Column<int>(type: "int", nullable: false),
                    TypeFormation = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduNiveaux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduNiveaux_EduFilieres_FiliereId",
                        column: x => x.FiliereId,
                        principalTable: "EduFilieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NiveauId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FiliereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProfesseurPrincipalId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CapaciteMax = table.Column<int>(type: "int", nullable: false),
                    EffectifActuel = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "active")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salle = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduClasses_EduNiveaux_NiveauId",
                        column: x => x.NiveauId,
                        principalTable: "EduNiveaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduPromotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NiveauId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FiliereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResponsableId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CapaciteMax = table.Column<int>(type: "int", nullable: false),
                    EffectifActuel = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "active")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduPromotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduPromotions_EduNiveaux_NiveauId",
                        column: x => x.NiveauId,
                        principalTable: "EduNiveaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduUniteEnseignements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FiliereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NiveauId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Semestre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Libelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Credits = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Coefficient = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    VolHoraireTotal = table.Column<int>(type: "int", nullable: false),
                    NatureEvaluation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PonderationCC = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PonderationExamen = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Eliminatoire = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SeuilValidation = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Compensable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduUniteEnseignements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduUniteEnseignements_EduFilieres_FiliereId",
                        column: x => x.FiliereId,
                        principalTable: "EduFilieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduUniteEnseignements_EduNiveaux_NiveauId",
                        column: x => x.NiveauId,
                        principalTable: "EduNiveaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduGroupes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PromotionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Libelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CapaciteMax = table.Column<int>(type: "int", nullable: false),
                    EffectifActuel = table.Column<int>(type: "int", nullable: false),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduGroupes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduGroupes_EduPromotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "EduPromotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduInscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "brouillon")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClasseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PromotionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    NumeroInscription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateInscription = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateLimiteValidation = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateValidation = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ValidePar = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotifRejet = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FraisInscription = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    FraisPayes = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ListAttente = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PositionListeAttente = table.Column<int>(type: "int", nullable: true),
                    Commentaire = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReinscriptionDepuisId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduInscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduInscriptions_EduAnneesAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalTable: "EduAnneesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduInscriptions_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduInscriptions_EduClasses_ClasseId",
                        column: x => x.ClasseId,
                        principalTable: "EduClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EduInscriptions_EduInscriptions_ReinscriptionDepuisId",
                        column: x => x.ReinscriptionDepuisId,
                        principalTable: "EduInscriptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EduInscriptions_EduPromotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "EduPromotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduMatieres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FiliereId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    NiveauId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Libelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Coefficient = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    VolHoraireCM = table.Column<int>(type: "int", nullable: false),
                    VolHoraireTD = table.Column<int>(type: "int", nullable: false),
                    VolHoraireTP = table.Column<int>(type: "int", nullable: false),
                    VolHoraireTotal = table.Column<int>(type: "int", nullable: false),
                    NatureEvaluation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PonderationCC = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PonderationExamen = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Eliminatoire = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SeuilEliminatoire = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NoteMax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduMatieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduMatieres_EduFilieres_FiliereId",
                        column: x => x.FiliereId,
                        principalTable: "EduFilieres",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EduMatieres_EduUniteEnseignements_UeId",
                        column: x => x.UeId,
                        principalTable: "EduUniteEnseignements",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduInscriptionGroupes",
                columns: table => new
                {
                    InscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GroupeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduInscriptionGroupes", x => new { x.InscriptionId, x.GroupeId });
                    table.ForeignKey(
                        name: "FK_EduInscriptionGroupes_EduGroupes_GroupeId",
                        column: x => x.GroupeId,
                        principalTable: "EduGroupes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduInscriptionGroupes_EduInscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "EduInscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduInscriptionHistoriques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Par = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Commentaire = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduInscriptionHistoriques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduInscriptionHistoriques_EduInscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "EduInscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduInscriptionUEs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "inscrit")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduInscriptionUEs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduInscriptionUEs_EduInscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "EduInscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduInscriptionUEs_EduUniteEnseignements_UeId",
                        column: x => x.UeId,
                        principalTable: "EduUniteEnseignements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduListesAttente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClasseOuPromotionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Position = table.Column<int>(type: "int", nullable: false),
                    DateAjout = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Notifie = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduListesAttente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduListesAttente_EduInscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "EduInscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduAffectationsMatieres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MatiereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClasseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PromotionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    HeuresPrevues = table.Column<int>(type: "int", nullable: false),
                    HeuresRealisees = table.Column<int>(type: "int", nullable: false),
                    Actif = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduAffectationsMatieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduAffectationsMatieres_EduEnseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduAffectationsMatieres_EduMatieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "EduMatieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduBulletinLignes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BulletinId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MatiereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Coefficient = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NoteCC = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NotePartiel = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NoteExamen = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Moyenne = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    MoyenneClasse = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Appreciation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnseignantNom = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rang = table.Column<int>(type: "int", nullable: true),
                    Eliminatoire = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduBulletinLignes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduBulletinLignes_EduBulletins_BulletinId",
                        column: x => x.BulletinId,
                        principalTable: "EduBulletins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduBulletinLignes_EduMatieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "EduMatieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduCoursPlanifies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PeriodeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MatiereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClasseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PromotionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    GroupeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SalleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreneauId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TypeCours = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeRecurrence = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JourSemaine = table.Column<int>(type: "int", nullable: false),
                    HeureDebut = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HeureFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    DateDebutValidite = table.Column<DateOnly>(type: "date", nullable: false),
                    DateFinValidite = table.Column<DateOnly>(type: "date", nullable: false),
                    Couleur = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduCoursPlanifies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduCoursPlanifies_EduEnseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduCoursPlanifies_EduMatieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "EduMatieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduCoursPlanifies_EduSalles_SalleId",
                        column: x => x.SalleId,
                        principalTable: "EduSalles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduEpreuves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SessionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MatiereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClasseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PromotionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SalleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnseignantSurveillantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    HeureDebut = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HeureFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    DureeMinutes = table.Column<int>(type: "int", nullable: false),
                    Coefficient = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NoteMax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ConvocationsGenerees = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduEpreuves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduEpreuves_EduEnseignants_EnseignantSurveillantId",
                        column: x => x.EnseignantSurveillantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EduEpreuves_EduMatieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "EduMatieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduEpreuves_EduSalles_SalleId",
                        column: x => x.SalleId,
                        principalTable: "EduSalles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduEpreuves_EduSessionsExamen_SessionId",
                        column: x => x.SessionId,
                        principalTable: "EduSessionsExamen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduEvaluations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MatiereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PeriodeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnneeAcademiqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClasseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PromotionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Intitule = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ponderation = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Coefficient = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NoteMax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DateEvaluation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduEvaluations_EduEnseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduEvaluations_EduMatieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "EduMatieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduMoyennesMatieres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MatiereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PeriodeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Moyenne = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NoteCC = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NotePartiel = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NoteExamen = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AppreciationEnseignant = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Eliminatoire = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SeuilEliminatoire = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduMoyennesMatieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduMoyennesMatieres_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduMoyennesMatieres_EduMatieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "EduMatieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduSeances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CoursPlanifieId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EtablissementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MatiereId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClasseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PromotionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    GroupeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SalleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TypeCours = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    HeureDebut = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HeureFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    DureeMinutes = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "planifiee")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContenuEnseignant = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TravauxDemandes = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PresencesSaisies = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EstRemplacement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EnseignantRemplacantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MotifAnnulation = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotifReport = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateReport = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduSeances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduSeances_EduClasses_ClasseId",
                        column: x => x.ClasseId,
                        principalTable: "EduClasses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EduSeances_EduCoursPlanifies_CoursPlanifieId",
                        column: x => x.CoursPlanifieId,
                        principalTable: "EduCoursPlanifies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EduSeances_EduEnseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduSeances_EduEnseignants_EnseignantRemplacantId",
                        column: x => x.EnseignantRemplacantId,
                        principalTable: "EduEnseignants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EduSeances_EduGroupes_GroupeId",
                        column: x => x.GroupeId,
                        principalTable: "EduGroupes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EduSeances_EduMatieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "EduMatieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduSeances_EduPromotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "EduPromotions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EduSeances_EduSalles_SalleId",
                        column: x => x.SalleId,
                        principalTable: "EduSalles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduConvocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EpreuveId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NumeroPlace = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salle = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "generee")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateEnvoi = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Eligible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MotifIneligibilite = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduConvocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduConvocations_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduConvocations_EduEpreuves_EpreuveId",
                        column: x => x.EpreuveId,
                        principalTable: "EduEpreuves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduPVExamens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EpreuveId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateRedaction = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Observations = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SignePar = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateSigne = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "brouillon")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduPVExamens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduPVExamens_EduEpreuves_EpreuveId",
                        column: x => x.EpreuveId,
                        principalTable: "EduEpreuves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EvaluationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Valeur = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Absent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Dispense = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Commentaire = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "brouillon")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaisieParId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ValideParId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MotifModification = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduNotes_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduNotes_EduEvaluations_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "EduEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduAbsences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SeanceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MatiereId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EnseignantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ClasseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PromotionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    HeureDebut = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HeureFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    DureeHeures = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "non_justifiee")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstExamen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ImpactNote = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotifieeParent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DateNotification = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduAbsences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduAbsences_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduAbsences_EduSeances_SeanceId",
                        column: x => x.SeanceId,
                        principalTable: "EduSeances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduPresences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SeanceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Statut = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "present")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MinutesRetard = table.Column<int>(type: "int", nullable: true),
                    Remarque = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaisieParId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduPresences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduPresences_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EduPresences_EduSeances_SeanceId",
                        column: x => x.SeanceId,
                        principalTable: "EduSeances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduCasFraude",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PVExamenId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApprenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sanction = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduCasFraude", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduCasFraude_EduApprenants_ApprenantId",
                        column: x => x.ApprenantId,
                        principalTable: "EduApprenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EduCasFraude_EduPVExamens_PVExamenId",
                        column: x => x.PVExamenId,
                        principalTable: "EduPVExamens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduNotesHistorique",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NoteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AncienneValeur = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NouvelleValeur = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ModifiePar = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Motif = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduNotesHistorique", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduNotesHistorique_EduNotes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "EduNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EduJustificatifs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AbsenceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FichierUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FichierNom = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SoumisParId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateSoumission = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ValidateParId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DateValidation = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CommentaireValidation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduJustificatifs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduJustificatifs_EduAbsences_AbsenceId",
                        column: x => x.AbsenceId,
                        principalTable: "EduAbsences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EduAbsences_ApprenantId_Date_Statut",
                table: "EduAbsences",
                columns: new[] { "ApprenantId", "Date", "Statut" });

            migrationBuilder.CreateIndex(
                name: "IX_EduAbsences_SeanceId",
                table: "EduAbsences",
                column: "SeanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EduAbsencesEnseignants_EnseignantId",
                table: "EduAbsencesEnseignants",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduAbsencesEnseignants_EnseignantRemplacantId",
                table: "EduAbsencesEnseignants",
                column: "EnseignantRemplacantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduAffectationsMatieres_EnseignantId",
                table: "EduAffectationsMatieres",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduAffectationsMatieres_MatiereId",
                table: "EduAffectationsMatieres",
                column: "MatiereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduApprenants_NumeroInscription",
                table: "EduApprenants",
                column: "NumeroInscription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduBulletinLignes_BulletinId",
                table: "EduBulletinLignes",
                column: "BulletinId");

            migrationBuilder.CreateIndex(
                name: "IX_EduBulletinLignes_MatiereId",
                table: "EduBulletinLignes",
                column: "MatiereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduBulletins_ApprenantId_PeriodeId",
                table: "EduBulletins",
                columns: new[] { "ApprenantId", "PeriodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduBulletins_PeriodeId",
                table: "EduBulletins",
                column: "PeriodeId");

            migrationBuilder.CreateIndex(
                name: "IX_EduCasFraude_ApprenantId",
                table: "EduCasFraude",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduCasFraude_PVExamenId",
                table: "EduCasFraude",
                column: "PVExamenId");

            migrationBuilder.CreateIndex(
                name: "IX_EduClasses_NiveauId",
                table: "EduClasses",
                column: "NiveauId");

            migrationBuilder.CreateIndex(
                name: "IX_EduConvocations_ApprenantId",
                table: "EduConvocations",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduConvocations_EpreuveId_ApprenantId",
                table: "EduConvocations",
                columns: new[] { "EpreuveId", "ApprenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduCoursPlanifies_EnseignantId",
                table: "EduCoursPlanifies",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduCoursPlanifies_MatiereId",
                table: "EduCoursPlanifies",
                column: "MatiereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduCoursPlanifies_SalleId",
                table: "EduCoursPlanifies",
                column: "SalleId");

            migrationBuilder.CreateIndex(
                name: "IX_EduDeliberationLignes_ApprenantId",
                table: "EduDeliberationLignes",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduDeliberationLignes_DeliberationId",
                table: "EduDeliberationLignes",
                column: "DeliberationId");

            migrationBuilder.CreateIndex(
                name: "IX_EduDeliberationMembres_DeliberationId",
                table: "EduDeliberationMembres",
                column: "DeliberationId");

            migrationBuilder.CreateIndex(
                name: "IX_EduEnseignants_Email",
                table: "EduEnseignants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduEnseignants_EtablissementId_Matricule",
                table: "EduEnseignants",
                columns: new[] { "EtablissementId", "Matricule" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduEnseignantSpecialites_EnseignantId",
                table: "EduEnseignantSpecialites",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduEpreuves_EnseignantSurveillantId",
                table: "EduEpreuves",
                column: "EnseignantSurveillantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduEpreuves_MatiereId",
                table: "EduEpreuves",
                column: "MatiereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduEpreuves_SalleId",
                table: "EduEpreuves",
                column: "SalleId");

            migrationBuilder.CreateIndex(
                name: "IX_EduEpreuves_SessionId",
                table: "EduEpreuves",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_EduEvaluations_EnseignantId",
                table: "EduEvaluations",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduEvaluations_MatiereId",
                table: "EduEvaluations",
                column: "MatiereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduFilieres_CycleId",
                table: "EduFilieres",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_EduGroupes_PromotionId",
                table: "EduGroupes",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_EduIndisponibilites_EnseignantId",
                table: "EduIndisponibilites",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptionGroupes_GroupeId",
                table: "EduInscriptionGroupes",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptionHistoriques_InscriptionId",
                table: "EduInscriptionHistoriques",
                column: "InscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptions_AnneeAcademiqueId",
                table: "EduInscriptions",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptions_ApprenantId",
                table: "EduInscriptions",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptions_ClasseId",
                table: "EduInscriptions",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptions_NumeroInscription",
                table: "EduInscriptions",
                column: "NumeroInscription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptions_PromotionId",
                table: "EduInscriptions",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptions_ReinscriptionDepuisId",
                table: "EduInscriptions",
                column: "ReinscriptionDepuisId");

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptionUEs_InscriptionId_UeId",
                table: "EduInscriptionUEs",
                columns: new[] { "InscriptionId", "UeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduInscriptionUEs_UeId",
                table: "EduInscriptionUEs",
                column: "UeId");

            migrationBuilder.CreateIndex(
                name: "IX_EduJustificatifs_AbsenceId",
                table: "EduJustificatifs",
                column: "AbsenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduListesAttente_InscriptionId",
                table: "EduListesAttente",
                column: "InscriptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduMatieres_FiliereId",
                table: "EduMatieres",
                column: "FiliereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduMatieres_UeId",
                table: "EduMatieres",
                column: "UeId");

            migrationBuilder.CreateIndex(
                name: "IX_EduMoyennesGenerales_ApprenantId_AnneeAcademiqueId_PeriodeId",
                table: "EduMoyennesGenerales",
                columns: new[] { "ApprenantId", "AnneeAcademiqueId", "PeriodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduMoyennesMatieres_ApprenantId",
                table: "EduMoyennesMatieres",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduMoyennesMatieres_MatiereId",
                table: "EduMoyennesMatieres",
                column: "MatiereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduNiveaux_FiliereId",
                table: "EduNiveaux",
                column: "FiliereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduNotes_ApprenantId",
                table: "EduNotes",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduNotes_EvaluationId_ApprenantId",
                table: "EduNotes",
                columns: new[] { "EvaluationId", "ApprenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduNotesHistorique_NoteId",
                table: "EduNotesHistorique",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_EduParametresAbsenteisme_EtablissementId",
                table: "EduParametresAbsenteisme",
                column: "EtablissementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduPeriodes_AnneeAcademiqueId",
                table: "EduPeriodes",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_EduPiecesJustificatives_ApprenantId",
                table: "EduPiecesJustificatives",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduPresences_ApprenantId",
                table: "EduPresences",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduPresences_SeanceId_ApprenantId",
                table: "EduPresences",
                columns: new[] { "SeanceId", "ApprenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduPromotions_NiveauId",
                table: "EduPromotions",
                column: "NiveauId");

            migrationBuilder.CreateIndex(
                name: "IX_EduPVExamens_EpreuveId",
                table: "EduPVExamens",
                column: "EpreuveId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduSalleEquipements_SalleId",
                table: "EduSalleEquipements",
                column: "SalleId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSalles_CampusId",
                table: "EduSalles",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_ClasseId",
                table: "EduSeances",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_CoursPlanifieId",
                table: "EduSeances",
                column: "CoursPlanifieId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_EnseignantId",
                table: "EduSeances",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_EnseignantRemplacantId",
                table: "EduSeances",
                column: "EnseignantRemplacantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_EtablissementId_Date_EnseignantId",
                table: "EduSeances",
                columns: new[] { "EtablissementId", "Date", "EnseignantId" });

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_EtablissementId_Date_SalleId",
                table: "EduSeances",
                columns: new[] { "EtablissementId", "Date", "SalleId" });

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_GroupeId",
                table: "EduSeances",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_MatiereId",
                table: "EduSeances",
                column: "MatiereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_PromotionId",
                table: "EduSeances",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_EduSeances_SalleId",
                table: "EduSeances",
                column: "SalleId");

            migrationBuilder.CreateIndex(
                name: "IX_EduTuteurs_ApprenantId",
                table: "EduTuteurs",
                column: "ApprenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EduUniteEnseignements_FiliereId",
                table: "EduUniteEnseignements",
                column: "FiliereId");

            migrationBuilder.CreateIndex(
                name: "IX_EduUniteEnseignements_NiveauId",
                table: "EduUniteEnseignements",
                column: "NiveauId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EduAbsencesEnseignants");

            migrationBuilder.DropTable(
                name: "EduAffectationsMatieres");

            migrationBuilder.DropTable(
                name: "EduBulletinLignes");

            migrationBuilder.DropTable(
                name: "EduCasFraude");

            migrationBuilder.DropTable(
                name: "EduConvocations");

            migrationBuilder.DropTable(
                name: "EduCreneauxHoraires");

            migrationBuilder.DropTable(
                name: "EduDeliberationLignes");

            migrationBuilder.DropTable(
                name: "EduDeliberationMembres");

            migrationBuilder.DropTable(
                name: "EduEnseignantSpecialites");

            migrationBuilder.DropTable(
                name: "EduEvenementsCalendrier");

            migrationBuilder.DropTable(
                name: "EduIndisponibilites");

            migrationBuilder.DropTable(
                name: "EduInscriptionGroupes");

            migrationBuilder.DropTable(
                name: "EduInscriptionHistoriques");

            migrationBuilder.DropTable(
                name: "EduInscriptionUEs");

            migrationBuilder.DropTable(
                name: "EduJustificatifs");

            migrationBuilder.DropTable(
                name: "EduListesAttente");

            migrationBuilder.DropTable(
                name: "EduModelesMessage");

            migrationBuilder.DropTable(
                name: "EduMoyennesGenerales");

            migrationBuilder.DropTable(
                name: "EduMoyennesMatieres");

            migrationBuilder.DropTable(
                name: "EduNotesHistorique");

            migrationBuilder.DropTable(
                name: "EduParametresAbsenteisme");

            migrationBuilder.DropTable(
                name: "EduPeriodesInscription");

            migrationBuilder.DropTable(
                name: "EduPiecesJustificatives");

            migrationBuilder.DropTable(
                name: "EduPresences");

            migrationBuilder.DropTable(
                name: "EduSalleEquipements");

            migrationBuilder.DropTable(
                name: "EduTuteurs");

            migrationBuilder.DropTable(
                name: "EduBulletins");

            migrationBuilder.DropTable(
                name: "EduPVExamens");

            migrationBuilder.DropTable(
                name: "EduDeliberations");

            migrationBuilder.DropTable(
                name: "EduAbsences");

            migrationBuilder.DropTable(
                name: "EduInscriptions");

            migrationBuilder.DropTable(
                name: "EduNotes");

            migrationBuilder.DropTable(
                name: "EduPeriodes");

            migrationBuilder.DropTable(
                name: "EduEpreuves");

            migrationBuilder.DropTable(
                name: "EduSeances");

            migrationBuilder.DropTable(
                name: "EduApprenants");

            migrationBuilder.DropTable(
                name: "EduEvaluations");

            migrationBuilder.DropTable(
                name: "EduAnneesAcademiques");

            migrationBuilder.DropTable(
                name: "EduSessionsExamen");

            migrationBuilder.DropTable(
                name: "EduClasses");

            migrationBuilder.DropTable(
                name: "EduCoursPlanifies");

            migrationBuilder.DropTable(
                name: "EduGroupes");

            migrationBuilder.DropTable(
                name: "EduEnseignants");

            migrationBuilder.DropTable(
                name: "EduMatieres");

            migrationBuilder.DropTable(
                name: "EduSalles");

            migrationBuilder.DropTable(
                name: "EduPromotions");

            migrationBuilder.DropTable(
                name: "EduUniteEnseignements");

            migrationBuilder.DropTable(
                name: "EduCampus");

            migrationBuilder.DropTable(
                name: "EduNiveaux");

            migrationBuilder.DropTable(
                name: "EduFilieres");

            migrationBuilder.DropTable(
                name: "EduCycles");
        }
    }
}
