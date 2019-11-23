using DigitalGreen.Core.DataBase.DigitalGreenDB;   
using DigitalGreen.Core.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalGreen.Core
{
    public class UnitOfWork : IDisposable
    {

        #region Private member variables...
        private readonly DataBase.DigitalGreenDB.DGEntities _context;
        private DbContextTransaction Transaction { get; set; }
        private Dictionary<string, object> repositories;
        private GenericRepository<Farmer> _farmerRepository;
        private GenericRepository<FarmerImage> _farmerImagesRepository;
        private GenericRepository<Client> _clientRepository;
        #endregion


        public UnitOfWork()
        {
            _context = new DataBase.DigitalGreenDB.DGEntities();
        }

        public UnitOfWork BeginTransaction()
        {
            Transaction = _context.Database.BeginTransaction();
            return this;
        }

        #region Public Repository Creation properties...
        /// <summary>
        /// Get/Set Property for Farmer repository.
        /// </summary>
        public GenericRepository<Farmer> FarmerRepository
        {
            get
            {
                if (this._farmerRepository == null)
                    this._farmerRepository = new GenericRepository<Farmer>(_context);
                return _farmerRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Farmer Images's repository.
        /// </summary>
        public GenericRepository<FarmerImage> FarmerImagesRepository
        {
            get
            {
                if (this._farmerImagesRepository == null)
                    this._farmerImagesRepository = new GenericRepository<FarmerImage>(_context);
                return _farmerImagesRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Client repository.
        /// </summary>
        public GenericRepository<Client> ClientRepository
        {
            get
            {
                if (this._clientRepository == null)
                    this._clientRepository = new GenericRepository<Client>(_context);
                return _clientRepository;
            }
        }
        #endregion


        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                //   System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }
        public bool EndTransaction()
        {
            try
            {
                _context.SaveChanges();
                Transaction.Commit();
            }
            catch (DbEntityValidationException dbEx)
            {
                Transaction.Rollback();
                Console.WriteLine(dbEx.Message);
                throw new Exception() { };
                // add your exception handling code here
            }
            return true;
        }

        public void RollBack()
        {
            Transaction.Rollback();
            Dispose();
        }
        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        //when we need to create separte repository for every table
        public GenericRepository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<T>)repositories[type];
        }



    }
}
