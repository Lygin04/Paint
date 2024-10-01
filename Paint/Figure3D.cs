using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paint
{
    public class Figure3D
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
            { 0, 0, 1, -0.002f },  // Чем меньше коэффициент, тем сильнее эффект перспективы
            { 0, 0, 0, 1 }
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

        public Figure3D(float[,] figure, float[,] figure3D, int[,] adjacent, PictureBox canvas)
        {
            _canvas = canvas;
            _figure = figure;
            _figure3D = figure3D;
/*            _figure = Multiplication(_figure, _reflect);
            _figure3D = Multiplication(_figure3D, _reflect);
*/            _reset = figure;
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
                float w = 1 + z * _perspective[2, 3];  // Это модифицированная координата W

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
        /// Вычисление нормали грани
        /// </summary>
        private float[] CalculateNormal(float[] v0, float[] v1, float[] v2)
        {
            float[] vector1 = { v1[0] - v0[0], v1[1] - v0[1], v1[2] - v0[2] };
            float[] vector2 = { v2[0] - v0[0], v2[1] - v0[1], v2[2] - v0[2] };

            // Векторное произведение двух векторов для получения нормали
            return new float[]
            {
                vector1[1] * vector2[2] - vector1[2] * vector2[1],
                vector1[2] * vector2[0] - vector1[0] * vector2[2],
                vector1[0] * vector2[1] - vector1[1] * vector2[0]
            };
        }

        /// <summary>
        /// Нормализация вектора
        /// </summary>
        private float[] Normalize(float[] vector)
        {
            float length = (float)Math.Sqrt(vector[0] * vector[0] + vector[1] * vector[1] + vector[2] * vector[2]);
            return new float[] { vector[0] / length, vector[1] / length, vector[2] / length };
        }

        /// <summary>
        /// Проверка видимости грани
        /// </summary>
        private bool IsFaceVisible(float[] normal, float[] cameraPosition, float[] pointOnFace)
        {
            // Вектор от точки на грани до камеры
            float[] viewVector = { cameraPosition[0] - pointOnFace[0], cameraPosition[1] - pointOnFace[1], cameraPosition[2] - pointOnFace[2] };

            // Скалярное произведение нормали и вектора взгляда
            float dotProduct = normal[0] * viewVector[0] + normal[1] * viewVector[1] + normal[2] * viewVector[2];

            return dotProduct < 0; // Грань видима, если скалярное произведение отрицательное
        }

        /// <summary>
        /// Отрисовка фигуры с удалением невидимых ребер
        /// </summary>
        public void DrawFigure()
        {
            var screenPoints = WorldToScreen(_figure);
            var g = _canvas.CreateGraphics();
            var pen = new Pen(Color.Blue);

            float[] cameraPosition = { 0, 1, 0 }; // Камера расположена по оси Z

            for (var i = 0; i < _adjacent.GetLength(0); i++)
            {
                // Определяем вершины текущей грани
                float[] v0 = { _figure[_adjacent[i, 0] - 1, 0], _figure[_adjacent[i, 0] - 1, 1], _figure[_adjacent[i, 0] - 1, 2] };
                float[] v1 = { _figure[_adjacent[i, 1] - 1, 0], _figure[_adjacent[i, 1] - 1, 1], _figure[_adjacent[i, 1] - 1, 2] };
                float[] v2 = { _figure[_adjacent[i, 1] - 1, 0], _figure[_adjacent[i, 1] - 1, 1], _figure[_adjacent[i, 1] - 1, 2] };

                // Вычисляем нормаль
                var normal = CalculateNormal(v0, v1, v2);
                normal = Normalize(normal);

                // Проверяем видимость грани
                if (IsFaceVisible(normal, cameraPosition, v0))
                {
                    // Рисуем видимую грань
                    g.DrawLine(pen,
                        screenPoints[_adjacent[i, 0] - 1, 0], screenPoints[_adjacent[i, 0] - 1, 1],
                        screenPoints[_adjacent[i, 1] - 1, 0], screenPoints[_adjacent[i, 1] - 1, 1]);
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
