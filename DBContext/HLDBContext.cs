namespace DBTest
{
    using SQLite.CodeFirst;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.SQLite;
    using System.Linq;

    public class HLDBContext : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DBTest.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public HLDBContext()
            : base("name=HLDBContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Data> Data { get; set; }
        public virtual DbSet<Output> Outputs { get; set; }
        public virtual DbSet<Simulation> Simulations { get; set; }
        public virtual DbSet<Model> Models { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<HLDBContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        public HLDBContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configure();
        }

        public HLDBContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }
    }
    public interface IEntity
    {
        [Autoincrement]
        int Id { get; set; }
    }

    public class Data : IEntity
    {
        [Autoincrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public double Value { get; set; }

        public int SimulationId { get; set; }
        public virtual Simulation Simulation { get; set; }

        public int OutputId { get; set; }
        public Output Output { get; set; }
    }

    public class Output : IEntity
    {
        [Autoincrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Unit { get; set; }
    }

    public class Model : IEntity
    {
        [Autoincrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class Simulation : IEntity
    {
        [Autoincrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<Model> Models { get; set; }

        public virtual List<Data> Data { get; set; }
    }
}