namespace ConverterApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();

            // Tab: Dönüştürücü
            this.tabConverter = new System.Windows.Forms.TabPage();
            this.cardFileArea = new MaterialSkin.Controls.MaterialCard();
            this.cardConverterSettings = new MaterialSkin.Controls.MaterialCard();
            this.cbQuality = new MaterialSkin.Controls.MaterialComboBox();
            this.cbResolution = new MaterialSkin.Controls.MaterialComboBox();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.btnBrowse = new MaterialSkin.Controls.MaterialButton();
            this.btnRemoveFile = new MaterialSkin.Controls.MaterialButton();
            this.cbTargetFormat = new MaterialSkin.Controls.MaterialComboBox();
            this.btnConvert = new MaterialSkin.Controls.MaterialButton();
            this.lblStatus = new MaterialSkin.Controls.MaterialLabel();
            this.progressBar = new MaterialSkin.Controls.MaterialProgressBar();

            // Tab: YouTube/Spotify
            this.tabYoutube = new System.Windows.Forms.TabPage();
            this.cardYtLink = new MaterialSkin.Controls.MaterialCard();
            this.txtUrl = new MaterialSkin.Controls.MaterialTextBox();
            this.cbYtResolution = new MaterialSkin.Controls.MaterialComboBox();
            this.cbYtFormat = new MaterialSkin.Controls.MaterialComboBox();
            this.btnDownloadYt = new MaterialSkin.Controls.MaterialButton();
            this.btnShowFileYt = new MaterialSkin.Controls.MaterialButton();
            this.lblYtStatus = new MaterialSkin.Controls.MaterialLabel();
            this.progressBarYt = new MaterialSkin.Controls.MaterialProgressBar();

            // Tab: Sosyal Medya
            this.tabSocial = new System.Windows.Forms.TabPage();
            this.cardSocialLink = new MaterialSkin.Controls.MaterialCard();
            this.txtSocialUrl = new MaterialSkin.Controls.MaterialTextBox();
            this.cbSocialResolution = new MaterialSkin.Controls.MaterialComboBox();
            this.cbSocialFormat = new MaterialSkin.Controls.MaterialComboBox();
            this.btnDownloadSocial = new MaterialSkin.Controls.MaterialButton();
            this.btnShowFileSocial = new MaterialSkin.Controls.MaterialButton();
            this.lblSocialStatus = new MaterialSkin.Controls.MaterialLabel();
            this.progressBarSocial = new MaterialSkin.Controls.MaterialProgressBar();

            // Tab: Ayarlar
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.cardSettings = new MaterialSkin.Controls.MaterialCard();
            this.txtOutputFolder = new MaterialSkin.Controls.MaterialTextBox();
            this.btnBrowseOutput = new MaterialSkin.Controls.MaterialButton();
            this.chkDarkMode = new MaterialSkin.Controls.MaterialCheckbox();
            this.cbThemeColor = new MaterialSkin.Controls.MaterialComboBox();

            this.materialTabControl1.SuspendLayout();
            this.tabConverter.SuspendLayout();
            this.tabYoutube.SuspendLayout();
            this.tabSocial.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.SuspendLayout();

            // ===== materialTabSelector1 =====
            this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTabSelector1.Location = new System.Drawing.Point(0, 0);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(800, 48);
            this.materialTabSelector1.TabIndex = 0;

            // ===== materialTabControl1 =====
            this.materialTabControl1.Controls.Add(this.tabConverter);
            this.materialTabControl1.Controls.Add(this.tabYoutube);
            this.materialTabControl1.Controls.Add(this.tabSocial);
            this.materialTabControl1.Controls.Add(this.tabSettings);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Location = new System.Drawing.Point(0, 48);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(800, 352);
            this.materialTabControl1.TabIndex = 1;

            // ================================================================
            // ==================== TAB: DÖNÜŞTÜRÜCÜ ==========================
            // ================================================================
            this.tabConverter.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.tabConverter.Controls.Add(this.cardFileArea);
            this.tabConverter.Controls.Add(this.cardConverterSettings);
            this.tabConverter.Controls.Add(this.lblStatus);
            this.tabConverter.Controls.Add(this.progressBar);
            this.tabConverter.Location = new System.Drawing.Point(4, 24);
            this.tabConverter.Name = "tabConverter";
            this.tabConverter.Padding = new System.Windows.Forms.Padding(3);
            this.tabConverter.Size = new System.Drawing.Size(792, 324);
            this.tabConverter.TabIndex = 0;
            this.tabConverter.Text = "\uE8B7 Dönüştürücü";

            // --- cardFileArea ---
            this.cardFileArea.BackColor = System.Drawing.Color.FromArgb(55, 55, 55);
            this.cardFileArea.Depth = 0;
            this.cardFileArea.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cardFileArea.Location = new System.Drawing.Point(10, 10);
            this.cardFileArea.Margin = new System.Windows.Forms.Padding(8);
            this.cardFileArea.MouseState = MaterialSkin.MouseState.HOVER;
            this.cardFileArea.Name = "cardFileArea";
            this.cardFileArea.Padding = new System.Windows.Forms.Padding(10);
            this.cardFileArea.Size = new System.Drawing.Size(760, 140);
            this.cardFileArea.TabIndex = 10;
            this.cardFileArea.Controls.Add(this.lbFiles);
            this.cardFileArea.Controls.Add(this.btnBrowse);
            this.cardFileArea.Controls.Add(this.btnRemoveFile);

            // --- lbFiles ---
            this.lbFiles.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lbFiles.ForeColor = System.Drawing.Color.White;
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.ItemHeight = 15;
            this.lbFiles.Location = new System.Drawing.Point(10, 10);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(540, 109);
            this.lbFiles.TabIndex = 0;
            this.lbFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragDrop);
            this.lbFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragEnter);
            this.lbFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFiles_KeyDown);

            // --- btnBrowse ---
            this.btnBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowse.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnBrowse.Depth = 0;
            this.btnBrowse.HighEmphasis = true;
            this.btnBrowse.Icon = null;
            this.btnBrowse.Location = new System.Drawing.Point(560, 15);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnBrowse.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnBrowse.Size = new System.Drawing.Size(185, 36);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "\uED25  Dosya Seç";
            this.btnBrowse.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnBrowse.UseAccentColor = false;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            // --- btnRemoveFile ---
            this.btnRemoveFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRemoveFile.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnRemoveFile.Depth = 0;
            this.btnRemoveFile.HighEmphasis = true;
            this.btnRemoveFile.Icon = null;
            this.btnRemoveFile.Location = new System.Drawing.Point(560, 65);
            this.btnRemoveFile.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnRemoveFile.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRemoveFile.Name = "btnRemoveFile";
            this.btnRemoveFile.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnRemoveFile.Size = new System.Drawing.Size(185, 36);
            this.btnRemoveFile.TabIndex = 2;
            this.btnRemoveFile.Text = "\uE74D  Kaldır";
            this.btnRemoveFile.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnRemoveFile.UseAccentColor = true;
            this.btnRemoveFile.UseVisualStyleBackColor = true;
            this.btnRemoveFile.Click += new System.EventHandler(this.btnRemoveFile_Click);

            // --- cardConverterSettings ---
            this.cardConverterSettings.BackColor = System.Drawing.Color.FromArgb(55, 55, 55);
            this.cardConverterSettings.Depth = 0;
            this.cardConverterSettings.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cardConverterSettings.Location = new System.Drawing.Point(10, 158);
            this.cardConverterSettings.Margin = new System.Windows.Forms.Padding(8);
            this.cardConverterSettings.MouseState = MaterialSkin.MouseState.HOVER;
            this.cardConverterSettings.Name = "cardConverterSettings";
            this.cardConverterSettings.Padding = new System.Windows.Forms.Padding(10);
            this.cardConverterSettings.Size = new System.Drawing.Size(760, 62);
            this.cardConverterSettings.TabIndex = 11;
            this.cardConverterSettings.Controls.Add(this.cbResolution);
            this.cardConverterSettings.Controls.Add(this.cbQuality);
            this.cardConverterSettings.Controls.Add(this.cbTargetFormat);
            this.cardConverterSettings.Controls.Add(this.btnConvert);

            // --- cbResolution ---
            this.cbResolution.AutoResize = false;
            this.cbResolution.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.cbResolution.Depth = 0;
            this.cbResolution.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbResolution.DropDownHeight = 174;
            this.cbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResolution.DropDownWidth = 121;
            this.cbResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbResolution.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cbResolution.FormattingEnabled = true;
            this.cbResolution.Hint = "Çözünürlük";
            this.cbResolution.IntegralHeight = false;
            this.cbResolution.ItemHeight = 43;
            this.cbResolution.Items.AddRange(new object[] { "1080p", "720p", "480p" });
            this.cbResolution.Location = new System.Drawing.Point(10, 6);
            this.cbResolution.MaxDropDownItems = 4;
            this.cbResolution.MouseState = MaterialSkin.MouseState.OUT;
            this.cbResolution.Name = "cbResolution";
            this.cbResolution.Size = new System.Drawing.Size(150, 49);
            this.cbResolution.StartIndex = 0;
            this.cbResolution.TabIndex = 7;

            // --- cbQuality ---
            this.cbQuality.AutoResize = false;
            this.cbQuality.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.cbQuality.Depth = 0;
            this.cbQuality.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbQuality.DropDownHeight = 174;
            this.cbQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbQuality.DropDownWidth = 121;
            this.cbQuality.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbQuality.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cbQuality.FormattingEnabled = true;
            this.cbQuality.Hint = "Kalite";
            this.cbQuality.IntegralHeight = false;
            this.cbQuality.ItemHeight = 43;
            this.cbQuality.Items.AddRange(new object[] { "100", "80", "60", "30" });
            this.cbQuality.Location = new System.Drawing.Point(170, 6);
            this.cbQuality.MaxDropDownItems = 4;
            this.cbQuality.MouseState = MaterialSkin.MouseState.OUT;
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.Size = new System.Drawing.Size(130, 49);
            this.cbQuality.StartIndex = 0;
            this.cbQuality.TabIndex = 8;

            // --- cbTargetFormat ---
            this.cbTargetFormat.AutoResize = false;
            this.cbTargetFormat.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.cbTargetFormat.Depth = 0;
            this.cbTargetFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbTargetFormat.DropDownHeight = 174;
            this.cbTargetFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetFormat.DropDownWidth = 121;
            this.cbTargetFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbTargetFormat.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cbTargetFormat.FormattingEnabled = true;
            this.cbTargetFormat.Hint = "Format";
            this.cbTargetFormat.IntegralHeight = false;
            this.cbTargetFormat.ItemHeight = 43;
            this.cbTargetFormat.Location = new System.Drawing.Point(310, 6);
            this.cbTargetFormat.MaxDropDownItems = 4;
            this.cbTargetFormat.MouseState = MaterialSkin.MouseState.OUT;
            this.cbTargetFormat.Name = "cbTargetFormat";
            this.cbTargetFormat.Size = new System.Drawing.Size(120, 49);
            this.cbTargetFormat.StartIndex = 0;
            this.cbTargetFormat.TabIndex = 3;

            // --- btnConvert ---
            this.btnConvert.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnConvert.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnConvert.Depth = 0;
            this.btnConvert.HighEmphasis = true;
            this.btnConvert.Icon = null;
            this.btnConvert.Location = new System.Drawing.Point(560, 12);
            this.btnConvert.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnConvert.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnConvert.Size = new System.Drawing.Size(185, 36);
            this.btnConvert.TabIndex = 4;
            this.btnConvert.Text = "\uE8AB  Dönüştür";
            this.btnConvert.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnConvert.UseAccentColor = false;
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);

            // --- lblStatus ---
            this.lblStatus.AutoSize = false;
            this.lblStatus.Depth = 0;
            this.lblStatus.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStatus.Location = new System.Drawing.Point(20, 230);
            this.lblStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(650, 20);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Durum: Bekleniyor";

            // --- progressBar ---
            this.progressBar.Depth = 0;
            this.progressBar.Location = new System.Drawing.Point(20, 260);
            this.progressBar.MouseState = MaterialSkin.MouseState.HOVER;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(740, 5);
            this.progressBar.TabIndex = 6;

            // ================================================================
            // ==================== TAB: YOUTUBE/SPOTIFY =======================
            // ================================================================
            this.tabYoutube.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.tabYoutube.Controls.Add(this.cardYtLink);
            this.tabYoutube.Controls.Add(this.lblYtStatus);
            this.tabYoutube.Controls.Add(this.progressBarYt);
            this.tabYoutube.Controls.Add(this.btnShowFileYt);
            this.tabYoutube.Location = new System.Drawing.Point(4, 24);
            this.tabYoutube.Name = "tabYoutube";
            this.tabYoutube.Padding = new System.Windows.Forms.Padding(3);
            this.tabYoutube.Size = new System.Drawing.Size(780, 298);
            this.tabYoutube.TabIndex = 1;
            this.tabYoutube.Text = "\uE774 YouTube / Spotify";

            // --- cardYtLink ---
            this.cardYtLink.BackColor = System.Drawing.Color.FromArgb(55, 55, 55);
            this.cardYtLink.Depth = 0;
            this.cardYtLink.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cardYtLink.Location = new System.Drawing.Point(10, 10);
            this.cardYtLink.Margin = new System.Windows.Forms.Padding(8);
            this.cardYtLink.MouseState = MaterialSkin.MouseState.HOVER;
            this.cardYtLink.Name = "cardYtLink";
            this.cardYtLink.Padding = new System.Windows.Forms.Padding(10);
            this.cardYtLink.Size = new System.Drawing.Size(760, 130);
            this.cardYtLink.TabIndex = 10;
            this.cardYtLink.Controls.Add(this.txtUrl);
            this.cardYtLink.Controls.Add(this.cbYtResolution);
            this.cardYtLink.Controls.Add(this.cbYtFormat);
            this.cardYtLink.Controls.Add(this.btnDownloadYt);

            // --- txtUrl ---
            this.txtUrl.AnimateReadOnly = false;
            this.txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUrl.Depth = 0;
            this.txtUrl.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtUrl.Hint = "YouTube veya Spotify Linki";
            this.txtUrl.LeadingIcon = null;
            this.txtUrl.Location = new System.Drawing.Point(10, 10);
            this.txtUrl.MaxLength = 500;
            this.txtUrl.MouseState = MaterialSkin.MouseState.OUT;
            this.txtUrl.Multiline = false;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(440, 50);
            this.txtUrl.TabIndex = 0;
            this.txtUrl.Text = "";
            this.txtUrl.TrailingIcon = null;
            this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);

            // --- cbYtResolution ---
            this.cbYtResolution.AutoResize = false;
            this.cbYtResolution.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.cbYtResolution.Depth = 0;
            this.cbYtResolution.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbYtResolution.DropDownHeight = 174;
            this.cbYtResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbYtResolution.DropDownWidth = 121;
            this.cbYtResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbYtResolution.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cbYtResolution.FormattingEnabled = true;
            this.cbYtResolution.Hint = "Çözünürlük";
            this.cbYtResolution.IntegralHeight = false;
            this.cbYtResolution.ItemHeight = 43;
            this.cbYtResolution.Items.AddRange(new object[] { "En Yuksek", "1080p", "720p", "480p", "360p" });
            this.cbYtResolution.Location = new System.Drawing.Point(460, 10);
            this.cbYtResolution.MaxDropDownItems = 4;
            this.cbYtResolution.MouseState = MaterialSkin.MouseState.OUT;
            this.cbYtResolution.Name = "cbYtResolution";
            this.cbYtResolution.Size = new System.Drawing.Size(135, 49);
            this.cbYtResolution.StartIndex = 0;
            this.cbYtResolution.TabIndex = 3;

            // --- cbYtFormat ---
            this.cbYtFormat.AutoResize = false;
            this.cbYtFormat.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.cbYtFormat.Depth = 0;
            this.cbYtFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbYtFormat.DropDownHeight = 174;
            this.cbYtFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbYtFormat.DropDownWidth = 121;
            this.cbYtFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbYtFormat.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cbYtFormat.FormattingEnabled = true;
            this.cbYtFormat.Hint = "Format";
            this.cbYtFormat.IntegralHeight = false;
            this.cbYtFormat.ItemHeight = 43;
            this.cbYtFormat.Items.AddRange(new object[] { "MP3", "MP4", "WAV" });
            this.cbYtFormat.Location = new System.Drawing.Point(605, 10);
            this.cbYtFormat.MaxDropDownItems = 4;
            this.cbYtFormat.MouseState = MaterialSkin.MouseState.OUT;
            this.cbYtFormat.Name = "cbYtFormat";
            this.cbYtFormat.Size = new System.Drawing.Size(140, 49);
            this.cbYtFormat.StartIndex = 0;
            this.cbYtFormat.TabIndex = 1;

            // --- btnDownloadYt ---
            this.btnDownloadYt.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDownloadYt.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnDownloadYt.Depth = 0;
            this.btnDownloadYt.HighEmphasis = true;
            this.btnDownloadYt.Icon = null;
            this.btnDownloadYt.Location = new System.Drawing.Point(460, 72);
            this.btnDownloadYt.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDownloadYt.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnDownloadYt.Name = "btnDownloadYt";
            this.btnDownloadYt.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnDownloadYt.Size = new System.Drawing.Size(285, 36);
            this.btnDownloadYt.TabIndex = 2;
            this.btnDownloadYt.Text = "\uE896  İndir";
            this.btnDownloadYt.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnDownloadYt.UseAccentColor = false;
            this.btnDownloadYt.UseVisualStyleBackColor = true;
            this.btnDownloadYt.Click += new System.EventHandler(this.btnDownloadYt_Click);

            // --- lblYtStatus ---
            this.lblYtStatus.AutoSize = false;
            this.lblYtStatus.Depth = 0;
            this.lblYtStatus.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblYtStatus.Location = new System.Drawing.Point(20, 160);
            this.lblYtStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblYtStatus.Name = "lblYtStatus";
            this.lblYtStatus.Size = new System.Drawing.Size(740, 40);
            this.lblYtStatus.TabIndex = 5;
            this.lblYtStatus.Text = "Durum: Bekleniyor";

            // --- progressBarYt ---
            this.progressBarYt.Depth = 0;
            this.progressBarYt.Location = new System.Drawing.Point(20, 205);
            this.progressBarYt.MouseState = MaterialSkin.MouseState.HOVER;
            this.progressBarYt.Name = "progressBarYt";
            this.progressBarYt.Size = new System.Drawing.Size(740, 5);
            this.progressBarYt.TabIndex = 6;

            // --- btnShowFileYt ---
            this.btnShowFileYt.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnShowFileYt.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnShowFileYt.Depth = 0;
            this.btnShowFileYt.HighEmphasis = false;
            this.btnShowFileYt.Icon = null;
            this.btnShowFileYt.Location = new System.Drawing.Point(20, 220);
            this.btnShowFileYt.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnShowFileYt.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnShowFileYt.Name = "btnShowFileYt";
            this.btnShowFileYt.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnShowFileYt.Size = new System.Drawing.Size(200, 36);
            this.btnShowFileYt.TabIndex = 7;
            this.btnShowFileYt.Text = "\uED25  Dosyayı Göster";
            this.btnShowFileYt.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnShowFileYt.UseAccentColor = true;
            this.btnShowFileYt.UseVisualStyleBackColor = true;
            this.btnShowFileYt.Visible = false;
            this.btnShowFileYt.Click += new System.EventHandler(this.btnShowFileYt_Click);

            // ================================================================
            // ==================== TAB: SOSYAL MEDYA =========================
            // ================================================================
            this.tabSocial.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.tabSocial.Controls.Add(this.cardSocialLink);
            this.tabSocial.Controls.Add(this.lblSocialStatus);
            this.tabSocial.Controls.Add(this.progressBarSocial);
            this.tabSocial.Controls.Add(this.btnShowFileSocial);
            this.tabSocial.Location = new System.Drawing.Point(4, 24);
            this.tabSocial.Name = "tabSocial";
            this.tabSocial.Padding = new System.Windows.Forms.Padding(3);
            this.tabSocial.Size = new System.Drawing.Size(780, 298);
            this.tabSocial.TabIndex = 2;
            this.tabSocial.Text = "\uE909 Sosyal Medya";

            // --- cardSocialLink ---
            this.cardSocialLink.BackColor = System.Drawing.Color.FromArgb(55, 55, 55);
            this.cardSocialLink.Depth = 0;
            this.cardSocialLink.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cardSocialLink.Location = new System.Drawing.Point(10, 10);
            this.cardSocialLink.Margin = new System.Windows.Forms.Padding(8);
            this.cardSocialLink.MouseState = MaterialSkin.MouseState.HOVER;
            this.cardSocialLink.Name = "cardSocialLink";
            this.cardSocialLink.Padding = new System.Windows.Forms.Padding(10);
            this.cardSocialLink.Size = new System.Drawing.Size(760, 130);
            this.cardSocialLink.TabIndex = 10;
            this.cardSocialLink.Controls.Add(this.txtSocialUrl);
            this.cardSocialLink.Controls.Add(this.cbSocialResolution);
            this.cardSocialLink.Controls.Add(this.cbSocialFormat);
            this.cardSocialLink.Controls.Add(this.btnDownloadSocial);

            // --- txtSocialUrl ---
            this.txtSocialUrl.AnimateReadOnly = false;
            this.txtSocialUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSocialUrl.Depth = 0;
            this.txtSocialUrl.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSocialUrl.Hint = "Instagram, TikTok, Twitter vb. Linki";
            this.txtSocialUrl.LeadingIcon = null;
            this.txtSocialUrl.Location = new System.Drawing.Point(10, 10);
            this.txtSocialUrl.MaxLength = 500;
            this.txtSocialUrl.MouseState = MaterialSkin.MouseState.OUT;
            this.txtSocialUrl.Multiline = false;
            this.txtSocialUrl.Name = "txtSocialUrl";
            this.txtSocialUrl.Size = new System.Drawing.Size(440, 50);
            this.txtSocialUrl.TabIndex = 0;
            this.txtSocialUrl.Text = "";
            this.txtSocialUrl.TrailingIcon = null;
            this.txtSocialUrl.TextChanged += new System.EventHandler(this.txtSocialUrl_TextChanged);

            // --- cbSocialResolution ---
            this.cbSocialResolution.AutoResize = false;
            this.cbSocialResolution.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.cbSocialResolution.Depth = 0;
            this.cbSocialResolution.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbSocialResolution.DropDownHeight = 174;
            this.cbSocialResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSocialResolution.DropDownWidth = 121;
            this.cbSocialResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbSocialResolution.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cbSocialResolution.FormattingEnabled = true;
            this.cbSocialResolution.Hint = "Çözünürlük";
            this.cbSocialResolution.IntegralHeight = false;
            this.cbSocialResolution.ItemHeight = 43;
            this.cbSocialResolution.Items.AddRange(new object[] { "En Yuksek", "1080p", "720p", "480p" });
            this.cbSocialResolution.Location = new System.Drawing.Point(460, 10);
            this.cbSocialResolution.MaxDropDownItems = 4;
            this.cbSocialResolution.MouseState = MaterialSkin.MouseState.OUT;
            this.cbSocialResolution.Name = "cbSocialResolution";
            this.cbSocialResolution.Size = new System.Drawing.Size(135, 49);
            this.cbSocialResolution.StartIndex = 0;
            this.cbSocialResolution.TabIndex = 4;

            // --- cbSocialFormat ---
            this.cbSocialFormat.AutoResize = false;
            this.cbSocialFormat.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.cbSocialFormat.Depth = 0;
            this.cbSocialFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbSocialFormat.DropDownHeight = 174;
            this.cbSocialFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSocialFormat.DropDownWidth = 121;
            this.cbSocialFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbSocialFormat.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cbSocialFormat.FormattingEnabled = true;
            this.cbSocialFormat.Hint = "Format";
            this.cbSocialFormat.IntegralHeight = false;
            this.cbSocialFormat.ItemHeight = 43;
            this.cbSocialFormat.Items.AddRange(new object[] { "MP4", "MP3", "WAV" });
            this.cbSocialFormat.Location = new System.Drawing.Point(605, 10);
            this.cbSocialFormat.MaxDropDownItems = 4;
            this.cbSocialFormat.MouseState = MaterialSkin.MouseState.OUT;
            this.cbSocialFormat.Name = "cbSocialFormat";
            this.cbSocialFormat.Size = new System.Drawing.Size(140, 49);
            this.cbSocialFormat.StartIndex = 0;
            this.cbSocialFormat.TabIndex = 5;

            // --- btnDownloadSocial ---
            this.btnDownloadSocial.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDownloadSocial.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnDownloadSocial.Depth = 0;
            this.btnDownloadSocial.HighEmphasis = true;
            this.btnDownloadSocial.Icon = null;
            this.btnDownloadSocial.Location = new System.Drawing.Point(460, 72);
            this.btnDownloadSocial.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDownloadSocial.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnDownloadSocial.Name = "btnDownloadSocial";
            this.btnDownloadSocial.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnDownloadSocial.Size = new System.Drawing.Size(285, 36);
            this.btnDownloadSocial.TabIndex = 1;
            this.btnDownloadSocial.Text = "\uE896  İndir";
            this.btnDownloadSocial.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnDownloadSocial.UseAccentColor = false;
            this.btnDownloadSocial.UseVisualStyleBackColor = true;
            this.btnDownloadSocial.Click += new System.EventHandler(this.btnDownloadSocial_Click);

            // --- lblSocialStatus ---
            this.lblSocialStatus.AutoSize = false;
            this.lblSocialStatus.Depth = 0;
            this.lblSocialStatus.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSocialStatus.Location = new System.Drawing.Point(20, 160);
            this.lblSocialStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblSocialStatus.Name = "lblSocialStatus";
            this.lblSocialStatus.Size = new System.Drawing.Size(740, 40);
            this.lblSocialStatus.TabIndex = 2;
            this.lblSocialStatus.Text = "Durum: Bekleniyor";

            // --- progressBarSocial ---
            this.progressBarSocial.Depth = 0;
            this.progressBarSocial.Location = new System.Drawing.Point(20, 205);
            this.progressBarSocial.MouseState = MaterialSkin.MouseState.HOVER;
            this.progressBarSocial.Name = "progressBarSocial";
            this.progressBarSocial.Size = new System.Drawing.Size(740, 5);
            this.progressBarSocial.TabIndex = 3;

            // --- btnShowFileSocial ---
            this.btnShowFileSocial.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnShowFileSocial.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnShowFileSocial.Depth = 0;
            this.btnShowFileSocial.HighEmphasis = false;
            this.btnShowFileSocial.Icon = null;
            this.btnShowFileSocial.Location = new System.Drawing.Point(20, 220);
            this.btnShowFileSocial.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnShowFileSocial.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnShowFileSocial.Name = "btnShowFileSocial";
            this.btnShowFileSocial.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnShowFileSocial.Size = new System.Drawing.Size(200, 36);
            this.btnShowFileSocial.TabIndex = 7;
            this.btnShowFileSocial.Text = "\uED25  Dosyayı Göster";
            this.btnShowFileSocial.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnShowFileSocial.UseAccentColor = true;
            this.btnShowFileSocial.UseVisualStyleBackColor = true;
            this.btnShowFileSocial.Visible = false;
            this.btnShowFileSocial.Click += new System.EventHandler(this.btnShowFileSocial_Click);

            // ================================================================
            // ==================== TAB: AYARLAR ==============================
            // ================================================================
            this.tabSettings.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.tabSettings.Controls.Add(this.cardSettings);
            this.tabSettings.Location = new System.Drawing.Point(4, 24);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(780, 298);
            this.tabSettings.TabIndex = 3;
            this.tabSettings.Text = "\uE713 Ayarlar";

            // --- cardSettings ---
            this.cardSettings.BackColor = System.Drawing.Color.FromArgb(55, 55, 55);
            this.cardSettings.Depth = 0;
            this.cardSettings.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cardSettings.Location = new System.Drawing.Point(10, 10);
            this.cardSettings.Margin = new System.Windows.Forms.Padding(8);
            this.cardSettings.MouseState = MaterialSkin.MouseState.HOVER;
            this.cardSettings.Name = "cardSettings";
            this.cardSettings.Padding = new System.Windows.Forms.Padding(14);
            this.cardSettings.Size = new System.Drawing.Size(760, 270);
            this.cardSettings.TabIndex = 0;
            this.cardSettings.Controls.Add(this.txtOutputFolder);
            this.cardSettings.Controls.Add(this.btnBrowseOutput);
            this.cardSettings.Controls.Add(this.chkDarkMode);
            this.cardSettings.Controls.Add(this.cbThemeColor);

            // --- txtOutputFolder ---
            this.txtOutputFolder.AnimateReadOnly = false;
            this.txtOutputFolder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOutputFolder.Depth = 0;
            this.txtOutputFolder.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtOutputFolder.Hint = "Varsayılan Çıktı Klasörü";
            this.txtOutputFolder.LeadingIcon = null;
            this.txtOutputFolder.Location = new System.Drawing.Point(14, 20);
            this.txtOutputFolder.MaxLength = 500;
            this.txtOutputFolder.MouseState = MaterialSkin.MouseState.OUT;
            this.txtOutputFolder.Multiline = false;
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(550, 50);
            this.txtOutputFolder.TabIndex = 0;
            this.txtOutputFolder.Text = "";
            this.txtOutputFolder.TrailingIcon = null;

            // --- btnBrowseOutput ---
            this.btnBrowseOutput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowseOutput.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnBrowseOutput.Depth = 0;
            this.btnBrowseOutput.HighEmphasis = true;
            this.btnBrowseOutput.Icon = null;
            this.btnBrowseOutput.Location = new System.Drawing.Point(580, 25);
            this.btnBrowseOutput.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnBrowseOutput.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnBrowseOutput.Size = new System.Drawing.Size(160, 36);
            this.btnBrowseOutput.TabIndex = 1;
            this.btnBrowseOutput.Text = "\uED25  Klasör Seç";
            this.btnBrowseOutput.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnBrowseOutput.UseAccentColor = true;
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);

            // --- chkDarkMode ---
            this.chkDarkMode.AutoSize = true;
            this.chkDarkMode.Checked = true;
            this.chkDarkMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDarkMode.Depth = 0;
            this.chkDarkMode.Location = new System.Drawing.Point(14, 90);
            this.chkDarkMode.Margin = new System.Windows.Forms.Padding(0);
            this.chkDarkMode.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkDarkMode.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkDarkMode.Name = "chkDarkMode";
            this.chkDarkMode.ReadOnly = false;
            this.chkDarkMode.Ripple = true;
            this.chkDarkMode.Size = new System.Drawing.Size(180, 37);
            this.chkDarkMode.TabIndex = 2;
            this.chkDarkMode.Text = "Koyu Tema";
            this.chkDarkMode.UseVisualStyleBackColor = true;
            this.chkDarkMode.CheckedChanged += new System.EventHandler(this.chkDarkMode_CheckedChanged);

            // --- cbThemeColor ---
            this.cbThemeColor.AutoResize = false;
            this.cbThemeColor.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.cbThemeColor.Depth = 0;
            this.cbThemeColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbThemeColor.DropDownHeight = 174;
            this.cbThemeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThemeColor.DropDownWidth = 121;
            this.cbThemeColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbThemeColor.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            this.cbThemeColor.FormattingEnabled = true;
            this.cbThemeColor.Hint = "Tema Rengi";
            this.cbThemeColor.IntegralHeight = false;
            this.cbThemeColor.ItemHeight = 43;
            this.cbThemeColor.Items.AddRange(new object[] { "Yeşil", "Mavi", "Kırmızı", "Mor", "Turuncu", "Pembe" });
            this.cbThemeColor.Location = new System.Drawing.Point(14, 140);
            this.cbThemeColor.MaxDropDownItems = 6;
            this.cbThemeColor.MouseState = MaterialSkin.MouseState.OUT;
            this.cbThemeColor.Name = "cbThemeColor";
            this.cbThemeColor.Size = new System.Drawing.Size(200, 49);
            this.cbThemeColor.StartIndex = 0;
            this.cbThemeColor.TabIndex = 3;
            this.cbThemeColor.SelectedIndexChanged += new System.EventHandler(this.cbThemeColor_SelectedIndexChanged);

            // ================================================================
            // ==================== FORM ======================================
            // ================================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.Controls.Add(this.materialTabControl1);
            this.Controls.Add(this.materialTabSelector1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Irdelix Converter";
            this.materialTabControl1.ResumeLayout(false);
            this.tabConverter.ResumeLayout(false);
            this.tabConverter.PerformLayout();
            this.tabYoutube.ResumeLayout(false);
            this.tabYoutube.PerformLayout();
            this.tabSocial.ResumeLayout(false);
            this.tabSocial.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.ResumeLayout(false);
        }

        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabConverter;
        private System.Windows.Forms.TabPage tabYoutube;
        private System.Windows.Forms.TabPage tabSocial;
        private System.Windows.Forms.TabPage tabSettings;

        // Converter Tab
        private MaterialSkin.Controls.MaterialCard cardFileArea;
        private MaterialSkin.Controls.MaterialCard cardConverterSettings;
        private System.Windows.Forms.ListBox lbFiles;
        private MaterialSkin.Controls.MaterialButton btnBrowse;
        private MaterialSkin.Controls.MaterialButton btnRemoveFile;
        private MaterialSkin.Controls.MaterialComboBox cbTargetFormat;
        private MaterialSkin.Controls.MaterialComboBox cbResolution;
        private MaterialSkin.Controls.MaterialComboBox cbQuality;
        private MaterialSkin.Controls.MaterialButton btnConvert;
        private MaterialSkin.Controls.MaterialLabel lblStatus;
        private MaterialSkin.Controls.MaterialProgressBar progressBar;

        // YouTube Tab
        private MaterialSkin.Controls.MaterialCard cardYtLink;
        private MaterialSkin.Controls.MaterialTextBox txtUrl;
        private MaterialSkin.Controls.MaterialComboBox cbYtFormat;
        private MaterialSkin.Controls.MaterialComboBox cbYtResolution;
        private MaterialSkin.Controls.MaterialButton btnDownloadYt;
        private MaterialSkin.Controls.MaterialButton btnShowFileYt;
        private MaterialSkin.Controls.MaterialLabel lblYtStatus;
        private MaterialSkin.Controls.MaterialProgressBar progressBarYt;

        // Social Tab
        private MaterialSkin.Controls.MaterialCard cardSocialLink;
        private MaterialSkin.Controls.MaterialTextBox txtSocialUrl;
        private MaterialSkin.Controls.MaterialComboBox cbSocialResolution;
        private MaterialSkin.Controls.MaterialComboBox cbSocialFormat;
        private MaterialSkin.Controls.MaterialButton btnDownloadSocial;
        private MaterialSkin.Controls.MaterialButton btnShowFileSocial;
        private MaterialSkin.Controls.MaterialLabel lblSocialStatus;
        private MaterialSkin.Controls.MaterialProgressBar progressBarSocial;

        // Settings Tab
        private MaterialSkin.Controls.MaterialCard cardSettings;
        private MaterialSkin.Controls.MaterialTextBox txtOutputFolder;
        private MaterialSkin.Controls.MaterialButton btnBrowseOutput;
        private MaterialSkin.Controls.MaterialCheckbox chkDarkMode;
        private MaterialSkin.Controls.MaterialComboBox cbThemeColor;
    }
}