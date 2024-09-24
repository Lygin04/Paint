using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        private bool _isDragging;
        private bool _isRotating;
        private Point _lastMousePos;

        private Figure3D _figure2D;

        private float[,] _bob = 
        {
            {-4, 5, 0, 1 },
            { 4, 5, 0, 1 },
            { -4, -5, 0, 1 },
            { 4, -5, 0, 1},
            {4, -2, 0, 1 },
            {-4, -2, 0, 1 },
            {2, -7, 0, 1},
            {-2, -7, 0, 1},
            {2, -5, 0, 1 },
            {-2, -5, 0, 1 },
            {3, -7, 0, 1},
            {-3, -7, 0, 1},
            {-4, 1, 0, 1},
            {4, 1, 0, 1},
            {6, -2, 0, 1},
            {-6, -2, 0, 1 },
            {2, 3, 0, 1 },
            {2, 2, 0, 1 },
            {1, 2, 0, 1 },
            {1, 3, 0, 1},
            {-2, 3, 0, 1 },
            {-2, 2, 0, 1 },
            {-1, 2, 0, 1 },
            {-1, 3, 0, 1},
            {1, 0, 0, 1},
            {-1, 0, 0, 1 },
            {-1, 1, 0, 1 },
            {1, 1, 0, 1}
        };

        private float[,] _bob3D =
        {
            {-4, 5, 5, 1 },
            { 4, 5, 5, 1 },
            { -4, -5, 5, 1 },
            { 4, -5, 5, 1},
            {4, -2, 5, 1 },
            {-4, -2, 5, 1 },
            {2, -7, 5, 1},
            {-2, -7, 5, 1},
            {2, -5, 5, 1 },
            {-2, -5, 5, 1 },
            {3, -7, 5, 1},
            {-3, -7, 5, 1},
            {-4, 1, 5, 1},
            {4, 1, 5, 1},
            {6, -2, 5, 1},
            {-6, -2, 5, 1 },
            {2, 3, 5, 1 },
            {2, 2, 5, 1 },
            {1, 2, 5, 1 },
            {1, 3, 5, 1},
            {-2, 3, 5, 1 },
            {-2, 2, 5, 1 },
            {-1, 2, 5, 1 },
            {-1, 3, 5, 1},
            {1, 0, 5, 1},
            {-1, 0, 5, 1 },
            {-1, 1, 5, 1 },
            {1, 1, 5, 1}
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

        public Form1()
        {
            InitializeComponent();
            canvas.Paint += PictureBox1_Paint;

            _figure2D = new Figure3D(_bob, _bob3D, _adjacent, canvas);
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

        private void drawButton_Click(object sender, EventArgs e)
        {
            _figure2D.DrawFigure();
        }

        private void scaleDownButton_Click(object sender, EventArgs e)
        {
            Refresh();
            _figure2D.ScaleDown();
        }

        private void scaleUpButton_Click(object sender, EventArgs e)
        {
            Refresh();
            _figure2D.ScaleUp();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Refresh();
            _figure2D.Clear();
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
                _figure2D.RotateY(deltaX);
                Refresh();
                _figure2D.DrawFigure();

                _lastMousePos = e.Location;
            }

            if (_isDragging)
            {
                _figure2D.Move(deltaX, deltaY, 0);
                Refresh();
                _figure2D.DrawFigure();

                _lastMousePos = e.Location;
            }
        }
    }
}
