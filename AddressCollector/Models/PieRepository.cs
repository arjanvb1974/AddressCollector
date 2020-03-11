using System;
using System.Collections.Generic;
using System.Linq;
using AddressCollector.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace AddressCollector.Models
{
    public class PieRepository: IPieRepository
    {
        private readonly AppDbContext _appDbContext;

        public PieRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Pie> AllPies
        {
            get
            {
                return null; //_appDbContext.Pies.Include(c => c.Category);
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return null; //_appDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public Pie GetPieById(int pieId)
        {
            return null; //_appDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);
        }
    }
}
