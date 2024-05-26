namespace Sun_planets
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GlCtrl = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.tmr = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // GlCtrl
            // 
            this.GlCtrl.AccumBits = ((byte)(0));
            this.GlCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GlCtrl.AutoCheckErrors = false;
            this.GlCtrl.AutoFinish = false;
            this.GlCtrl.AutoMakeCurrent = true;
            this.GlCtrl.AutoSwapBuffers = true;
            this.GlCtrl.BackColor = System.Drawing.Color.Black;
            this.GlCtrl.ColorBits = ((byte)(32));
            this.GlCtrl.DepthBits = ((byte)(16));
            this.GlCtrl.Location = new System.Drawing.Point(-4, 0);
            this.GlCtrl.Name = "GlCtrl";
            this.GlCtrl.Size = new System.Drawing.Size(1007, 626);
            this.GlCtrl.StencilBits = ((byte)(0));
            this.GlCtrl.TabIndex = 0;
            // 
            // tmr
            // 
            this.tmr.Tick += new System.EventHandler(this.tmr_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 595);
            this.Controls.Add(this.GlCtrl);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "планетки";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl GlCtrl;
        private System.Windows.Forms.Timer tmr;
    }
}

