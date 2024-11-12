using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Paint
{
    public class Figure3D
    {
        private List<float[,]> _cubs = [];
        private float[,] _figure;                         // Матрицы фигуры.
        private readonly int[,] _adjacent;                // Смежная матрица фигуры.
        private readonly float[,] _reset;                 // Исходная матрица фигуры.
        private readonly PictureBox _canvas;              // Холст на котором всё рисуется.
        
        private int[,] _indices =
        {
            {0, 1, 2}, // 0
            {0, 2, 3}, // 1

            {4, 6, 5}, // 2
            {4, 7, 6}, // 3

            {0, 5, 1}, // 4
            {0, 4, 5}, // 5

            {1, 5, 2}, // 6
            {6, 2, 5}, // 7

            {3, 2, 6}, // 8
            {3, 6, 7}, // 9

            {3, 4, 0}, // 10
            {4, 3, 7}, // 11
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

        public Figure3D(float[,] figure, int[,] adjacent, PictureBox canvas)
        {
            _canvas = canvas;
            _figure = figure;
            _figure = Multiplication(_figure, _reflect);
            _reset = figure;
            _adjacent = adjacent;
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

            var scale = 50f;

            for (var i = 0; i < position.Length / 4; i++)
            {
                temp[i, 0] = screenCenterX + temp[i, 0] * scale;
                temp[i, 1] = screenCenterY + temp[i, 1] * scale;
            }

            return temp;
        }

        /// <summary>
        /// Перевод из мировых в экранные координаты.
        /// </summary>
        /// <param name="cubs"></param>
        private List<float[,]> WorldToScreen(List<float[,]> cubs)
        {
            List<float[,]> worldPosition = new();
            foreach (var cub in cubs)
            {
                var temp = Utils.Copy(cub);
                var screenCenterX = (float)_canvas.ClientSize.Width / 2;
                var screenCenterY = (float)_canvas.ClientSize.Height / 2;

                var scale = 10f;

                for (var i = 0; i < cub.Length / 4; i++)
                {
                    temp[i, 0] = screenCenterX + temp[i, 0] * scale;
                    temp[i, 1] = screenCenterY + temp[i, 1] * scale;
                }   
                worldPosition.Add(temp);
            }

            return worldPosition;
        }

        /// <summary>
        /// Отрисовка фигуры.
        /// </summary>
        public void DrawFigure()
        {
            var temp = WorldToScreen(_figure);
            var g = _canvas.CreateGraphics();
            var brush = new SolidBrush(Color.FromArgb(128, Color.Blue));
            var pen = new Pen(Color.Blue);

            for (var i = 0; i < _adjacent.GetLength(0); i++)
            {
                var v1 = _adjacent[i, 0];
                var v2 = _adjacent[i, 1];
                var v3 = _adjacent[i, 2];

                var V1 = new Vector3(temp[v1, 0], temp[v1, 1], temp[v1, 2]);
                var V2 = new Vector3(temp[v2, 0], temp[v2, 1], temp[v2, 2]);
                var V3 = new Vector3(temp[v3, 0], temp[v3, 1], temp[v3, 2]);

                var t1 = V1 - V2;
                var t2 = V2 - V3;

                var normal = Vector3.Normalize(Vector3.Cross(t1, t2));
                var camera = new Vector3(0f, 0f, 1f);
                if (Vector3.Dot(camera, normal) > 0)
                {
                    PointF[] points =
                    {
                        new (temp[v1, 0], temp[v1, 1]),
                        new (temp[v2, 0], temp[v2, 1]),
                        new (temp[v3, 0], temp[v3, 1])
                    };

                    //g.FillPolygon(brush, points);  // Закраска грани
                    g.DrawPolygon(pen, points);    // Обводка грани
                }
            }

            g.Dispose();
        }

        /// <summary>
        /// Отрисовка фигуры.
        /// </summary>
        public void DrawCubs()
        {
            var temps = WorldToScreen(_cubs);
            var g = _canvas.CreateGraphics();
            var brush = new SolidBrush(Color.FromArgb(128, Color.Blue));
            var pen = new Pen(Color.Blue);

            foreach (var temp in temps)
            {
                for (var i = 0; i < _indices.GetLength(0); i++)
                {
                    var v1 = _indices[i, 0];
                    var v2 = _indices[i, 1];
                    var v3 = _indices[i, 2];

                    var V1 = new Vector3(temp[v1, 0], temp[v1, 1], temp[v1, 2]);
                    var V2 = new Vector3(temp[v2, 0], temp[v2, 1], temp[v2, 2]);
                    var V3 = new Vector3(temp[v3, 0], temp[v3, 1], temp[v3, 2]);

                    var t1 = V1 - V2;
                    var t2 = V2 - V3;

                    var normal = Vector3.Normalize(Vector3.Cross(t1, t2));
                    var camera = new Vector3(0f, 0f, 1f);
                    if (Vector3.Dot(camera, normal) > 0)
                    {
                        PointF[] points =
                        {
                            new(temp[v1, 0], temp[v1, 1]),
                            new(temp[v2, 0], temp[v2, 1]),
                            new(temp[v3, 0], temp[v3, 1])
                        };

                        //g.FillPolygon(brush, points);  // Закраска грани
                        g.DrawPolygon(pen, points); // Обводка грани
                    }
                }

                g.Dispose();
            }
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
            DrawFigure();
        }

        /// <summary>
        /// Масштабирование вверх.
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
            var temp = _reset;
            _figure = temp;
            _figure = Multiplication(_figure, _reflect);
        }

        /// <summary>
        /// Поворот фигуры по оси X.
        /// </summary>
        public void RotateX(float angle = 0.05f)
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
        }

        /// <summary>
        /// Поворот фигуры по оси Y.
        /// </summary>
        public void RotateY(float angle = 0.05f)
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
        }

        /// <summary>
        /// Поворот фигуры по оси Z.
        /// </summary>
        public void RotateZ(float angle = 0.05f)
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
        }

        public void CreateCube(Point3D startPosition, float sizeX, float sizeY, float sizeZ)
        {
            float[,] cube =
            {
                {startPosition.X, startPosition.Y, startPosition.Z, 1},
                {startPosition.X - sizeX, startPosition.Y, startPosition.Z, 1},
                {startPosition.X - sizeX, startPosition.Y, startPosition.Z + sizeZ, 1},
                {startPosition.X, startPosition.Y, startPosition.Z + sizeZ, 1},
                
                {startPosition.X, startPosition.Y - sizeY, startPosition.Z, 1},
                {startPosition.X + sizeX, startPosition.Y - sizeY, startPosition.Z, 1},
                {startPosition.X - sizeX, startPosition.Y - sizeY, startPosition.Z + sizeZ, 1},
                {startPosition.X, startPosition.Y - sizeY, startPosition.Z + sizeZ, 1},
            };
            _cubs.Add(cube);
        }
    }
}
