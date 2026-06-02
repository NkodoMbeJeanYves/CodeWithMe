using System.Reflection;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Edu;
using CodeWithMe.Core.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Core
{
    public sealed class ApiContext : IdentityDbContext<User>
    {
        private readonly ITenantContext _tenantContext;

        public DbSet<Tenant> Tenants { get; set; } = null!;
        public DbSet<Period> Periods { get; set; } = null!;
        public DbSet<School> Schools { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<ProgramModel> Programs { get; set; } = null!;
        public DbSet<RevokedToken> RevokedTokens { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;

        // ---- Domaine academic ----
        public DbSet<Filiere> Filieres { get; set; } = null!;
        public DbSet<Classe> Classes { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<CourseSession> CourseSessions { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Timetable> Timetables { get; set; } = null!;
        public DbSet<TimetableEntry> TimetableEntries { get; set; } = null!;

        // ---- Domaine admin ----
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        // ---- Domaine finance ----
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<PayrollPeriod> PayrollPeriods { get; set; } = null!;
        public DbSet<PayrollRun> PayrollRuns { get; set; } = null!;

        // ---- Domaine communication ----
        public DbSet<Announcement> Announcements { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        // ====== Domaine Edu (architecture edu_manager) ======

        // Tenant / Établissement
        public DbSet<Campus> EduCampus { get; set; } = null!;
        public DbSet<AnneeAcademique> EduAnneesAcademiques { get; set; } = null!;
        public DbSet<PeriodeEdu> EduPeriodes { get; set; } = null!;
        public DbSet<EvenementCalendrier> EduEvenementsCalendrier { get; set; } = null!;
        public DbSet<Salle> EduSalles { get; set; } = null!;
        public DbSet<SalleEquipement> EduSalleEquipements { get; set; } = null!;
        public DbSet<CreneauHoraire> EduCreneauxHoraires { get; set; } = null!;

        // Structure académique
        public DbSet<Cycle> EduCycles { get; set; } = null!;
        public DbSet<FiliereEdu> EduFilieres { get; set; } = null!;
        public DbSet<Niveau> EduNiveaux { get; set; } = null!;
        public DbSet<ClasseEdu> EduClasses { get; set; } = null!;
        public DbSet<Promotion> EduPromotions { get; set; } = null!;
        public DbSet<Groupe> EduGroupes { get; set; } = null!;

        // Référentiel pédagogique
        public DbSet<Matiere> EduMatieres { get; set; } = null!;
        public DbSet<UniteEnseignement> EduUniteEnseignements { get; set; } = null!;

        // Apprenants
        public DbSet<Apprenant> EduApprenants { get; set; } = null!;
        public DbSet<Tuteur> EduTuteurs { get; set; } = null!;
        public DbSet<PieceJustificative> EduPiecesJustificatives { get; set; } = null!;

        // Enseignants
        public DbSet<Enseignant> EduEnseignants { get; set; } = null!;
        public DbSet<EnseignantSpecialite> EduEnseignantSpecialites { get; set; } = null!;
        public DbSet<AffectationMatiere> EduAffectationsMatieres { get; set; } = null!;
        public DbSet<Indisponibilite> EduIndisponibilites { get; set; } = null!;

        // Inscriptions
        public DbSet<Inscription> EduInscriptions { get; set; } = null!;
        public DbSet<InscriptionGroupe> EduInscriptionGroupes { get; set; } = null!;
        public DbSet<InscriptionUE> EduInscriptionUEs { get; set; } = null!;
        public DbSet<PeriodeInscription> EduPeriodesInscription { get; set; } = null!;
        public DbSet<ListeAttente> EduListesAttente { get; set; } = null!;
        public DbSet<InscriptionHistorique> EduInscriptionHistoriques { get; set; } = null!;

        // EDT
        public DbSet<CoursPlanifie> EduCoursPlanifies { get; set; } = null!;
        public DbSet<Seance> EduSeances { get; set; } = null!;

        // Notes
        public DbSet<Evaluation> EduEvaluations { get; set; } = null!;
        public DbSet<NoteEdu> EduNotes { get; set; } = null!;
        public DbSet<NoteHistorique> EduNotesHistorique { get; set; } = null!;
        public DbSet<MoyenneMatiere> EduMoyennesMatieres { get; set; } = null!;
        public DbSet<MoyenneGenerale> EduMoyennesGenerales { get; set; } = null!;

        // Absences
        public DbSet<Presence> EduPresences { get; set; } = null!;
        public DbSet<Absence> EduAbsences { get; set; } = null!;
        public DbSet<Justificatif> EduJustificatifs { get; set; } = null!;
        public DbSet<AbsenceEnseignant> EduAbsencesEnseignants { get; set; } = null!;
        public DbSet<ParametresAbsenteisme> EduParametresAbsenteisme { get; set; } = null!;

        // Bulletins / Délibérations
        public DbSet<Bulletin> EduBulletins { get; set; } = null!;
        public DbSet<BulletinLigne> EduBulletinLignes { get; set; } = null!;
        public DbSet<Deliberation> EduDeliberations { get; set; } = null!;
        public DbSet<DeliberationMembre> EduDeliberationMembres { get; set; } = null!;
        public DbSet<DeliberationLigne> EduDeliberationLignes { get; set; } = null!;

        // Examens
        public DbSet<SessionExamen> EduSessionsExamen { get; set; } = null!;
        public DbSet<Epreuve> EduEpreuves { get; set; } = null!;
        public DbSet<Convocation> EduConvocations { get; set; } = null!;
        public DbSet<PVExamen> EduPVExamens { get; set; } = null!;
        public DbSet<CasFraude> EduCasFraude { get; set; } = null!;

        // Communication
        public DbSet<ModeleMessage> EduModelesMessage { get; set; } = null!;

        public ApiContext(DbContextOptions<ApiContext> options, ITenantContext tenantContext)
            : base(options)
        {
            _tenantContext = tenantContext;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<HasTimestamps>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Modified))
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            // Force le tenant courant sur tout INSERT d'entité ITenantScoped.
            foreach (var entry in ChangeTracker.Entries<ITenantScoped>())
            {
                if (entry.State == EntityState.Added && string.IsNullOrEmpty(entry.Entity.TenantId))
                {
                    entry.Entity.TenantId = _tenantContext.CurrentTenantId ?? Tenant.DefaultTenantId;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all Fluent API configurations from this assembly (Edu domain)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiContext).Assembly);

            // ---- Filtre global multi-tenant ----
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ITenantScoped).IsAssignableFrom(entityType.ClrType))
                {
                    SetTenantFilterMethod
                        .MakeGenericMethod(entityType.ClrType)
                        .Invoke(this, new object[] { modelBuilder });
                }
            }

            // ---- Existing configuration ----
            modelBuilder.Entity<School>()
                .HasMany(s => s.Periods);

            modelBuilder.Entity<School>()
                .Property(p => p.SchoolType)
                .HasConversion<string>();

            modelBuilder.Entity<Period>()
                .Property(p => p.PeriodType)
                .HasConversion<string>();

            // ---- Tenant config ----
            modelBuilder.Entity<Tenant>()
                .HasIndex(t => t.Code).IsUnique();
            modelBuilder.Entity<Tenant>().Property(t => t.Type).HasConversion<string>();
            modelBuilder.Entity<Tenant>().Property(t => t.Status).HasConversion<string>();

            // Seed tenant par défaut — utilisé pour le backfill des entités existantes
            // (School, Period, Subject, Program) lors de la migration AddMultiTenant.
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Tenant>().HasData(new Tenant
            {
                Id = Tenant.DefaultTenantId,
                Code = Tenant.DefaultTenantCode,
                Name = "Default Tenant",
                Type = Models.Enums.TenantType.School,
                Status = Models.Enums.TenantStatus.Active,
                Locale = "fr-FR",
                Timezone = "Europe/Paris",
                AcademicYear = null,
                SettingsJson = null,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            });

            // ---- User config (enum + identité user étendue) ----
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<User>().HasIndex(u => u.TenantId);

            // ---- Academic enums ----
            modelBuilder.Entity<Student>().Property(s => s.Status).HasConversion<string>();
            modelBuilder.Entity<Student>().Property(s => s.Gender).HasConversion<string>();
            modelBuilder.Entity<Student>().HasIndex(s => new { s.TenantId, s.Matricule }).IsUnique();
            modelBuilder.Entity<Teacher>().Property(t => t.Status).HasConversion<string>();
            modelBuilder.Entity<Teacher>().Property(t => t.ContractType).HasConversion<string>();
            modelBuilder.Entity<Teacher>().HasIndex(t => new { t.TenantId, t.Matricule }).IsUnique();
            modelBuilder.Entity<CourseSession>().Property(s => s.Type).HasConversion<string>();
            modelBuilder.Entity<CourseSession>().Property(s => s.Status).HasConversion<string>();
            modelBuilder.Entity<CourseSession>().HasIndex(s => new { s.TenantId, s.StartAt });
            modelBuilder.Entity<Attendance>().Property(a => a.Status).HasConversion<string>();
            modelBuilder.Entity<Attendance>().HasIndex(a => new { a.TenantId, a.SessionId, a.StudentId }).IsUnique();
            modelBuilder.Entity<Grade>().Property(g => g.Type).HasConversion<string>();
            modelBuilder.Entity<Grade>().Property(g => g.Period).HasConversion<string>();
            modelBuilder.Entity<Grade>().HasIndex(g => new { g.TenantId, g.StudentId, g.SubjectId, g.Period });
            modelBuilder.Entity<Timetable>().Property(t => t.OwnerType).HasConversion<string>();
            modelBuilder.Entity<Timetable>()
                .HasMany(t => t.Entries)
                .WithOne()
                .HasForeignKey(e => e.TimetableId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static readonly MethodInfo SetTenantFilterMethod = typeof(ApiContext)
            .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)!;

        private void SetTenantFilter<TEntity>(ModelBuilder builder) where TEntity : class, ITenantScoped
        {
            builder.Entity<TEntity>()
                .HasQueryFilter(e =>
                    _tenantContext.CurrentTenantId == null
                    || e.TenantId == _tenantContext.CurrentTenantId);
        }
    }

    public interface IHasTimestamps
    {
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
