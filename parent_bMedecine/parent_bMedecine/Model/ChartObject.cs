using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.Model
{
    /// <summary>
    /// Class to represent chart data for 'MetroChart' library
    /// <see cref="http://modernuicharts.codeplex.com/"/>
    /// </summary>
    public class ChartObject
    {
        /// <summary>
        /// Data category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Data value as int
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Data value as double
        /// </summary>
        public double NumberDouble { get; set; }
    }
}
