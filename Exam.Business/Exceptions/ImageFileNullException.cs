using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Business.Exceptions
{
    public class ImageFileNullException : Exception
    {
        public string PropertyName { get; set; }
        public ImageFileNullException(string propertyname,string? message) : base(message)
        {
            PropertyName = propertyname;
        }
    }
}
