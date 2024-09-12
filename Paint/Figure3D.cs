using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    internal class Figure3D
    {
        private float[,] _figure;               // Матрица фигуры.
        private readonly int[,] _adjacent;      // Смежная матрица фигуры.
        private readonly float[,] _reset;       // Исходная матрица фигуры.
        private readonly PictureBox _canvas;    // Холст на котором всё рисуется.

        // Уменьшение
        private readonly float[,] _scaleDown =
        {
            { 0.5f, 0, 0 },
            { 0, 0.5f, 0 },
            { 0, 0, 1 }
        };

        // Увеличение
        private readonly float[,] _scaleUp =
        {
            { 2, 0, 0 },
            { 0, 2, 0 },
            { 0, 0, 1 }
        };

        // Отражение
        private readonly float[,] _reflect =
        {
            { 1, 0, 0 },
            { 0, -1, 0 },
            { 0, 0, 1 },
        };

        public Figure3D(float[,] figure, int[,] adjacent, PictureBox canvas)
        {
            _canvas = canvas;
            _figure = figure;
            _figure = Multiplication(_figure, _reflect);
            WorldToScreen(_figure);
            _reset = figure;
            _adjacent = adjacent;
        }

        /// <summary>
        /// Перевод из мировых в экранные координаты.
        /// </summary>
        /// <param name="position"></param>
        private void WorldToScreen(float[,] position)
        {
            float screenCenterX = (float)_canvas.ClientSize.Width / 2;
            float screenCenterY = (float)_canvas.ClientSize.Height / 2;

            float scale = 20f;

            for (int i = 0; i < position.Length / 3; i++)
            {
                position[i, 0] = screenCenterX + position[i, 0] * scale;
                position[i, 1] = screenCenterY + position[i, 1] * scale;
            }
        }

        /// <summary>
        /// Отрисовка фигуры.
        /// </summary>
        public void DrawFigure()
        {
            Graphics g = _canvas.CreateGraphics();
            Pen pen = new Pen(Color.Blue);
            for (int i = 0; i < _adjacent.Length / 2 - 1; i++)
            {
                if (i == _adjacent.Length / 2 - 1)
                {
                    g.DrawLine(pen, _figure[_adjacent[i, 0] - 1, 0], _figure[_adjacent[i, 1] - 1, 1],
                        _figure[0, _adjacent[i + 1, 0] - 1], _figure[0, _adjacent[i + 1, 1] - 1]);
                }
                else
                {
                    g.DrawLine(pen, _figure[_adjacent[i, 0] - 1, 0], _figure[_adjacent[i, 0] - 1, 1],
                        _figure[_adjacent[i, 1] - 1, 0], _figure[_adjacent[i, 1] - 1, 1]);
                }
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
        /// Уменьшение размера фигуры.
        /// </summary>
        public void ScaleDown()
        {
            _figure = Multiplication(_figure, _scaleDown);
            DrawFigure();
        }

        /// <summary>
        /// Увеличение размера фигуры.
        /// </summary>
        public void ScaleUp()
        {
            _figure = Multiplication(_figure, _scaleUp);
            DrawFigure();
        }

        /// <summary>
        /// Сброс фигуры до начальных значений.
        /// </summary>
        public void Clear()
        {
            _figure = _reset;
            DrawFigure();
        }

        /// <summary>
        /// Поворот фигуры.
        /// </summary>
        /// <param name="deltaX">Смещение по оси X. Для расчета угла смещения.</param>
        public void Rotate(float deltaX)
        {
            float _angle = deltaX * 0.05f;

            float cos = MathF.Cos(_angle);
            float sin = MathF.Sin(_angle);

            float[,] rotate =
            {
                    { cos, 0, -sin },
                    { 0, 1, 0 },
                    { sin, 0, cos }
                };

            _figure = Multiplication(_figure, rotate);
        }

        /// <summary>
        /// Перемещение фигуры.
        /// </summary>
        /// <param name="deltaX">Смещение фигуры по оси X.</param>
        /// <param name="deltaY">Смещение фигуры по оси Y.</param>
        public void Move(float deltaX, float deltaY)
        {
            float[,] translateMatrix =
{
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { deltaX, deltaY, 0, 1}
                };

            _figure = Multiplication(_figure, translateMatrix);
        }

    }
}
