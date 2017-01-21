using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelGenerator
{
  public static class PathFunctions
  {
    public static bool IsSupportedPath(string path)
    {
      if (path == null) throw new ArgumentNullException(nameof(path));
      return !Path.IsPathRooted(path) || Path.IsPathRooted(path) && Path.GetFullPath(path) == path;
    }

    public static bool IsPathRelative(string path)
    {
      if (path == null) throw new ArgumentNullException(nameof(path));
      return !Path.IsPathRooted(path);
    }
  }
}
