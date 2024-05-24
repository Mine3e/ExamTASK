using Exam.Business.Exceptions;
using Exam.Business.Services.Abstracts;
using Exam.Core.Models;
using Exam.Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Business.Services.Concretes
{
    public class MemberService : IMemberSercvice
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MemberService(IMemberRepository memberRepository, IWebHostEnvironment webHostEnvironment)
        {
            _memberRepository = memberRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddMember(Member member)
        {
            if (member.ImageFile == null) throw new ImageFileNullException("ImageFile", "ImageFile null ola bilmez");
            if (member.ImageFile.Length > 2097152) throw new FileSizeException("ImageFile", "Size error");
            if (!member.ImageFile.ContentType.Contains("image/")) throw new FileContentTypeException("ImageFile", "Content type error");
            string path = _webHostEnvironment.WebRootPath + @"\Upload\Member\" + member.ImageFile.FileName;
            using(FileStream stream =new FileStream(path, FileMode.Create))
            {
                member.ImageFile.CopyTo(stream);
            }
            member.ImageUrl = member.ImageFile.FileName;
            _memberRepository.Add(member);
            _memberRepository.Commit();
        }

        public void DeleteMember(int id)
        {
            var existmember = _memberRepository.Get(x => x.Id == id);
            if (existmember == null) throw new EntityNotFondException("", "Entity not found");
            string path = _webHostEnvironment.WebRootPath + @"\Upload\Member\" + existmember.ImageUrl;
            if (!File.Exists(path)) throw new ImageFileNotFoundException("", "File not found");
            File.Delete(path);
            _memberRepository.Delete(existmember);
            _memberRepository.Commit();
        }

        public List<Member> GetAllMember(Func<Member, bool>? func = null)
        {
            return _memberRepository.GetAll(func);
        }

        public Member GetMember(Func<Member, bool>? func = null)
        {
            return _memberRepository.Get(func);
        }

        public void UpdateMember(int id, Member member)
        {
            var existmember = _memberRepository.Get(x => x.Id == id);
            if (existmember == null) throw new EntityNotFondException("", "Entity not found");
            if (member.ImageFile != null)
            {
                if (member.ImageFile.Length > 2097152) throw new FileSizeException("ImageFile", "Size error");
                if (!member.ImageFile.ContentType.Contains("image/")) throw new FileContentTypeException("ImageFile", "Content type error");
                string path = _webHostEnvironment.WebRootPath + @"\Upload\Member\" + member.ImageFile.FileName;
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    member.ImageFile.CopyTo(stream);
                }
                string path1 = _webHostEnvironment.WebRootPath + @"\Upload\Member\" + existmember.ImageUrl;
                if (!File.Exists(path1)) throw new ImageFileNotFoundException("", "File not found");
                File.Delete(path1);
                existmember.ImageUrl = member.ImageFile.FileName;
            }
            existmember.Name=member.Name;
            existmember.Title=member.Title;
            _memberRepository.Commit();
        }
    }
}
