using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Toolchain {
    public static class StringHelpers {
        public static string RemoveWhitespace(this string textString) {
            return Regex.Replace(textString, @"\s+", " ");
        }
    }
}