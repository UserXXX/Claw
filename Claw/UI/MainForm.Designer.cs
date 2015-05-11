namespace Claw.UI
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
            this.clawButton2 = new Claw.UI.Controls.ClawButton();
            this.clawButton3 = new Claw.UI.Controls.ClawButton();
            this.SuspendLayout();
            // 
            // clawButton2
            // 
            this.clawButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.clawButton2.ForeColor = System.Drawing.Color.Red;
            this.clawButton2.Location = new System.Drawing.Point(43, 68);
            this.clawButton2.Name = "clawButton2";
            this.clawButton2.Size = new System.Drawing.Size(99, 34);
            this.clawButton2.TabIndex = 1;
            this.clawButton2.Text = "clawButton2";
            this.clawButton2.UseVisualStyleBackColor = false;
            // 
            // clawButton3
            // 
            this.clawButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.clawButton3.ForeColor = System.Drawing.Color.Red;
            this.clawButton3.Location = new System.Drawing.Point(43, 108);
            this.clawButton3.Name = "clawButton3";
            this.clawButton3.Size = new System.Drawing.Size(99, 34);
            this.clawButton3.TabIndex = 2;
            this.clawButton3.Text = "clawButton3";
            this.clawButton3.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 534);
            this.Controls.Add(this.clawButton3);
            this.Controls.Add(this.clawButton2);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Controls.SetChildIndex(this.clawButton2, 0);
            this.Controls.SetChildIndex(this.clawButton3, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ClawButton clawButton2;
        private Controls.ClawButton clawButton3;
    }
}