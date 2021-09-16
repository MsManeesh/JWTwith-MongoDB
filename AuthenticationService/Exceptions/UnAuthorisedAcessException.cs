using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Exceptions
{
    public class UnAuthorisedAcessException:Exception
    {
        public UnAuthorisedAcessException(string message):base(message)
        {

        }
    }
}
