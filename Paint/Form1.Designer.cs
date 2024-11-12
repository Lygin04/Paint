
namespace Paint
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            canvas = new System.Windows.Forms.PictureBox();
            drawButton = new System.Windows.Forms.Button();
            scaleUpButton = new System.Windows.Forms.Button();
            scaleDownButton = new System.Windows.Forms.Button();
            clearButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            SuspendLayout();
            // 
            // canvas
            // 
            canvas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            canvas.BackColor = System.Drawing.SystemColors.Window;
            canvas.Location = new System.Drawing.Point(11, 13);
            canvas.Name = "canvas";
            canvas.Size = new System.Drawing.Size(1918, 1174);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.MouseDown += canvas_MouseDown;
            canvas.MouseMove += canvas_MouseMove;
            canvas.MouseUp += canvas_MouseUp;
            // 
            // drawButton
            // 
            drawButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            drawButton.BackColor = System.Drawing.SystemColors.Info;
            drawButton.Location = new System.Drawing.Point(11, 1194);
            drawButton.Name = "drawButton";
            drawButton.Size = new System.Drawing.Size(231, 46);
            drawButton.TabIndex = 1;
            drawButton.Text = "Нарисовать Боба";
            drawButton.UseVisualStyleBackColor = false;
            drawButton.Click += drawButton_Click;
            // 
            // scaleUpButton
            // 
            scaleUpButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            scaleUpButton.BackColor = System.Drawing.SystemColors.Info;
            scaleUpButton.Location = new System.Drawing.Point(1935, 406);
            scaleUpButton.Name = "scaleUpButton";
            scaleUpButton.Size = new System.Drawing.Size(47, 67);
            scaleUpButton.TabIndex = 3;
            scaleUpButton.Text = "+";
            scaleUpButton.UseVisualStyleBackColor = false;
            scaleUpButton.Click += scaleUpButton_Click;
            // 
            // scaleDownButton
            // 
            scaleDownButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            scaleDownButton.BackColor = System.Drawing.SystemColors.Info;
            scaleDownButton.Location = new System.Drawing.Point(1935, 494);
            scaleDownButton.Name = "scaleDownButton";
            scaleDownButton.Size = new System.Drawing.Size(47, 61);
            scaleDownButton.TabIndex = 4;
            scaleDownButton.Text = "-";
            scaleDownButton.UseVisualStyleBackColor = false;
            scaleDownButton.Click += scaleDownButton_Click;
            // 
            // clearButton
            // 
            clearButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            clearButton.BackColor = System.Drawing.SystemColors.Info;
            clearButton.Location = new System.Drawing.Point(248, 1194);
            clearButton.Name = "clearButton";
            clearButton.Size = new System.Drawing.Size(231, 46);
            clearButton.TabIndex = 7;
            clearButton.Text = "Очистить";
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += clearButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1982, 1251);
            Controls.Add(clearButton);
            Controls.Add(scaleDownButton);
            Controls.Add(scaleUpButton);
            Controls.Add(drawButton);
            Controls.Add(canvas);
            Name = "Form1";
            Text = "Губка Боб";
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.Button scaleUpButton;
        private System.Windows.Forms.Button scaleDownButton;
        private System.Windows.Forms.Button clearButton;
    }
}

