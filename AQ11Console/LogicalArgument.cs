using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public interface LogicalArgument
    {
        string toString();
        bool isEqual(LogicalArgument argument);
    }
}
