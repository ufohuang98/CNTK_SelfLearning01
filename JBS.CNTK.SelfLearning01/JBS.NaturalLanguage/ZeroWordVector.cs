using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.NaturalLanguage.Models;

namespace JBS.NaturalLanguage
{
    /// <summary>
    /// 量を持たないベクトル値。
    /// </summary>
    public class ZeroWordVector :IWordVector
    {
        private int dimensionSize;

        public ZeroWordVector(int dimensionSize)
        {
            this.dimensionSize = dimensionSize;
        }

        public bool HasValue => false;

        public float[] ToNumericValue()
        {
            return new float[dimensionSize];
        }

        public string ToSparseString()
        {
            return string.Empty;
        }
    }
}
