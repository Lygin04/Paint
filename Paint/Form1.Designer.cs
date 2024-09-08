
namespace AffineTransformation
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
            reflectionButton = new System.Windows.Forms.Button();
            clearButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            SuspendLayout();
            // 
            // canvas
            // 
            canvas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            canvas.BackColor = System.Drawing.SystemColors.Window;
            canvas.Location = new System.Drawing.Point(7, 8);
            canvas.Margin = new System.Windows.Forms.Padding(2);
            canvas.Name = "canvas";
            canvas.Size = new System.Drawing.Size(1180, 734);
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
            drawButton.Location = new System.Drawing.Point(7, 746);
            drawButton.Margin = new System.Windows.Forms.Padding(2);
            drawButton.Name = "drawButton";
            drawButton.Size = new System.Drawing.Size(142, 29);
            drawButton.TabIndex = 1;
            drawButton.Text = "Нарисовать Боба";
            drawButton.UseVisualStyleBackColor = false;
            drawButton.Click += drawButton_Click;
            // 
            // scaleUpButton
            // 
            scaleUpButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            scaleUpButton.BackColor = System.Drawing.SystemColors.Info;
            scaleUpButton.Location = new System.Drawing.Point(1191, 254);
            scaleUpButton.Margin = new System.Windows.Forms.Padding(2);
            scaleUpButton.Name = "scaleUpButton";
            scaleUpButton.Size = new System.Drawing.Size(29, 42);
            scaleUpButton.TabIndex = 3;
            scaleUpButton.Text = "+";
            scaleUpButton.UseVisualStyleBackColor = false;
            scaleUpButton.Click += scaleUpButton_Click;
            // 
            // scaleDownButton
            // 
            scaleDownButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            scaleDownButton.BackColor = System.Drawing.SystemColors.Info;
            scaleDownButton.Location = new System.Drawing.Point(1191, 309);
            scaleDownButton.Margin = new System.Windows.Forms.Padding(2);
            scaleDownButton.Name = "scaleDownButton";
            scaleDownButton.Size = new System.Drawing.Size(29, 38);
            scaleDownButton.TabIndex = 4;
            scaleDownButton.Text = "-";
            scaleDownButton.UseVisualStyleBackColor = false;
            scaleDownButton.Click += scaleDownButton_Click;
            // 
            // reflectionButton
            // 
            reflectionButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            reflectionButton.BackColor = System.Drawing.SystemColors.Info;
            reflectionButton.Location = new System.Drawing.Point(153, 746);
            reflectionButton.Margin = new System.Windows.Forms.Padding(2);
            reflectionButton.Name = "reflectionButton";
            reflectionButton.Size = new System.Drawing.Size(142, 29);
            reflectionButton.TabIndex = 5;
            reflectionButton.Text = "Отражение";
            reflectionButton.UseVisualStyleBackColor = false;
            reflectionButton.Click += reflectionButton_Click;
            // 
            // clearButton
            // 
            clearButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            clearButton.BackColor = System.Drawing.SystemColors.Info;
            clearButton.Location = new System.Drawing.Point(299, 746);
            clearButton.Margin = new System.Windows.Forms.Padding(2);
            clearButton.Name = "clearButton";
            clearButton.Size = new System.Drawing.Size(142, 29);
            clearButton.TabIndex = 7;
            clearButton.Text = "Очистить";
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += clearButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1220, 782);
            Controls.Add(clearButton);
            Controls.Add(reflectionButton);
            Controls.Add(scaleDownButton);
            Controls.Add(scaleUpButton);
            Controls.Add(drawButton);
            Controls.Add(canvas);
            Margin = new System.Windows.Forms.Padding(2);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.Button scaleUpButton;
        private System.Windows.Forms.Button scaleDownButton;
        private System.Windows.Forms.Button reflectionButton;
        private System.Windows.Forms.Button clearButton;
    }
}

