using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ColorFilter
    {
        public static float[] IDENTITY = new float[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0 };

        private static float[] tmp = new float[20];

        private const float LUMA_R = 0.299f;
        private const float LUMA_G = 0.587f;
        private const float LUMA_B = 0.114f;

        public float[] _matrix;
        public Matrix4x4 _shaderMatrix;
        public Vector4 _offset;

        public ColorFilter(float[] matrix = null)
        {
            if (matrix != null)
                this.matrix = matrix;
            else
            {
                _matrix = new float[20];
                Array.Copy(IDENTITY, _matrix, _matrix.Length);
            }
        }

        public float[] matrix
        {
            get { return _matrix; }
            set
            {
                _matrix = value;
                UpdateMatrix();
            }
        }

        public Matrix4x4 shaderMatrix
        {
            get { return _shaderMatrix; }
        }

        public Vector4 offset
        {
            get { return _offset; }
        }

        /// <summary>
		/// Changes the saturation. Typical values are in the range (-1, 1).
		/// Values above zero will raise, values below zero will reduce the saturation.
		/// '-1' will produce a grayscale image. 
		/// </summary>
		/// <param name="sat"></param>
		public void AdjustSaturation(float sat)
        {
            sat += 1;

            float invSat = 1 - sat;
            float invLumR = invSat * LUMA_R;
            float invLumG = invSat * LUMA_G;
            float invLumB = invSat * LUMA_B;

            ConcatValues((invLumR + sat), invLumG, invLumB, 0, 0,
                          invLumR, (invLumG + sat), invLumB, 0, 0,
                          invLumR, invLumG, (invLumB + sat), 0, 0,
                          0, 0, 0, 1, 0);
        }

        /// <summary>
        /// Changes the contrast. Typical values are in the range (-1, 1).
        /// Values above zero will raise, values below zero will reduce the contrast.
        /// </summary>
        /// <param name="value"></param>
        public void AdjustContrast(float value)
        {
            float s = value + 1;
            float o = 128f / 255 * (1 - s);

            ConcatValues(s, 0, 0, 0, o,
                         0, s, 0, 0, o,
                         0, 0, s, 0, o,
                         0, 0, 0, 1, 0);
        }

        /// <summary>
        /// Changes the brightness. Typical values are in the range (-1, 1).
        /// Values above zero will make the image brighter, values below zero will make it darker.
        /// </summary>
        /// <param name="value"></param>
        public void AdjustBrightness(float value)
        {
            ConcatValues(1, 0, 0, 0, value,
                         0, 1, 0, 0, value,
                         0, 0, 1, 0, value,
                         0, 0, 0, 1, 0);
        }

        /// <summary>
        ///Changes the hue of the image. Typical values are in the range (-1, 1).
        /// </summary>
        /// <param name="value"></param>
        public void AdjustHue(float value)
        {
            value *= Mathf.PI;

            float cos = Mathf.Cos(value);
            float sin = Mathf.Sin(value);

            ConcatValues(
                ((LUMA_R + (cos * (1 - LUMA_R))) + (sin * -(LUMA_R))), ((LUMA_G + (cos * -(LUMA_G))) + (sin * -(LUMA_G))), ((LUMA_B + (cos * -(LUMA_B))) + (sin * (1 - LUMA_B))), 0, 0,
                ((LUMA_R + (cos * -(LUMA_R))) + (sin * 0.143f)), ((LUMA_G + (cos * (1 - LUMA_G))) + (sin * 0.14f)), ((LUMA_B + (cos * -(LUMA_B))) + (sin * -0.283f)), 0, 0,
                ((LUMA_R + (cos * -(LUMA_R))) + (sin * -((1 - LUMA_R)))), ((LUMA_G + (cos * -(LUMA_G))) + (sin * LUMA_G)), ((LUMA_B + (cos * (1 - LUMA_B))) + (sin * LUMA_B)), 0, 0,
                0, 0, 0, 1, 0);
        }

        public void Reset()
        {
            Array.Copy(IDENTITY, _matrix, _matrix.Length);

            UpdateMatrix();
        }

        public void ConcatValues(params float[] values)
        {
            int i = 0;

            for (int y = 0; y < 4; ++y)
            {
                for (int x = 0; x < 5; ++x)
                {
                    tmp[i + x] = values[i] * _matrix[x] +
                            values[i + 1] * _matrix[x + 5] +
                            values[i + 2] * _matrix[x + 10] +
                            values[i + 3] * _matrix[x + 15] +
                            (x == 4 ? values[i + 4] : 0);
                }
                i += 5;
            }
            Array.Copy(tmp, _matrix, tmp.Length);

            UpdateMatrix();
        }

        public void ConcatValues(ColorFilter filter)
        {
            ConcatValues(filter._matrix);
        }

        public void SetValues(params float[] values)
        {
            Array.Copy(values, _matrix, values.Length);
            UpdateMatrix();
        }

        public void SetValues(ColorFilter filter)
        {
            Array.Copy(filter._matrix, _matrix, filter._matrix.Length);
            UpdateMatrix();
        }

        protected void UpdateMatrix()
        {
            _shaderMatrix.SetRow(0, new Vector4(_matrix[0], _matrix[1], _matrix[2], _matrix[3]));
            _shaderMatrix.SetRow(1, new Vector4(_matrix[5], _matrix[6], _matrix[7], _matrix[8]));
            _shaderMatrix.SetRow(2, new Vector4(_matrix[10], _matrix[11], _matrix[12], _matrix[13]));
            _shaderMatrix.SetRow(3, new Vector4(_matrix[15], _matrix[16], _matrix[17], _matrix[18]));
            _offset = new Vector4(_matrix[4], _matrix[9], _matrix[14], _matrix[19]);
        }

        public void Apply(Material material)
        {
            material.SetInt("_ApplyColorFilter", 1);
            material.SetMatrix("_ColorMatrix", _shaderMatrix);
            material.SetVector("_ColorOffset", _offset);
        }
    }
}
