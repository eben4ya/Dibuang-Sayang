using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appview.ViewModel
{
    public static class UserSession
    {
        private static int _userId;
        public static int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
    }
}
