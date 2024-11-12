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

        private float[,] _vertices =
        {
            {-0.5f, 1, 0.5f, 1 },
            {-0.5f, 1, -0.5f, 1 },
            {0.5f, 1, -0.5f, 1 },
            {0.5f, 1, 0.5f, 1 },
            {-1, -1, 1, 1 },
            {-1, -1, -1, 1 },
            {1, -1, -1, 1 },
            {1, -1, 1, 1 },
        };

        private float[,] indices =
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
            {-4, 5, -5, 1},     // 0
            {4, 5, -5, 1},      // 1
            {4, 5, 0, 1},       // 2
            {-4, 5, 0, 1},      // 3
            {-4, -2, -5, 1},     // 4
            {4, -2, -5, 1},      // 5
            {4, -2, 0, 1},       // 6
            {-4, -2, 0, 1},      // 7
            {-4, -5, -5, 1},    // 8
            {4, -5, -5, 1},     // 9
            {4, -5, 0, 1},      // 10
            {-4, -5, 0, 1},     // 11
            {-2, 3, 0, 1},      // 12
            {-1, 3, 0, 1},      // 13
            {-2, 2, 0, 1},      // 14
            {-1, 2, 0, 1},      // 15
            {1, 3, 0, 1},      // 16
            {2, 3, 0, 1},      // 17
            {1, 2, 0, 1},      // 18
            {2, 2, 0, 1},      // 19
            {-1, 1, 0, 1},     // 20
            {1, 1, 0, 1},       // 21
            {-1, 0, 0, 1},       // 22
            {1, 0, 0, 1},      //23
            
            {4, 0.5f, -2.5f, 1},    //24
            {8, 0.5f, -2.5f, 1},   // 25
            {8, 0.5f, -3f, 1},   // 26
            {4, 0.5f, -3f, 1},   // 27
            
            {4, -0.5f, -2.5f, 1},    //28
            {8, -0.5f, -2.5f, 1},   // 29
            {8, -0.5f, -3f, 1},   // 30
            {4, -0.5f, -3f, 1},   // 31
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

        private int[,] _adjacent3D =
        {
            {0, 1, 2}, // 0
            {0, 2, 3}, // 1

            /*{4, 6, 5}, // 2
            {4, 7, 6}, // 3*/

            {0, 5, 1}, // 4
            {0, 4, 5}, // 5

            {1, 5, 2}, // 6
            {6, 2, 5}, // 7

            {3, 2, 6}, // 8
            {3, 6, 7}, // 9

            {3, 4, 0}, // 10
            {4, 3, 7}, // 11
            
            
            /*{4, 5, 6},
            {4, 6, 7},*/

            {8, 10, 9},
            {8, 11, 10},
            
            {4, 9, 5},
            {4, 8, 9},
            
            {5, 9, 6},
            {10, 6, 9},

            {7, 6, 10},
            {7, 10, 11},

            {7, 8, 4},
            {8, 7, 11},
            
            {12, 13, 15},   // Левый глаз
            {12, 15, 14},
            
            {16, 17, 19},   // Правый глаз
            {16, 19, 18},
            
            {20, 21, 23},   // Рот
            {20, 23, 22},
            
            /*{0 + 24, 1 + 24, 2 + 24}, // 0
            {0 + 24, 2 + 24, 3 + 24}, // 1

            {4 + 24, 6 + 24, 5 + 24}, // 2
            {4 + 24, 7 + 24, 6 + 24}, // 3

            {0 + 24, 5 + 24, 1 + 24}, // 4
            {0 + 24, 4 + 24, 5 + 24}, // 5

            {1 + 24, 5 + 24, 2 + 24}, // 6
            {6 + 24, 2 + 24, 5 + 24}, // 7

            {3 + 24, 2 + 24, 6 + 24}, // 8
            {3 + 24, 6 + 24, 7 + 24}, // 9

            {3 + 24, 4 + 24, 0 + 24}, // 10
            {4 + 24, 3 + 24, 7 + 24}, // 11*/
        };
        
        public Form1()
        {
            InitializeComponent();
            canvas.Paint += PictureBox1_Paint;
            KeyPreview = true;
            _figure2D = new Figure3D(_bob3D, _adjacent3D, canvas);
            
            _figure2D.CreateCube(new Point3D(-4, 5, -5), 8, 7, 5);
            //_figure2D.CreateCube(new Point3D(-4, -2, -5), 8, 3, 5);
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
                _figure2D.RotateY(-deltaX / 100f);
                _figure2D.RotateX(deltaY / 100f);
                Refresh();
                _figure2D.DrawFigure();

                _lastMousePos = e.Location;
            }

            if (_isDragging)
            {
                _figure2D.Move(deltaX / 50f, deltaY / 50f, 0);
                Refresh();
                _figure2D.DrawFigure();

                _lastMousePos = e.Location;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    _figure2D.RotateX(-0.05f);
                    break;
                case Keys.S:
                    _figure2D.RotateX();
                    break;
                case Keys.A:
                    _figure2D.RotateY();
                    break;
                case Keys.D:
                    _figure2D.RotateY(-0.05f);
                    break;
            }
            Refresh();
            _figure2D.DrawFigure();
        }
    }
}
