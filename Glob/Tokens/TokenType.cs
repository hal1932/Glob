using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Tokens
{
    enum TokenType
    {
        None,
        Literal,
        AnyCharacter,
        BeginRange,
        EndRange,
        DirectorySeparator,
        Wildcard,
        DirectoryWildcard,
    }
}
