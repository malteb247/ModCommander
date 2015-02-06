using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModCommander.Service
{
    interface IErrorService
    {
        void HandleException(Exception ex);
    }
}
