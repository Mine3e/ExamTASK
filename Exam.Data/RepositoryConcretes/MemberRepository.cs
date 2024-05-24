using Exam.Core.Models;
using Exam.Core.RepositoryAbstracts;
using Exam.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data.RepositoryConcretes
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
