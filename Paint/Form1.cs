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
        };

        /*private float[,] _bob3D =
        {
            {-4, 5, 0, 1 },     // 0
            { 4, 5, 0, 1 },     // 1
            { -4, -5, 0, 1 },   // 2
            { 4, -5, 0, 1},     // 3
            {4, -2, 0, 1 },     // 4
            {-4, -2, 0, 1 },    // 5
            {2, -7, 2.5f, 1},   // 6
            {-2, -7, 2.5f, 1},  // 7
            {2, -5, 2.5f, 1 },  // 8
            {-2, -5, 2.5f, 1 }, // 9
            {3, -7, 2.5f, 1},   // 10
            {-3, -7, 2.5f, 1},  // 11
            {-4, 1, 2.5f, 1},   // 12
            {4, 1, 2.5f, 1},    // 13
            {6, -2, 2.5f, 1},   // 14
            {-6, -2, 2.5f, 1 }, // 15
            {2, 3, 0, 1 },      // 16
            {2, 2, 0, 1 },      // 17
            {1, 2, 0, 1 },      // 18
            {1, 3, 0, 1},       // 19
            {-2, 3, 0, 1 },     // 20
            {-2, 2, 0, 1 },     // 21
            {-1, 2, 0, 1 },     // 22
            {-1, 3, 0, 1},      // 23
            {1, 0, 0, 1},       // 24
            {-1, 0, 0, 1 },     // 25
            {-1, 1, 0, 1 },     // 26
            {1, 1, 0, 1},       // 27

            {-4, 5, 5, 1 },     // 28
            { 4, 5, 5, 1 },     // 29
            { -4, -5, 5, 1 },   // 30
            { 4, -5, 5, 1},     // 31
            {4, -2, 5, 1 },     // 32
            {-4, -2, 5, 1 },    // 33
            {2, -7, 5, 1},      // 34
            {-2, -7, 5, 1},     // 35
            {2, -5, 5, 1 },     // 36
            {-2, -5, 5, 1 },    // 37
            {3, -7, 5, 1},      // 38
            {-3, -7, 5, 1},     // 39
            {-4, 1, 5, 1},      // 40
            {4, 1, 5, 1},       // 41
            {6, -2, 5, 1},      // 42
            {-6, -2, 5, 1 },    // 43
        };*/

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

        /*private readonly int[,] _adjacent3D =
        {
            {1, 2, 3 },         // Тело
            {2, 3, 4 },
            {3, 6, 5 },
            {3, 5, 4 },

            {13, 16, 13 },      // Левая рука
            {14, 15, 14 },      // Правая рука

            {10, 8, 12 },       // Левая нога
            {9, 7, 11 },        // Правая нога

            {27, 28, 26 },      // Рот
            {26, 28, 25 },

            {22, 21, 24 },      // Правый глаз
            {22, 24, 23 },

            {19, 20, 17 },      // Левый глаз
            {19, 17, 18 },

            {1 + 28, 2 + 28, 3 + 28},         // Тело
            {2 + 28, 3 + 28, 4 + 28 },
            {3 + 28, 6 + 28, 5 + 28 },
            {3 + 28, 5 + 28, 4 + 28 },

            {1, 29, 2 },               // Соединяющие линии
            {2, 30, 1},
            {3, 31, 4},
            {4, 32, 3},
            {5, 33, 6},
            {6, 34, 5}

        };*/

        private int[,] _adjacent3D =
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
            
            /*{8, 9, 10},
            {8, 10, 11},

            {4, 9, 5},
            {4, 8, 9},

            {5, 9, 6},
            {10, 6, 9},

            {7, 6, 10},
            {7, 10, 11},

            {7, 8, 4},
            {8, 7, 11},*/
        };

        /*private readonly int[,] _adjacent3D =
        {
            {1, 2 },              // Первая 2D фигура
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
            {19, 20},

            {1 + 28, 2 + 28},       // Вторая 2D фигура
            {2 + 28, 4 + 28 },
            {4 + 28, 3 + 28 },
            {3 + 28, 1 + 28 },
            {13 + 28, 16 + 28 },
            {14 + 28, 15 + 28 },
            {10 + 28, 8 + 28 },
            {8 + 28, 12 + 28 },
            {9 + 28, 7 + 28 },
            {7 + 28, 11 + 28 },
            {6 + 28, 5 + 28 },
            {26 + 28, 25 + 28 },
            {25 + 28, 28 + 28 },
            {28 + 28, 27 + 28 },
            {27 + 28, 26 + 28 },
            {21 + 28, 24 + 28 },
            {24 + 28, 23 + 28 },
            {23 + 28, 22 + 28 },
            {22 + 28, 21 + 28 },
            {20 + 28, 17 + 28 },
            {17 + 28, 18 + 28 },
            {18 + 28, 19 + 28 },
            {19 + 28, 20 + 28},

            {1, 29 },               // Соединяющие линии
            {2, 30},
            {3, 31},
            {4, 32},
            {5, 33},
            {6, 34 }
        };*/


        public Form1()
        {
            InitializeComponent();
            canvas.Paint += PictureBox1_Paint;
            KeyPreview = true;
            _figure2D = new Figure3D(_bob3D, _adjacent3D, canvas);
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
                _figure2D.RotateY(deltaX / 100f);
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
                    _figure2D.RotateX();
                    break;
                case Keys.S:
                    _figure2D.RotateX(-0.05f);
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
