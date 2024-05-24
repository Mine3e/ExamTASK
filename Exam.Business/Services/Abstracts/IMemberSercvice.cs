using Exam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Business.Services.Abstracts
{
    public  interface IMemberSercvice
    {
        void AddMember(Member member);
        void DeleteMember(int id);
        void UpdateMember(int id, Member member);
        Member GetMember(Func<Member,bool>? func=null);
        List<Member> GetAllMember(Func<Member,bool>? func=null);
    }
}
