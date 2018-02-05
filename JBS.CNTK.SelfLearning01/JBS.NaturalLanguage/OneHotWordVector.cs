using JBS.NaturalLanguage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.NaturalLanguage
{
    /// <summary>
    /// one-hotベクトル表現の単語ベクトル。
    /// </summary>
    public class OneHotWordVector:IWordVector
    {
        private int dimensionSize;

        /// <summary>
        /// このベクトルが対応する次元。
        /// </summary>
        public int HotDimension { get; private set; }

        public OneHotWordVector(int dimensionSize, int assignedDimension)
        {
            if (dimensionSize <= assignedDimension) throw new ArgumentException($"illegal dimension value. {nameof(dimensionSize)}={dimensionSize} {nameof(assignedDimension)}={assignedDimension}");
            this.dimensionSize = dimensionSize;
            this.HotDimension = assignedDimension;
        }

        public bool HasValue => true;

        public float[] ToNumericValue()
        {
            var val = new float[dimensionSize];
            val[this.HotDimension] = 1;
            return val;
        }

        public string ToSparseString()
        {
            return $"{this.HotDimension}:1";
        }
    }
}
