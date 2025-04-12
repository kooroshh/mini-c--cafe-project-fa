namespace Cafe.Products
{
    partial class AddOrEditProduct
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPPrice = new System.Windows.Forms.NumericUpDown();
            this.txtPName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckPStatusTrue = new System.Windows.Forms.RadioButton();
            this.ckPStatusFalse = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPPrice)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPName);
            this.groupBox1.Controls.Add(this.txtPPrice);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 248);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "اطلاعات محصول";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(346, 268);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(123, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "ثبت";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(362, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "نام محصول :";
            // 
            // txtPPrice
            // 
            this.txtPPrice.Location = new System.Drawing.Point(27, 84);
            this.txtPPrice.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.txtPPrice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPPrice.Name = "txtPPrice";
            this.txtPPrice.Size = new System.Drawing.Size(324, 23);
            this.txtPPrice.TabIndex = 1;
            this.txtPPrice.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtPName
            // 
            this.txtPName.Location = new System.Drawing.Point(27, 51);
            this.txtPName.Name = "txtPName";
            this.txtPName.Size = new System.Drawing.Size(324, 23);
            this.txtPName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "قیمت محصول :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ckPStatusFalse);
            this.groupBox2.Controls.Add(this.ckPStatusTrue);
            this.groupBox2.Location = new System.Drawing.Point(27, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(422, 115);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "وضعیت";
            // 
            // ckPStatusTrue
            // 
            this.ckPStatusTrue.AutoSize = true;
            this.ckPStatusTrue.Checked = true;
            this.ckPStatusTrue.Location = new System.Drawing.Point(316, 32);
            this.ckPStatusTrue.Name = "ckPStatusTrue";
            this.ckPStatusTrue.Size = new System.Drawing.Size(62, 20);
            this.ckPStatusTrue.TabIndex = 0;
            this.ckPStatusTrue.TabStop = true;
            this.ckPStatusTrue.Text = "موجود";
            this.ckPStatusTrue.UseVisualStyleBackColor = true;
            // 
            // ckPStatusFalse
            // 
            this.ckPStatusFalse.AutoSize = true;
            this.ckPStatusFalse.Location = new System.Drawing.Point(308, 58);
            this.ckPStatusFalse.Name = "ckPStatusFalse";
            this.ckPStatusFalse.Size = new System.Drawing.Size(70, 20);
            this.ckPStatusFalse.TabIndex = 1;
            this.ckPStatusFalse.Text = "ناموجود";
            this.ckPStatusFalse.UseVisualStyleBackColor = true;
            // 
            // AddOrEditProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 303);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddOrEditProduct";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "افزودن محصول";
            this.Load += new System.EventHandler(this.AddOrEditProduct_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPPrice)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown txtPPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton ckPStatusFalse;
        private System.Windows.Forms.RadioButton ckPStatusTrue;
    }
}