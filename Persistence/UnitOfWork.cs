using MwProject.Core;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        // readonly przy polu oznacza, że jego wartość
        // możemy zmienić tylko w konstruktorze
        private readonly IApplicationDbContext _context;
        public UnitOfWork(IApplicationDbContext context, IProjectRepository project, ICategoryRepository category)
        {
            _context = context;
            Project = project;      // new AdvertRepository(context);
            Category = category;    // new CategoryRepository(context);
        }

        // obiekty repozytoryjne 
        public IProjectRepository Project { get; set; }
        public ICategoryRepository Category { get; set; }

        // na koniec metoda zapisująca zmiany
        public void Complete()
        {
            _context.SaveChanges();
        }

    }
}
