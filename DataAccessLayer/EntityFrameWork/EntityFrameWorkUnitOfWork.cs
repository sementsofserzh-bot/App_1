using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.EntityFrameWork
{
    public sealed class EntityFrameWorkUnitOfWork : Contracts.IUnitOfWork
    {
        private readonly DBContext _context;

        public Contracts.IRepository<App_Model.Trainer> Trainers { get; }
        public Contracts.IRepository<App_Model.Athlete> Athletes { get; }

        public EntityFrameWorkUnitOfWork(string? connectionString = null)
        {
            _context = new DBContext(connectionString);
            Trainers = new EntityRepository<App_Model.Trainer>(_context);
            Athletes = new EntityRepository<App_Model.Athlete>(_context);
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}
