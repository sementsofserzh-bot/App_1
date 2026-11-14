using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public interface IView
    {
        void ShowMessage(string message);
        void ShowError(string message);
    }
}
