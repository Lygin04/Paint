using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AffineTransformation
{
    public partial class Form1 : Form
    {
        private string _logger = string.Empty;
        private bool _isDragging;
        private bool _isRotating;
        private Point _lastMousePos;
        private float _angle;

        private float[,] _bob = 
        {
            {-4, 5, 1 },
            { 4, 5, 1 },
            { -4, -5, 1 },
            { 4, -5, 1},
            {4, -2, 1 },
            {-4, -2, 1 },
            {2, -7, 1},
            {-2, -7, 1},
            {2, -5, 1 },
            {-2, -5, 1 },
            {3, -7, 1},
            {-3, -7, 1},
            {-4, 1, 1},
            {4, 1, 1},
            {6, -2, 1},
            {-6, -2, 1 },
            {2, 3, 1 },
            {2, 2, 1 },
            {1, 2, 1 },
            {1, 3, 1},
            {-2, 3, 1 },
            {-2, 2, 1 },
            {-1, 2, 1 },
            {-1, 3, 1},
            {1, 0, 1},
            {-1, 0, 1 },
            {-1, 1, 1 },
            {1, 1, 1}
        };

        private readonly int[,] _adjacent =
        {
            {1, 2 },
            {2, 4 },
            {4, 3 },
            {3, 1 },
            {13, 16 },
            {14, 15 },
            {10, 8 },
            {8, 12 },
            {9, 7 },
            {7, 11 },
            {6, 5 },
            {26, 25 },
            {25, 28 },
            {28, 27 },
            {27, 26 },
            {21, 24 },
            {24, 23 },
            {23, 22 },
            {22, 21 },
            {20, 17 },
            {17, 18 },
            {18, 19 },
            {19, 20}
        };

        // Туловище
        private float[,] _body = 
        {
            { -4, 5, 1 },
            { 4, 5, 1 },
            { 4, -5, 1 },
            { -4, -5, 1}
        };

        // Штаны
        private float[,] _pants =
        {
            { 4, -2, 1 },
            { -4, -2, 1 },
        };

        // Левая нога
        private float[,] _leftLeg =
        {
            { -2, -5, 1 },
            { -2, -7, 1 },
            { -3, -7, 1 },
            { -2, -7, 1 }
        };

        // Правая нога
        private float[,] _rightLeg =
        {
            { 2, -5, 1 },
            { 2, -7, 1 },
            { 3, -7, 1 },
            { 2, -7, 1 }
        };

        // Левая рука
        private float[,] _leftHand =
        {
            { -4, 1, 1 },
            { -6, -2, 1 }
        };

        // Правая рука
        private float[,] _rightHand =
        {
            { 4, 1, 1 },
            { 6, -2, 1 }
        };

        // Левый глаз
        private float[,] _leftEye =
        {
            { -2, 3, 1 },
            { -2, 2, 1 },
            { -1, 2 , 1 },
            { -1, 3, 1 }
        };

        // Правый глаз
        private float[,] _rightEye =
        {
            { 2, 3, 1 },
            { 2, 2, 1 },
            { 1, 2 , 1 },
            { 1, 3, 1 }
        };

        // Правый глаз
        private float[,] _mouth =
        {
            { 1, 0, 1 },
            { -1, 0, 1 },
            { -1, 1 , 1 },
            { 1, 1, 1 }
        };

        private readonly float[,] _reset;

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


        public Form1()
        {
            InitializeComponent();
            canvas.Paint += PictureBox1_Paint;

            _reset = _bob;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int w = canvas.ClientSize.Width / 2;
            int h = canvas.ClientSize.Height / 2;
            e.Graphics.TranslateTransform(w, h);
            DrawXAxis(new Point(-w, 0), new Point(w, 0), e.Graphics);
            DrawYAxis(new Point(0, h), new Point(0, -h), e.Graphics);
            e.Graphics.FillEllipse(Brushes.Red, -2, -2, 4, 4);
        }

        private void DrawXAxis(Point start, Point end, Graphics g)
        {
            g.DrawLine(Pens.Black, start, end);
            for (int i = start.X; i < end.X; i += 20)
            {
                g.DrawLine(Pens.LightGray, i, -1000, i, 1000);
            }
        }

        private void DrawYAxis(Point start, Point end, Graphics g)
        {
            g.DrawLine(Pens.Black, start, end);
            for (int i = start.Y; i > end.Y; i -= 20)
            {
                g.DrawLine(Pens.LightGray, -1000, i, 1000, i);
            }
        }

        private void DrawBob()
        {
            DrawFigure(_body);
            DrawFigure(_pants);
            DrawFigure(_leftLeg);
            DrawFigure(_rightLeg);
            DrawFigure(_leftHand);
            DrawFigure(_rightHand);
            DrawFigure(_leftEye);
            DrawFigure(_rightEye);
            DrawFigure(_mouth);
        }

        private void DrawFigure(float[,] figure)
        {
            Graphics g = canvas.CreateGraphics();
            Pen pen = new Pen(Color.Orange);
            int w = canvas.ClientSize.Width / 2;
            int h = canvas.ClientSize.Height / 2;
            g.TranslateTransform(w, h);
            for (int i = 0; i < figure.Length / 3; i++)
            {
                if (i == figure.Length / 3 - 1)
                {
                    g.DrawLine(pen, figure[i, 0] * 20, figure[i, 1] * 20,
                        figure[0, 0] * 20, figure[0, 1] * 20);
                }
                else
                {
                    g.DrawLine(pen, figure[i, 0] * 20, figure[i, 1] * 20,
                        figure[i + 1, 0] * 20, figure[i + 1, 1] * 20);
                }
            }

            g.Dispose();
        }

        private void DrawFigure()
        {
            Graphics g = canvas.CreateGraphics();
            Pen pen = new Pen(Color.Orange);
            int w = canvas.ClientSize.Width / 2;
            int h = canvas.ClientSize.Height / 2;
            g.TranslateTransform(w, h);
            string logger = string.Empty;
            for (int i = 0; i < _adjacent.Length / 2 - 1; i++)
            {
                if (i == _adjacent.Length / 2 - 1)
                {
                    g.DrawLine(pen, _bob[_adjacent[i, 0] - 1, 0] * 20, _bob[_adjacent[i, 1] - 1, 1] * 20,
                        _bob[0, _adjacent[i + 1, 0] - 1] * 20, _bob[0, _adjacent[i + 1, 1] - 1] * 20);
                }
                else
                {
                    g.DrawLine(pen, _bob[_adjacent[i, 0] - 1, 0] * 20, _bob[_adjacent[i, 0] - 1, 1] * 20,
                        _bob[_adjacent[i, 1] - 1, 0] * 20, _bob[_adjacent[i, 1] - 1, 1] * 20);

                    logger += $"X: {_bob[_adjacent[i, 0] - 1, 0]}, Y:{_bob[_adjacent[i, 0] - 1, 1]}  -  " +
                              $"X:{_bob[_adjacent[i, 1] - 1, 0]}, Y:{_bob[_adjacent[i, 1] - 1, 1]}\n";
                }
            }

            g.Dispose();

        }

        private void MultiplicationBob(float[,] b)
        {
            _body = Multiplication(_body, b);
            _pants = Multiplication(_pants, b);
            _leftLeg = Multiplication(_leftLeg, b);
            _rightLeg = Multiplication(_rightLeg, b);
            _leftHand = Multiplication(_leftHand, b);
            _rightHand = Multiplication(_rightHand, b);
            _leftEye = Multiplication(_leftEye, b);
            _rightEye = Multiplication(_rightEye, b);
            _mouth = Multiplication(_mouth, b);
        }

        static float[,] Multiplication(float[,] a, float[,] b)
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

        private void drawButton_Click(object sender, EventArgs e)
        {
            DrawFigure();
        }

        private void scaleDownButton_Click(object sender, EventArgs e)
        {
            Refresh();
            _bob = Multiplication(_bob, _scaleDown);
            DrawFigure();
        }

        private void scaleUpButton_Click(object sender, EventArgs e)
        {
            Refresh();
            _bob = Multiplication(_bob, _scaleUp);
            DrawFigure();
        }

        private void reflectionButton_Click(object sender, EventArgs e)
        {
            Refresh();
            _bob = Multiplication(_bob, _reflect);
            DrawFigure();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Refresh();
            _bob = _reset;
            DrawFigure();
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _isRotating = true;
                canvas.Cursor = Cursors.SizeWE;
            }
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                canvas.Cursor = Cursors.SizeAll;
            }
            _lastMousePos = e.Location;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _isRotating = false;
            }
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
            }
            canvas.Cursor = Cursors.Default;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            int deltaX = e.X - _lastMousePos.X;
            int deltaY = e.Y - _lastMousePos.Y;
            
            if (_isRotating)
            {
                _logger += $"{e.X} - {_lastMousePos.X} = {deltaX}";

                _angle = deltaX * 0.05f;

                float cos = MathF.Cos(_angle);
                float sin = MathF.Sin(_angle);

                float[,] rotate =
                {
                    { cos, -sin, 0 },
                    { sin, cos, 0 },
                    { 0, 0, 1 }
                };

                _bob = Multiplication(_bob, rotate);

                Refresh();
                DrawFigure();

                _lastMousePos = e.Location;
            }

            if (_isDragging)
            {
                _logger += $"X:{deltaX}, Y:{deltaY}\n";

                float[,] translateMatrix =
                {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { deltaX, deltaY, 1 }
                };

                _bob = Multiplication(_bob, translateMatrix);

                Refresh();
                DrawFigure();

                _lastMousePos = e.Location;
            }
        }
    }
}
