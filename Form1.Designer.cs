namespace ConverterApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox lbFiles;
        private MaterialSkin.Controls.MaterialButton btnRemoveFile;
        private MaterialSkin.Controls.MaterialButton btnBrowse;
        private MaterialSkin.Controls.MaterialComboBox cbTargetFormat;
        private MaterialSkin.Controls.MaterialButton btnConvert;
        private MaterialSkin.Controls.MaterialLabel lblYtHeader;
        private MaterialSkin.Controls.MaterialDivider dividerYt;
        private MaterialSkin.Controls.MaterialTextBox txtUrl;
        private MaterialSkin.Controls.MaterialComboBox cbYtFormat;
        private MaterialSkin.Controls.MaterialButton btnDownloadYt;
        private System.Windows.Forms.ProgressBar progressBar;
        private MaterialSkin.Controls.MaterialLabel lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lbFiles = new System.Windows.Forms.ListBox();
            btnRemoveFile = new MaterialSkin.Controls.MaterialButton();
            btnBrowse = new MaterialSkin.Controls.MaterialButton();
            cbTargetFormat = new MaterialSkin.Controls.MaterialComboBox();
            btnConvert = new MaterialSkin.Controls.MaterialButton();
            lblYtHeader = new MaterialSkin.Controls.MaterialLabel();
            dividerYt = new MaterialSkin.Controls.MaterialDivider();
            txtUrl = new MaterialSkin.Controls.MaterialTextBox();
            cbYtFormat = new MaterialSkin.Controls.MaterialComboBox();
            btnDownloadYt = new MaterialSkin.Controls.MaterialButton();
            progressBar = new System.Windows.Forms.ProgressBar();
            lblStatus = new MaterialSkin.Controls.MaterialLabel();
            SuspendLayout();
            // 
            // lbFiles
            // 
            lbFiles.AllowDrop = true;
            lbFiles.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            lbFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lbFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            lbFiles.ForeColor = System.Drawing.Color.White;
            lbFiles.FormattingEnabled = true;
            lbFiles.Location = new System.Drawing.Point(12, 80);
            lbFiles.Name = "lbFiles";
            lbFiles.Size = new System.Drawing.Size(890, 112);
            lbFiles.TabIndex = 9;
            lbFiles.SelectedIndexChanged += lbFiles_SelectedIndexChanged;
            lbFiles.DragDrop += lbFiles_DragDrop;
            lbFiles.DragEnter += lbFiles_DragEnter;
            // 
            // btnRemoveFile
            // 
            btnRemoveFile.AutoSize = false;
            btnRemoveFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnRemoveFile.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnRemoveFile.Depth = 0;
            btnRemoveFile.HighEmphasis = true;
            btnRemoveFile.Icon = null;
            btnRemoveFile.Location = new System.Drawing.Point(928, 80);
            btnRemoveFile.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btnRemoveFile.MouseState = MaterialSkin.MouseState.HOVER;
            btnRemoveFile.Name = "btnRemoveFile";
            btnRemoveFile.NoAccentTextColor = System.Drawing.Color.Empty;
            btnRemoveFile.Size = new System.Drawing.Size(75, 36);
            btnRemoveFile.TabIndex = 8;
            btnRemoveFile.Text = "SIL (X)";
            btnRemoveFile.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnRemoveFile.UseAccentColor = false;
            btnRemoveFile.Click += btnRemoveFile_Click;
            // 
            // btnBrowse
            // 
            btnBrowse.AutoSize = false;
            btnBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnBrowse.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnBrowse.Depth = 0;
            btnBrowse.HighEmphasis = true;
            btnBrowse.Icon = null;
            btnBrowse.Location = new System.Drawing.Point(12, 201);
            btnBrowse.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btnBrowse.MouseState = MaterialSkin.MouseState.HOVER;
            btnBrowse.Name = "btnBrowse";
            btnBrowse.NoAccentTextColor = System.Drawing.Color.Empty;
            btnBrowse.Size = new System.Drawing.Size(110, 49);
            btnBrowse.TabIndex = 7;
            btnBrowse.Text = "DOSYA SEC";
            btnBrowse.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnBrowse.UseAccentColor = false;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // cbTargetFormat
            // 
            cbTargetFormat.AutoResize = false;
            cbTargetFormat.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)255)), ((int)((byte)255)));
            cbTargetFormat.Depth = 0;
            cbTargetFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cbTargetFormat.DropDownHeight = 174;
            cbTargetFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbTargetFormat.DropDownWidth = 121;
            cbTargetFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            cbTargetFormat.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)222)), ((int)((byte)0)), ((int)((byte)0)), ((int)((byte)0)));
            cbTargetFormat.IntegralHeight = false;
            cbTargetFormat.ItemHeight = 43;
            cbTargetFormat.Location = new System.Drawing.Point(695, 201);
            cbTargetFormat.MaxDropDownItems = 4;
            cbTargetFormat.MouseState = MaterialSkin.MouseState.OUT;
            cbTargetFormat.Name = "cbTargetFormat";
            cbTargetFormat.Size = new System.Drawing.Size(100, 49);
            cbTargetFormat.StartIndex = 0;
            cbTargetFormat.TabIndex = 6;
            // 
            // btnConvert
            // 
            btnConvert.AutoSize = false;
            btnConvert.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnConvert.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnConvert.Depth = 0;
            btnConvert.HighEmphasis = true;
            btnConvert.Icon = null;
            btnConvert.Location = new System.Drawing.Point(802, 202);
            btnConvert.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btnConvert.MouseState = MaterialSkin.MouseState.HOVER;
            btnConvert.Name = "btnConvert";
            btnConvert.NoAccentTextColor = System.Drawing.Color.Empty;
            btnConvert.Size = new System.Drawing.Size(100, 49);
            btnConvert.TabIndex = 5;
            btnConvert.Text = "CEVIR";
            btnConvert.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnConvert.UseAccentColor = false;
            btnConvert.Click += btnConvert_Click;
            // 
            // lblYtHeader
            // 
            lblYtHeader.AutoSize = true;
            lblYtHeader.Depth = 0;
            lblYtHeader.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            lblYtHeader.Location = new System.Drawing.Point(12, 275);
            lblYtHeader.MouseState = MaterialSkin.MouseState.HOVER;
            lblYtHeader.Name = "lblYtHeader";
            lblYtHeader.Size = new System.Drawing.Size(191, 19);
            lblYtHeader.TabIndex = 10;
            lblYtHeader.Text = "YouTube ve Spotify İndirici";
            // 
            // dividerYt
            // 
            dividerYt.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            dividerYt.Depth = 0;
            dividerYt.Location = new System.Drawing.Point(210, 285);
            dividerYt.MouseState = MaterialSkin.MouseState.HOVER;
            dividerYt.Name = "dividerYt";
            dividerYt.Size = new System.Drawing.Size(793, 2);
            dividerYt.TabIndex = 11;
            // 
            // txtUrl
            // 
            txtUrl.AnimateReadOnly = false;
            txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtUrl.Depth = 0;
            txtUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            txtUrl.Hint = "YouTube veya Spotify Linki";
            txtUrl.LeadingIcon = null;
            txtUrl.Location = new System.Drawing.Point(12, 310);
            txtUrl.MaxLength = 500;
            txtUrl.MouseState = MaterialSkin.MouseState.OUT;
            txtUrl.Multiline = false;
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new System.Drawing.Size(890, 50);
            txtUrl.TabIndex = 4;
            txtUrl.Text = "";
            txtUrl.TrailingIcon = null;
            // 
            // cbYtFormat
            // 
            cbYtFormat.AutoResize = false;
            cbYtFormat.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)255)), ((int)((byte)255)));
            cbYtFormat.Depth = 0;
            cbYtFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cbYtFormat.DropDownHeight = 174;
            cbYtFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbYtFormat.DropDownWidth = 121;
            cbYtFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            cbYtFormat.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)222)), ((int)((byte)0)), ((int)((byte)0)), ((int)((byte)0)));
            cbYtFormat.IntegralHeight = false;
            cbYtFormat.ItemHeight = 43;
            cbYtFormat.Items.AddRange(new object[] { "MP3", "MP4" });
            cbYtFormat.Location = new System.Drawing.Point(913, 309);
            cbYtFormat.MaxDropDownItems = 4;
            cbYtFormat.MouseState = MaterialSkin.MouseState.OUT;
            cbYtFormat.Name = "cbYtFormat";
            cbYtFormat.Size = new System.Drawing.Size(90, 49);
            cbYtFormat.StartIndex = 0;
            cbYtFormat.TabIndex = 3;
            // 
            // btnDownloadYt
            // 
            btnDownloadYt.AutoSize = false;
            btnDownloadYt.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnDownloadYt.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnDownloadYt.Depth = 0;
            btnDownloadYt.HighEmphasis = true;
            btnDownloadYt.Icon = null;
            btnDownloadYt.Location = new System.Drawing.Point(878, 394);
            btnDownloadYt.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btnDownloadYt.MouseState = MaterialSkin.MouseState.HOVER;
            btnDownloadYt.Name = "btnDownloadYt";
            btnDownloadYt.NoAccentTextColor = System.Drawing.Color.Empty;
            btnDownloadYt.Size = new System.Drawing.Size(125, 36);
            btnDownloadYt.TabIndex = 2;
            btnDownloadYt.Text = "INDIR";
            btnDownloadYt.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnDownloadYt.UseAccentColor = false;
            btnDownloadYt.Click += btnDownloadYt_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new System.Drawing.Point(12, 375);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(991, 12);
            progressBar.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Depth = 0;
            lblStatus.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            lblStatus.Location = new System.Drawing.Point(12, 400);
            lblStatus.MouseState = MaterialSkin.MouseState.HOVER;
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(113, 19);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Durum: Bekliyor";
            // 
            // Form1
            // 
            ClientSize = new System.Drawing.Size(1021, 450);
            Controls.Add(lblStatus);
            Controls.Add(progressBar);
            Controls.Add(btnDownloadYt);
            Controls.Add(cbYtFormat);
            Controls.Add(txtUrl);
            Controls.Add(dividerYt);
            Controls.Add(lblYtHeader);
            Controls.Add(btnConvert);
            Controls.Add(cbTargetFormat);
            Controls.Add(btnBrowse);
            Controls.Add(btnRemoveFile);
            Controls.Add(lbFiles);
            Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
            MaximizeBox = false;
            Sizable = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "irdelix Converter";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}