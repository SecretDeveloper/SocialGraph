using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGraph.Extract
{
    interface IExtracter<T>
    {
        T Extract(string body);
    }
}
