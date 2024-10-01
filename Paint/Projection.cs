using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    internal class Projection
    {
        private float[,] _figure, _figure3D;
        private readonly int[,] _adjacent;
        private readonly float[,] _reset, _reset3D;
        private readonly PictureBox _canvas;

        // Матрица перспективного преобразования
        private readonly float[,] _perspective =
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },  // Чем меньше коэффициент, тем сильнее эффект перспективы
            { 0, 0, -0.002f, 1 }
        };

        // Уменьшение
        private readonly float[,] _scaleDown =
        {
            { 0.5f, 0, 0, 0 },
            { 0, 0.5f, 0, 0 },
            { 0, 0, 0.5f, 0 },
            { 0, 0, 0, 1 }
        };

        // Увеличение
        private readonly float[,] _scaleUp =
        {
            { 2, 0, 0, 0 },
            { 0, 2, 0, 0 },
            { 0, 0, 2, 0 },
            { 0, 0, 0, 1 }
        };

        // Отражение по оси Z
        private readonly float[,] _reflect =
        {
            { 1, 0, 0, 0 },
            { 0, -1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        public Projection(float[,] figure, float[,] figure3D, int[,] adjacent, PictureBox canvas)
        {
            _canvas = canvas;
            _figure = figure;
            _figure3D = figure3D;
            _figure = Multiplication(_figure, _reflect);
            _figure3D = Multiplication(_figure3D, _reflect);
            _reset = figure;
            _reset3D = figure3D;
            _adjacent = adjacent;
        }

        /// <summary>
        /// Применение перспективного преобразования
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private float[,] ApplyPerspective(float[,] position)
        {
            var temp = Utils.Copy(position);

            for (var i = 0; i < position.Length / 4; i++)
            {
                float z = temp[i, 2];
                float w = 1 + z * _perspective[3, 2];  // Это модифицированная координата W

                temp[i, 0] /= w;
                temp[i, 1] /= w;
                temp[i, 2] /= w;
            }

            return temp;
        }

        /// <summary>
        /// Перевод из мировых в экранные координаты.
        /// </summary>
        /// <param name="position"></param>
        private float[,] WorldToScreen(float[,] position)
        {
            var temp = Utils.Copy(position);
            var screenCenterX = (float)_canvas.ClientSize.Width / 2;
            var screenCenterY = (float)_canvas.ClientSize.Height / 2;

            var scale = 20f;

            for (var i = 0; i < position.Length / 4; i++)
            {
                temp[i, 0] = screenCenterX + temp[i, 0] * scale;
                temp[i, 1] = screenCenterY + temp[i, 1] * scale;
            }

            return temp;
        }

        /// <summary>
        /// Отрисовка фигуры.
        /// </summary>
        public void DrawFigure()
        {
            var temp = ApplyPerspective(_figure);
            var temp3D = ApplyPerspective(_figure3D);
            temp = WorldToScreen(temp);
            temp3D = WorldToScreen(temp3D);

            var g = _canvas.CreateGraphics();
            var pen = new Pen(Color.Blue);
            for (var i = 0; i < _adjacent.Length / 2 - 1; i++)
            {
                if (i == _adjacent.Length / 2 - 1)
                {
                    g.DrawLine(pen, temp[_adjacent[i, 0] - 1, 0], temp[_adjacent[i, 1] - 1, 1],
                        temp[0, _adjacent[i + 1, 0] - 1], temp[0, _adjacent[i + 1, 1] - 1]);

                    g.DrawLine(pen, temp3D[_adjacent[i, 0] - 1, 0], temp3D[_adjacent[i, 1] - 1, 1],
                        temp3D[0, _adjacent[i + 1, 0] - 1], temp3D[0, _adjacent[i + 1, 1] - 1]);
                }
                else
                {
                    g.DrawLine(pen, temp[_adjacent[i, 0] - 1, 0], temp[_adjacent[i, 0] - 1, 1],
                        temp[_adjacent[i, 1] - 1, 0], temp[_adjacent[i, 1] - 1, 1]);

                    g.DrawLine(pen, temp3D[_adjacent[i, 0] - 1, 0], temp3D[_adjacent[i, 0] - 1, 1],
                        temp3D[_adjacent[i, 1] - 1, 0], temp3D[_adjacent[i, 1] - 1, 1]);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                g.DrawLine(pen, temp[i, 0], temp[i, 1], temp3D[i, 0], temp3D[i, 1]);
            }

            g.Dispose();
        }

        /// <summary>
        /// Умножение 2 матриц.
        /// </summary>
        /// <param name="a">1 матрица.</param>
        /// <param name="b">2 матрица.</param>
        /// <returns>Результат умножения.</returns>
        public float[,] Multiplication(float[,] a, float[,] b)
        {
            float[,] result = new float[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        result[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Масштабирование вниз.
        /// </summary>
        public void ScaleDown()
        {
            _figure = Multiplication(_figure, _scaleDown);
            _figure3D = Multiplication(_figure3D, _scaleDown);
            DrawFigure();
        }

        /// <summary>
        /// Масштабирование вверх.
        /// </summary>
        public void ScaleUp()
        {
            _figure = Multiplication(_figure, _scaleUp);
            _figure3D = Multiplication(_figure3D, _scaleUp);
            DrawFigure();
        }

        /// <summary>
        /// Сброс фигуры до начальных значений.
        /// </summary>
        public void Clear()
        {
            var temp = _reset;
            var temp3D = _reset3D;
            _figure = temp;
            _figure3D = temp3D;
            _figure = Multiplication(_figure, _reflect);
            _figure3D = Multiplication(_figure3D, _reflect);

        }

        /// <summary>
        /// Поворот фигуры по оси X.
        /// </summary>
        public void RotateX(float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            float[,] rotateX =
            {
                { 1, 0, 0, 0 },
                { 0, cos, -sin, 0 },
                { 0, sin, cos, 0 },
                { 0, 0, 0, 1 }
            };

            _figure = Multiplication(_figure, rotateX);
            _figure3D = Multiplication(_figure3D, rotateX);
        }

        /// <summary>
        /// Поворот фигуры по оси Y.
        /// </summary>
        public void RotateY(float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            float[,] rotateY =
            {
                { cos, 0, sin, 0 },
                { 0, 1, 0, 0 },
                { -sin, 0, cos, 0 },
                { 0, 0, 0, 1 }
            };

            _figure = Multiplication(_figure, rotateY);
            _figure3D = Multiplication(_figure3D, rotateY);
        }

        /// <summary>
        /// Поворот фигуры по оси Z.
        /// </summary>
        public void RotateZ(float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            float[,] rotateZ =
            {
                { cos, -sin, 0, 0 },
                { sin, cos, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };

            _figure = Multiplication(_figure, rotateZ);
            _figure3D = Multiplication(_figure3D, rotateZ);
        }

        /// <summary>
        /// Перемещение фигуры.
        /// </summary>
        /// <param name="deltaX">Смещение фигуры по оси X.</param>
        /// <param name="deltaY">Смещение фигуры по оси Y.</param>
        /// <param name="deltaZ">Смещение фигуры по оси Z.</param>
        public void Move(float deltaX, float deltaY, float deltaZ)
        {
            float[,] translateMatrix =
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { deltaX, deltaY, deltaZ, 1 }
            };

            _figure = Multiplication(_figure, translateMatrix);
            _figure3D = Multiplication(_figure3D, translateMatrix);
        }

    }
}
