using System;
using System.IO;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using NReco.VideoConverter;
using ImageMagick;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using YoutubeDLSharp.Helpers;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ConverterApp
{
    public class ComboBoxItem
    {
        public string Text { get; set; } = string.Empty;
        public int Value { get; set; }
        public override string ToString() { return Text; }
    }

    public partial class Form1 : Form
    {
        // ====== DWM Immersive Dark Mode ======
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        // ====== State ======
        private string _lastDownloadedPathYt = "";
        private string _lastDownloadedPathSocial = "";
        private string _lastClipboardUrl = "";
        private PrivateFontCollection privateFonts = new PrivateFontCollection();
        private Font? montserratRegular;
        private Font? montserratBold;

        public Form1()
        {
            InitializeComponent();

            lbFiles.AllowDrop = true;
            cbYtFormat.SelectedIndex = 1;
            ResetResolutions();

            btnDownloadYt.Enabled = false;
            btnDownloadSocial.Enabled = false;

            LoadMontserratFont();
            ApplySegoeIcons();

            this.Activated += Form1_Activated;

            cbSocialFormat.SelectedIndex = 0;
            cbThemeColor.SelectedIndex = 0;

            cbYtResolution.Items.Clear();
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "En Yuksek", Value = 0 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "1080p", Value = 1080 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "720p", Value = 720 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "480p", Value = 480 });
            cbYtResolution.SelectedIndex = 0;

            cbSocialResolution.Items.Clear();
            cbSocialResolution.Items.Add(new ComboBoxItem { Text = "En Yuksek", Value = 0 });
            cbSocialResolution.Items.Add(new ComboBoxItem { Text = "1080p", Value = 1080 });
            cbSocialResolution.Items.Add(new ComboBoxItem { Text = "720p", Value = 720 });
            cbSocialResolution.Items.Add(new ComboBoxItem { Text = "480p", Value = 480 });
            cbSocialResolution.SelectedIndex = 0;

            btnDownloadYt.Enabled = false;
            btnDownloadSocial.Enabled = false;

            lbFiles.SelectionMode = SelectionMode.MultiExtended;
            lbFiles.KeyDown += lbFiles_KeyDown;

            cbYtFormat.SelectedIndexChanged += (s, ev) =>
            {
                if (cbYtFormat.SelectedItem != null && (cbYtFormat.SelectedItem.ToString() == "MP3" || cbYtFormat.SelectedItem.ToString() == "WAV"))
                {
                    cbYtResolution.Enabled = false;
                }
                else
                {
                    cbYtResolution.Enabled = true;
                }
            };

            cbTargetFormat.SelectedIndexChanged += (s, ev) =>
            {
                if (cbTargetFormat.SelectedItem == null) return;

                string format = cbTargetFormat.SelectedItem?.ToString()?.ToLower() ?? "";

                if (format == "mp4")
                {
                    cbResolution.Enabled = true;
                    cbQuality.Enabled = false;
                }
                else if (format == "jpg")
                {
                    cbResolution.Enabled = false;
                    cbQuality.Enabled = true;
                }
                else
                {
                    cbResolution.Enabled = false;
                    cbQuality.Enabled = false;
                }
            };

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Green600, Primary.Green700, Primary.Green200, Accent.Green400, TextShade.WHITE
            );

            // Apply immersive dark mode at startup
            SetImmersiveDarkMode(true);
            this.BackColor = Color.FromArgb(50, 50, 50);

            // Auto resize and word wrap for status labels
            lblStatus.AutoSize = true;
            lblStatus.MaximumSize = new System.Drawing.Size(650, 0);
            lblStatus.SizeChanged += (s, ev) => progressBar.Top = lblStatus.Bottom + 10;

            lblYtStatus.AutoSize = true;
            lblYtStatus.MaximumSize = new System.Drawing.Size(740, 0);
            lblYtStatus.SizeChanged += (s, ev) => progressBarYt.Top = lblYtStatus.Bottom + 10;

            lblSocialStatus.AutoSize = true;
            lblSocialStatus.MaximumSize = new System.Drawing.Size(740, 0);
            lblSocialStatus.SizeChanged += (s, ev) => progressBarSocial.Top = lblSocialStatus.Bottom + 10;

            // Default output folder
            txtOutputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        }

        // ====================================================================
        // ======================== DARK MODE ==================================
        // ====================================================================

        private void SetImmersiveDarkMode(bool enabled)
        {
            try
            {
                int value = enabled ? 1 : 0;
                DwmSetWindowAttribute(this.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
            }
            catch { }
        }

        private void chkDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            var mgr = MaterialSkinManager.Instance;
            if (chkDarkMode.Checked)
            {
                mgr.Theme = MaterialSkinManager.Themes.DARK;
                SetImmersiveDarkMode(true);
                this.BackColor = Color.FromArgb(50, 50, 50);
                ApplyTabColors(Color.FromArgb(50, 50, 50), Color.FromArgb(55, 55, 55));
            }
            else
            {
                mgr.Theme = MaterialSkinManager.Themes.LIGHT;
                SetImmersiveDarkMode(false);
                this.BackColor = Color.FromArgb(245, 245, 245);
                ApplyTabColors(Color.FromArgb(245, 245, 245), Color.FromArgb(255, 255, 255));
            }
            UpdateDynamicIconColors();
        }

        private void ApplyTabColors(Color tabBg, Color cardBg)
        {
            foreach (TabPage tab in materialTabControl1.TabPages)
            {
                tab.BackColor = tabBg;
            }
            // Update card backgrounds
            foreach (Control c in this.Controls)
            {
                UpdateCardColors(c, cardBg);
            }
        }

        private void UpdateCardColors(Control parent, Color cardBg)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is MaterialSkin.Controls.MaterialCard card)
                {
                    card.BackColor = cardBg;
                }
                if (c.HasChildren)
                {
                    UpdateCardColors(c, cardBg);
                }
            }
        }

        private void cbThemeColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mgr = MaterialSkinManager.Instance;
            string choice = cbThemeColor.SelectedItem?.ToString() ?? "Yeşil";

            switch (choice)
            {
                case "Mavi":
                    mgr.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue200, Accent.Blue400, TextShade.WHITE);
                    break;
                case "Kırmızı":
                    mgr.ColorScheme = new ColorScheme(Primary.Red600, Primary.Red700, Primary.Red200, Accent.Red400, TextShade.WHITE);
                    break;
                case "Mor":
                    mgr.ColorScheme = new ColorScheme(Primary.Purple600, Primary.Purple700, Primary.Purple200, Accent.Purple400, TextShade.WHITE);
                    break;
                case "Turuncu":
                    mgr.ColorScheme = new ColorScheme(Primary.Orange600, Primary.Orange700, Primary.Orange200, Accent.Orange400, TextShade.WHITE);
                    break;
                case "Pembe":
                    mgr.ColorScheme = new ColorScheme(Primary.Pink600, Primary.Pink700, Primary.Pink200, Accent.Pink400, TextShade.WHITE);
                    break;
                default:
                    mgr.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Green400, TextShade.WHITE);
                    break;
            }
        }

        // ====================================================================
        // =================== SEGOE MDL2 ICONS ===============================
        // ====================================================================

        private void ApplySegoeIcons()
        {
            try
            {
                var segoeFont = new Font("Segoe MDL2 Assets", 10f);
                // Icons are embedded in button Text via Unicode chars in Designer
                // This method ensures ListBox and other non-Material controls also get icon-friendly fonts
                lbFiles.Font = new Font("Segoe UI", 9.5f);
            }
            catch { }
        }

        private void UpdateDynamicIconColors()
        {
            bool isDark = MaterialSkinManager.Instance.Theme == MaterialSkinManager.Themes.DARK;
            Color iconColor = isDark ? Color.White : Color.FromArgb(50, 50, 50);

            // MaterialSkin handles button text color automatically based on theme
            // But we ensure the ListBox matches
            lbFiles.BackColor = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(240, 240, 240);
            lbFiles.ForeColor = isDark ? Color.White : Color.Black;
        }

        // ====================================================================
        // =================== FONTS ==========================================
        // ====================================================================

        private void LoadMontserratFont()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string regPath = Path.Combine(baseDir, "Resources", "Montserrat-Regular.ttf");
                string boldPath = Path.Combine(baseDir, "Resources", "Montserrat-Bold.ttf");

                if (File.Exists(regPath)) privateFonts.AddFontFile(regPath);
                if (File.Exists(boldPath)) privateFonts.AddFontFile(boldPath);

                if (privateFonts.Families.Length > 0)
                {
                    montserratRegular = new Font(privateFonts.Families[0], 10f, FontStyle.Regular);
                    montserratBold = new Font(privateFonts.Families[0], 10f, FontStyle.Bold);
                    ApplyFontToControls(this, montserratRegular);
                }
            }
            catch { }
        }

        private void ApplyFontToControls(Control parent, Font font)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is MaterialTextBox || c is MaterialComboBox ||
                    c is MaterialButton || c is MaterialLabel || c is MaterialCheckbox)
                {
                    c.Font = font;
                }
                if (c.HasChildren)
                {
                    ApplyFontToControls(c, font);
                }
            }
        }

        // ====================================================================
        // =================== TOAST NOTIFICATION =============================
        // ====================================================================

        private void ShowToastNotification(string title, string message)
        {
            try
            {
                var notification = new NotifyIcon()
                {
                    Visible = true,
                    Icon = SystemIcons.Information,
                    BalloonTipTitle = title,
                    BalloonTipText = message
                };
                notification.ShowBalloonTip(3000);
                // Dispose after a delay to ensure balloon shows
                var timer = new System.Windows.Forms.Timer { Interval = 5000 };
                timer.Tick += (s, e) => { notification.Dispose(); timer.Stop(); timer.Dispose(); };
                timer.Start();
            }
            catch { }
        }

        // ====================================================================
        // =================== SHOW FILE IN EXPLORER ==========================
        // ====================================================================

        private void btnShowFileYt_Click(object sender, EventArgs e)
        {
            OpenFileInExplorer(_lastDownloadedPathYt);
        }

        private void btnShowFileSocial_Click(object sender, EventArgs e)
        {
            OpenFileInExplorer(_lastDownloadedPathSocial);
        }

        private void OpenFileInExplorer(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && (File.Exists(path) || Directory.Exists(path)))
                {
                    if (File.Exists(path))
                    {
                        Process.Start("explorer.exe", $"/select,\"{path}\"");
                    }
                    else
                    {
                        Process.Start("explorer.exe", $"\"{path}\"");
                    }
                }
            }
            catch { }
        }

        // ====================================================================
        // =================== SETTINGS =======================================
        // ====================================================================

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtOutputFolder.Text) && Directory.Exists(txtOutputFolder.Text))
            {
                fbd.SelectedPath = txtOutputFolder.Text;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = fbd.SelectedPath;
            }
        }

        // ====================================================================
        // =================== RESOLUTIONS ====================================
        // ====================================================================

        private void ResetResolutions()
        {
            cbYtResolution.Items.Clear();
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "En Yuksek", Value = 0 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "1080p", Value = 1080 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "720p", Value = 720 });
            cbYtResolution.SelectedIndex = 0;

            cbResolution.Items.Clear();
            cbResolution.Items.Add(new ComboBoxItem { Text = "Orijinal", Value = 0 });
            cbResolution.Items.Add(new ComboBoxItem { Text = "1080p", Value = 1080 });
            cbResolution.Items.Add(new ComboBoxItem { Text = "720p", Value = 720 });
            cbResolution.Items.Add(new ComboBoxItem { Text = "480p", Value = 480 });
            cbResolution.SelectedIndex = 0;
        }

        // ====================================================================
        // =================== FILE LIST MANAGEMENT ===========================
        // ====================================================================

        private void UpdateTargetFormats()
        {
            if (lbFiles.Items.Count == 0)
            {
                cbTargetFormat.Items.Clear();
                return;
            }

            bool hasVideoOrAudio = false;
            bool hasImage = false;

            foreach (string file in lbFiles.Items)
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".mp4" || ext == ".avi" || ext == ".mkv" || ext == ".mp3" || ext == ".wav")
                    hasVideoOrAudio = true;
                else if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".gif" || ext == ".webp")
                    hasImage = true;
            }

            string? previousSelection = cbTargetFormat.SelectedItem?.ToString();
            cbTargetFormat.Items.Clear();

            if (hasImage && !hasVideoOrAudio)
            {
                cbTargetFormat.Items.AddRange(new object[] { "png", "jpg", "ico", "bmp", "gif" });
            }
            else if (hasVideoOrAudio && !hasImage)
            {
                cbTargetFormat.Items.AddRange(new object[] { "mp3", "wav", "mp4" });
            }
            else if (hasImage && hasVideoOrAudio)
            {
                cbTargetFormat.Items.AddRange(new object[] { "png", "jpg", "ico", "bmp", "gif", "mp3", "wav", "mp4" });
            }

            if (cbTargetFormat.Items.Count > 0)
            {
                if (previousSelection != null && cbTargetFormat.Items.Contains(previousSelection))
                    cbTargetFormat.SelectedItem = previousSelection;
                else
                    cbTargetFormat.SelectedIndex = 0;
            }
        }

        private void AddFilesFiltered(string[] newFiles)
        {
            bool hasMedia = false;
            bool hasImage = false;

            foreach (string existingFile in lbFiles.Items)
            {
                string ext = Path.GetExtension(existingFile).ToLower();
                if (ext == ".mp4" || ext == ".avi" || ext == ".mkv" || ext == ".mp3" || ext == ".wav")
                    hasMedia = true;
                else if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".gif" || ext == ".webp")
                    hasImage = true;
            }

            bool warningShown = false;

            foreach (string file in newFiles)
            {
                string ext = Path.GetExtension(file).ToLower();
                bool isMedia = (ext == ".mp4" || ext == ".avi" || ext == ".mkv" || ext == ".mp3" || ext == ".wav");
                bool isImage = (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".gif" || ext == ".webp");

                if (!isMedia && !isImage) continue;

                if (hasMedia && isImage)
                {
                    if (!warningShown)
                    {
                        MessageBox.Show("Listede zaten ses/video dosyasi var. Resim dosyasi ekleyemezsiniz.", "Tur Uyumsuzlugu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        warningShown = true;
                    }
                    continue;
                }

                if (hasImage && isMedia)
                {
                    if (!warningShown)
                    {
                        MessageBox.Show("Listede zaten resim dosyasi var. Ses/video dosyasi ekleyemezsiniz.", "Tur Uyumsuzlugu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        warningShown = true;
                    }
                    continue;
                }

                lbFiles.Items.Add(file);
                if (isMedia) hasMedia = true;
                if (isImage) hasImage = true;
            }

            UpdateTargetFormats();
        }

        private void lbFiles_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void lbFiles_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetData(DataFormats.FileDrop) is string[] files)
            {
                AddFilesFiltered(files);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                AddFilesFiltered(ofd.FileNames);
            }
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (lbFiles.SelectedIndices.Count > 0)
            {
                for (int i = lbFiles.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    lbFiles.Items.RemoveAt(lbFiles.SelectedIndices[i]);
                }
                UpdateTargetFormats();
            }
        }

        private void lbFiles_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                btnRemoveFile_Click(sender ?? this, e);
            }
        }

        // ====================================================================
        // =================== CONVERTER ======================================
        // ====================================================================

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count == 0 || cbTargetFormat.SelectedItem == null) return;

            btnConvert.Enabled = false;
            try
            {
                string targetExt = cbTargetFormat.SelectedItem?.ToString() ?? "";
                int total = lbFiles.Items.Count;
                int current = 0;

                foreach (string file in lbFiles.Items)
                {
                    current++;
                    lblStatus.Text = $"Cevriliyor: {current} / {total}";
                    progressBar.Value = 0;

                    string dir = Path.GetDirectoryName(file) ?? "";
                    string name = Path.GetFileNameWithoutExtension(file);
                    string targetFile = Path.Combine(dir, name + "_trim." + targetExt);

                    bool isVideoOrAudioTarget = "mp3,wav,mp4".Contains(targetExt.ToLower());
                    bool isImageTarget = "png,jpg,ico,bmp,gif".Contains(targetExt.ToLower());

                    if (isVideoOrAudioTarget)
                    {
                        await Task.Run(() =>
                        {
                            var convert = new FFMpegConverter();
                            var settings = new ConvertSettings();

                            if (targetExt == "mp4" && cbResolution.SelectedItem != null)
                            {
                                var res = (ComboBoxItem)cbResolution.SelectedItem;
                                if (res.Value == 1080) settings.VideoFrameSize = "1920x1080";
                                else if (res.Value == 720) settings.VideoFrameSize = "1280x720";
                            }

                            convert.ConvertMedia(file, null, targetFile, targetExt, settings);
                        });
                    }
                    else if (isImageTarget)
                    {
                        await Task.Run(() =>
                        {
                            using (var image = new MagickImage(file))
                            {
                                // Resim Çözünürlüğü Değiştirme (Compress/Enhance)
                                if (cbResolution.SelectedItem != null)
                                {
                                    var res = (ComboBoxItem)cbResolution.SelectedItem;
                                    if (res.Value > 0)
                                    {
                                        // Orantılı boyutlandırma (Genişliği yükseklik bazlı ayarlar)
                                        image.Resize(0, (uint)res.Value);
                                    }
                                }

                                if (targetExt == "jpg")
                                {
                                    uint q = 100;
                                    Invoke(new Action(() => { uint.TryParse(cbQuality.SelectedItem?.ToString(), out q); }));
                                    image.Quality = q;
                                }
                                image.Write(targetFile);
                            }
                        });
                    }
                    progressBar.Value = 100;
                }
                lblStatus.Text = "Islem tamam: _trim dosyaları olusturuldu.";
                ShowToastNotification("Dönüştürme Tamamlandı", $"{total} dosya başarıyla dönüştürüldü!");
            }
            finally
            {
                progressBar.Value = 0;
                btnConvert.Enabled = true;
            }
        }


        private string Temizle(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Isimsiz";

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            name = name.Trim();

            if (name.Length > 80)
            {
                name = name.Substring(0, 80).Trim();
            }

            return name;
        }

        private string FormatBytes(long bytes)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB" };
            if (bytes == 0) return "0 B";
            long bytesAbs = Math.Abs(bytes);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytesAbs, 1024)));
            if (place >= suf.Length) place = suf.Length - 1;
            double num = Math.Round(bytesAbs / Math.Pow(1024, place), 1);
            return (Math.Sign(bytes) * num).ToString() + " " + suf[place];
        }

        private string FormatETA(long bytesPerSec, long remainingBytes)
        {
            if (bytesPerSec <= 0) return "∞";
            long seconds = remainingBytes / bytesPerSec;
            if (seconds < 60) return $"{seconds}sn";
            if (seconds < 3600) return $"{seconds / 60}dk {seconds % 60}sn";
            return $"{seconds / 3600}sa {(seconds % 3600) / 60}dk";
        }

        private int ParseResolutionHeight(string resString)
        {
            if (resString.Contains("Dikey") && resString.Contains("x"))
            {
                var match = Regex.Match(resString, @"x(\d+)");
                if (match.Success)
                {
                    return int.Parse(match.Groups[1].Value);
                }
            }
            return int.Parse(Regex.Match(resString, @"^\d+").Value);
        }

        // ====================================================================
        // =================== YOUTUBE / SPOTIFY ==============================
        // ====================================================================

        private string lastYtUrl = "";
        private async void txtUrl_TextChanged(object sender, EventArgs e)
        {
            string currentUrl = txtUrl.Text;
            btnDownloadYt.Enabled = false;

            if (string.IsNullOrWhiteSpace(currentUrl) || !currentUrl.StartsWith("http")) return;

            // Smart format: auto-select MP3 for music links
            if (currentUrl.Contains("spotify.com") || currentUrl.Contains("music.youtube"))
            {
                cbYtFormat.SelectedItem = "MP3";
            }

            if (currentUrl.Contains("spotify") || currentUrl.Contains("playlist") || currentUrl.Contains("list=") || currentUrl.Contains("album"))
            {
                btnDownloadYt.Enabled = true;
                return;
            }

            if (currentUrl == lastYtUrl) return;
            lastYtUrl = currentUrl;

            await Task.Delay(800);

            if (txtUrl.Text != currentUrl) return;

            await FetchYoutubeResolutionsAsync(currentUrl);
        }

        private async Task FetchYoutubeResolutionsAsync(string url)
        {
            lblYtStatus.Text = "Çözünürlükler otomatik çekiliyor...";
            cbYtResolution.Enabled = false;

            try
            {
                var youtube = new YoutubeClient();
                var manifest = await youtube.Videos.Streams.GetManifestAsync(url);

                cbYtResolution.Items.Clear();
                cbYtResolution.Items.Add(new ComboBoxItem { Text = "En Yüksek", Value = 0 });

                var resList = manifest.GetVideoOnlyStreams()
                    .GroupBy(s => s.VideoResolution.Height)
                    .Select(g => g.First().VideoResolution)
                    .OrderByDescending(r => r.Height);

                foreach (var r in resList)
                {
                    if (r.Height > r.Width)
                    {
                        cbYtResolution.Items.Add(new ComboBoxItem { Text = $"{r.Width}p", Value = r.Height });
                    }
                    else
                    {
                        cbYtResolution.Items.Add(new ComboBoxItem { Text = $"{r.Height}p", Value = r.Height });
                    }
                }

                cbYtResolution.SelectedIndex = 0;
                lblYtStatus.Text = "Çözünürlük listesi hazır!";
                btnDownloadYt.Enabled = true;
            }
            catch
            {
                lblYtStatus.Text = "Çözünürlük alınamadı, varsayılanlarla devam edilecek.";
                ResetYoutubeResolutions();
                btnDownloadYt.Enabled = true;
            }
            finally
            {
                cbYtResolution.Enabled = true;
            }
        }

        private void ResetYoutubeResolutions()
        {
            cbYtResolution.Items.Clear();
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "En Yuksek", Value = 0 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "1080p", Value = 1080 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "720p", Value = 720 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "480p", Value = 480 });
            cbYtResolution.SelectedIndex = 0;
        }

        private async void btnDownloadYt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrl.Text)) return;

            btnDownloadYt.Enabled = false;
            btnShowFileYt.Visible = false;
            lblYtStatus.Text = "Bilgiler alınıyor...";
            var youtube = new YoutubeClient();
            string url = txtUrl.Text.Trim();
            string format = cbYtFormat.SelectedItem?.ToString() ?? "MP4";

            var resItem = cbYtResolution.SelectedItem as ComboBoxItem ?? new ComboBoxItem { Text = "En Yüksek", Value = 0 };

            try
            {
                if (url.Contains("youtube.com/playlist") || url.Contains("list="))
                {
                    await DownloadYoutubePlaylistAsZip(youtube, url, format, resItem);
                }
                else if (url.Contains("spotify.com/playlist") || url.Contains("spotify.com/album") || url.Contains("spotify.com/intl-tr/album"))
                {
                    await DownloadSpotifyPlaylistAsZip(youtube, url, format, resItem);
                }
                else
                {
                    await DownloadSingleMedia(youtube, url, format, resItem);
                }

                ShowToastNotification("Başarılı", "İndirme işlemi tamamlandı!");
                lblYtStatus.Text = "İşlem tamamlandı.";

                if (!string.IsNullOrEmpty(_lastDownloadedPathYt))
                {
                    btnShowFileYt.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblYtStatus.Text = "Hata: " + ex.Message;
                progressBarYt.Value = 0;
            }

            btnDownloadYt.Enabled = true;
        }

        private async Task<(string PlaylistName, List<string> TrackQueries)> GetSpotifyPlaylistTracksAsync(string playlistUrl)
        {
            List<string> searchQueries = new List<string>();
            var match = Regex.Match(playlistUrl, @"(playlist|album)/([a-zA-Z0-9]+)");

            if (!match.Success) throw new Exception("Gecersiz Spotify linki.");

            string type = match.Groups[1].Value;
            string id = match.Groups[2].Value;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

                string embedUrl = $"https://open.spotify.com/embed/{type}/{id}";
                var htmlResponse = await client.GetAsync(embedUrl);

                if (!htmlResponse.IsSuccessStatusCode) throw new Exception("Embed sayfasi alinamadi.");

                string html = await htmlResponse.Content.ReadAsStringAsync();

                var dataMatch = Regex.Match(html, @"<script id=""__NEXT_DATA__"" type=""application/json"">(.*?)</script>");
                if (!dataMatch.Success) throw new Exception("Liste verisi HTML icinde bulunamadi.");

                try
                {
                    var doc = JsonDocument.Parse(dataMatch.Groups[1].Value);
                    var entity = doc.RootElement
                        .GetProperty("props")
                        .GetProperty("pageProps")
                        .GetProperty("state")
                        .GetProperty("data")
                        .GetProperty("entity");

                    string entityName = entity.GetProperty("name").GetString() ?? "Spotify Playlist";
                    string entitySubtitle = (entity.TryGetProperty("subtitle", out var sub) ? sub.GetString() : "") ?? "";

                    string playlistFullName = string.IsNullOrWhiteSpace(entitySubtitle) ? entityName : $"{entitySubtitle} - {entityName}";

                    var trackList = entity.GetProperty("trackList");

                    foreach (var track in trackList.EnumerateArray())
                    {
                        string title = track.GetProperty("title").GetString() ?? "Unknown Track";
                        string artist = track.GetProperty("subtitle").GetString() ?? "Unknown Artist";
                        searchQueries.Add($"{artist} - {title}");
                    }

                    return (playlistFullName, searchQueries);
                }
                catch
                {
                    throw new Exception("JSON verisi cozumlenemedi, Spotify altyapiyi degistirmis olabilir.");
                }
            }
        }

        private async Task DownloadSpotifyPlaylistAsZip(YoutubeClient youtube, string playlistUrl, string format, ComboBoxItem resItem)
        {
            var playlistData = await GetSpotifyPlaylistTracksAsync(playlistUrl);
            List<string> trackQueries = playlistData.TrackQueries;

            string outputDir = txtOutputFolder.Text;
            if (string.IsNullOrEmpty(outputDir) || !Directory.Exists(outputDir))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() != DialogResult.OK) { lblYtStatus.Text = "İşlem iptal edildi."; return; }
                outputDir = fbd.SelectedPath;
            }

            string safePlaylistName = Temizle(playlistData.PlaylistName);
            string targetZipPath = Path.Combine(outputDir, safePlaylistName + ".zip");
            string tempDirPath = Path.Combine(Path.GetTempPath(), safePlaylistName + "_" + Guid.NewGuid().ToString().Substring(0, 8));

            Directory.CreateDirectory(tempDirPath);

            await EnsureDependenciesAsync(lblYtStatus);

            int total = trackQueries.Count;
            int current = 0;

            foreach (string query in trackQueries)
            {
                current++;
                lblYtStatus.Text = $"YouTube'da aranıyor ({current}/{total}): {query}";

                var searchResults = youtube.Search.GetVideosAsync(query);
                await foreach (var result in searchResults)
                {
                    string safeTitle = Temizle(result.Title);
                    string targetFilePath = Path.Combine(tempDirPath, safeTitle + "." + format.ToLower());

                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(result.Url);
                    IReadOnlyList<IStreamInfo>? streamInfos = null;

                    if (format.ToUpper() == "MP3" || format.ToUpper() == "WAV")
                    {
                        var audioStream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                        streamInfos = new IStreamInfo[] { audioStream };
                    }
                    else
                    {
                        var audioStream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                        IVideoStreamInfo? videoStream = null;

                        if (resItem.Text == "En Yuksek")
                        {
                            videoStream = streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
                        }
                        else
                        {
                            videoStream = streamManifest.GetVideoOnlyStreams()
                                .Where(s => s.VideoResolution.Height <= resItem.Value)
                                .OrderByDescending(s => s.VideoResolution.Height)
                                .FirstOrDefault() ?? streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
                        }
                        streamInfos = new IStreamInfo[] { audioStream, videoStream };
                    }

                    if (streamInfos != null)
                    {
                        long totalBytes = streamInfos.Sum(s => s.Size.Bytes);
                        DateTime lastUpdate = DateTime.Now;
                        long lastBytes = 0;

                        var progress = new Progress<double>(p =>
                        {
                            Invoke(new Action(() =>
                            {
                                int val = (int)(p * 100);
                                if (val >= 0 && val <= 100) progressBarYt.Value = val;

                                long currentBytes = (long)(totalBytes * p);
                                var now = DateTime.Now;
                                var timeSpan = now - lastUpdate;

                                if (timeSpan.TotalSeconds >= 0.5)
                                {
                                    long bytesPerSec = Math.Max(0, (long)((currentBytes - lastBytes) / timeSpan.TotalSeconds));
                                    long remaining = totalBytes - currentBytes;

                                    if (val >= 99)
                                    {
                                        lblYtStatus.Text = $"[{current}/{total}] İşleniyor/Birleştiriliyor. Lütfen bekleyin...";
                                    }
                                    else
                                    {
                                        lblYtStatus.Text = $"[{current}/{total}] İndiriliyor: %{val} ({FormatBytes(bytesPerSec)}/s) - Kalan: {FormatETA(bytesPerSec, remaining)}";
                                    }

                                    lastUpdate = now;
                                    lastBytes = currentBytes;
                                }
                            }));
                        });

                        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        string ffmpegPath = Path.Combine(baseDir, FFmpegExecutableName);

                        var conversionReq = new ConversionRequestBuilder(targetFilePath);
                        conversionReq.SetFFmpegPath(ffmpegPath);
                        if (format.ToUpper() == "MP3") conversionReq.SetContainer("mp3");
                        if (format.ToUpper() == "WAV") conversionReq.SetContainer("wav");
                        if (format.ToUpper() == "MP4") conversionReq.SetContainer("mp4");

                        await youtube.Videos.DownloadAsync(streamInfos, conversionReq.Build(), progress);
                    }
                    break;
                }
            }

            lblYtStatus.Text = "Dosyalar zipe donusturuluyor...";
            progressBarYt.Style = ProgressBarStyle.Marquee;

            await Task.Run(() =>
            {
                if (File.Exists(targetZipPath)) File.Delete(targetZipPath);
                ZipFile.CreateFromDirectory(tempDirPath, targetZipPath);
                Directory.Delete(tempDirPath, true);
            });

            progressBarYt.Style = ProgressBarStyle.Blocks;
            progressBarYt.Value = 100;
            lblYtStatus.Text = "İşlem tamamlandı: " + safePlaylistName + ".zip";
            _lastDownloadedPathYt = targetZipPath;
        }

        private async Task DownloadYoutubePlaylistAsZip(YoutubeClient youtube, string playlistUrl, string format, ComboBoxItem resItem)
        {
            var playlist = await youtube.Playlists.GetAsync(playlistUrl);
            string safePlaylistName = Temizle(playlist.Title);

            string outputDir = txtOutputFolder.Text;
            if (string.IsNullOrEmpty(outputDir) || !Directory.Exists(outputDir))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() != DialogResult.OK) { lblYtStatus.Text = "İşlem iptal edildi."; return; }
                outputDir = fbd.SelectedPath;
            }

            string targetZipPath = Path.Combine(outputDir, safePlaylistName + ".zip");
            string tempDirPath = Path.Combine(Path.GetTempPath(), safePlaylistName + "_" + Guid.NewGuid().ToString().Substring(0, 8));

            Directory.CreateDirectory(tempDirPath);

            await EnsureDependenciesAsync(lblYtStatus);

            var videos = youtube.Playlists.GetVideosAsync(playlist.Id);
            int totalVideos = 0;
            int currentVideo = 0;

            await foreach (var vid in videos) totalVideos++;

            await foreach (var video in videos)
            {
                currentVideo++;
                lblYtStatus.Text = $"İndiriliyor ({currentVideo}/{totalVideos}): {video.Title}";

                string safeTitle = Temizle(video.Title);
                string targetFilePath = Path.Combine(tempDirPath, safeTitle + "." + format.ToLower());

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Url);
                IReadOnlyList<IStreamInfo>? streamInfos = null;

                if (format.ToUpper() == "MP3" || format.ToUpper() == "WAV")
                {
                    var audioStream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                    streamInfos = new IStreamInfo[] { audioStream };
                }
                else
                {
                    var audioStream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                    IVideoStreamInfo? videoStream = null;

                    if (resItem.Text == "En Yuksek")
                    {
                        videoStream = streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
                    }
                    else
                    {
                        videoStream = streamManifest.GetVideoOnlyStreams()
                            .Where(s => s.VideoResolution.Height <= resItem.Value)
                            .OrderByDescending(s => s.VideoResolution.Height)
                            .FirstOrDefault() ?? streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
                    }
                    streamInfos = new IStreamInfo[] { audioStream, videoStream };
                }

                if (streamInfos != null)
                {
                    long totalBytes = streamInfos.Sum(s => s.Size.Bytes);
                    DateTime lastUpdate = DateTime.Now;
                    long lastBytes = 0;

                    var progress = new Progress<double>(p =>
                    {
                        Invoke(new Action(() =>
                        {
                            int val = (int)(p * 100);
                            if (val >= 0 && val <= 100) progressBarYt.Value = val;

                            long currentBytes = (long)(totalBytes * p);
                            var now = DateTime.Now;
                            var timeSpan = now - lastUpdate;

                            if (timeSpan.TotalSeconds >= 0.5)
                            {
                                long bytesPerSec = Math.Max(0, (long)((currentBytes - lastBytes) / timeSpan.TotalSeconds));
                                long remaining = totalBytes - currentBytes;

                                if (val >= 99)
                                {
                                    lblYtStatus.Text = $"[{currentVideo}/{totalVideos}] İşleniyor/Birleştiriliyor. Lütfen bekleyin...";
                                }
                                else
                                {
                                    lblYtStatus.Text = $"[{currentVideo}/{totalVideos}] İndiriliyor: %{val} ({FormatBytes(bytesPerSec)}/s) - Kalan: {FormatETA(bytesPerSec, remaining)}";
                                }

                                lastUpdate = now;
                                lastBytes = currentBytes;
                            }
                        }));
                    });

                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string ffmpegPath = Path.Combine(baseDir, FFmpegExecutableName);

                    var conversionReq = new ConversionRequestBuilder(targetFilePath);
                    conversionReq.SetFFmpegPath(ffmpegPath);
                    if (format.ToUpper() == "MP3") conversionReq.SetContainer("mp3");
                    if (format.ToUpper() == "WAV") conversionReq.SetContainer("wav");
                    if (format.ToUpper() == "MP4") conversionReq.SetContainer("mp4");

                    await youtube.Videos.DownloadAsync(streamInfos, conversionReq.Build(), progress);
                }
            }

            lblYtStatus.Text = "Dosyalar zipe donusturuluyor...";
            progressBarYt.Style = ProgressBarStyle.Marquee;

            await Task.Run(() =>
            {
                if (File.Exists(targetZipPath)) File.Delete(targetZipPath);
                ZipFile.CreateFromDirectory(tempDirPath, targetZipPath);
                Directory.Delete(tempDirPath, true);
            });

            progressBarYt.Style = ProgressBarStyle.Blocks;
            progressBarYt.Value = 100;
            lblYtStatus.Text = "İşlem tamamlandı: " + safePlaylistName + ".zip";
            _lastDownloadedPathYt = targetZipPath;
        }

        private async Task DownloadSingleMedia(YoutubeClient youtube, string videoUrl, string format, ComboBoxItem resItem)
        {
            if (videoUrl.Contains("spotify.com"))
            {
                lblYtStatus.Text = "Spotify sarkisi araniyor...";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "IrdelixConverter/1.0");
                    string oembedUrl = "https://open.spotify.com/oembed?url=" + videoUrl;

                    string json = await client.GetStringAsync(oembedUrl);

                    using (var doc = JsonDocument.Parse(json))
                    {
                        string title = (doc.RootElement.TryGetProperty("title", out var t) ? t.GetString() : "") ?? "";
                        string author = (doc.RootElement.TryGetProperty("author_name", out var a) ? a.GetString() : "") ?? "";
                        string searchQuery = $"{author} - {title}";

                        lblYtStatus.Text = "YouTube'da bulunuyor...";

                        var searchResults = youtube.Search.GetVideosAsync(searchQuery);
                        await foreach (var result in searchResults)
                        {
                            videoUrl = result.Url;
                            break;
                        }
                    }
                }
            }

            var video = await youtube.Videos.GetAsync(videoUrl);
            string titleFile = Temizle(video.Title);

            string outputDir = txtOutputFolder.Text;
            if (string.IsNullOrEmpty(outputDir) || !Directory.Exists(outputDir))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() != DialogResult.OK) { lblYtStatus.Text = "İşlem iptal edildi."; return; }
                outputDir = fbd.SelectedPath;
            }

            string targetPath = Path.Combine(outputDir, titleFile + "." + format.ToLower());

            await EnsureDependenciesAsync(lblYtStatus);

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
            IReadOnlyList<IStreamInfo>? streamInfos = null;

            if (format.ToUpper() == "MP3" || format.ToUpper() == "WAV")
            {
                var audioStream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                streamInfos = new IStreamInfo[] { audioStream };
            }
            else
            {
                var audioStream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                IVideoStreamInfo? videoStream = null;

                if (resItem.Text == "En Yuksek")
                {
                    videoStream = streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
                }
                else
                {
                    videoStream = streamManifest.GetVideoOnlyStreams()
                        .Where(s => s.VideoResolution.Height <= resItem.Value)
                        .OrderByDescending(s => s.VideoResolution.Height)
                        .FirstOrDefault() ?? streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
                }
                streamInfos = new IStreamInfo[] { audioStream, videoStream };
            }

            if (streamInfos != null)
            {
                long totalBytes = streamInfos.Sum(s => s.Size.Bytes);
                DateTime lastUpdate = DateTime.Now;
                long lastBytes = 0;

                var progress = new Progress<double>(p =>
                {
                    Invoke(new Action(() =>
                    {
                        int val = (int)(p * 100);
                        if (val >= 0 && val <= 100) progressBarYt.Value = val;

                        long currentBytes = (long)(totalBytes * p);
                        var now = DateTime.Now;
                        var timeSpan = now - lastUpdate;

                        if (timeSpan.TotalSeconds >= 0.5)
                        {
                            long bytesPerSec = Math.Max(0, (long)((currentBytes - lastBytes) / timeSpan.TotalSeconds));
                            long remaining = totalBytes - currentBytes;

                            if (val >= 99)
                            {
                                lblYtStatus.Text = "İşleniyor/Birleştiriliyor (Video ve Ses birleştiriliyor, lütfen bekleyin)...";
                            }
                            else
                            {
                                lblYtStatus.Text = $"İndiriliyor: %{val} ({FormatBytes(bytesPerSec)}/s) - Kalan: {FormatETA(bytesPerSec, remaining)}";
                            }

                            lastUpdate = now;
                            lastBytes = currentBytes;
                        }
                    }));
                });

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string ffmpegPath = Path.Combine(baseDir, FFmpegExecutableName);

                var conversionReq = new ConversionRequestBuilder(targetPath);
                conversionReq.SetFFmpegPath(ffmpegPath);
                if (format.ToUpper() == "MP3") conversionReq.SetContainer("mp3");
                if (format.ToUpper() == "WAV") conversionReq.SetContainer("wav");
                if (format.ToUpper() == "MP4") conversionReq.SetContainer("mp4");

                await youtube.Videos.DownloadAsync(streamInfos, conversionReq.Build(), progress);

                lblYtStatus.Text = "İndirme başarıyla tamamlandı.";
                progressBarYt.Value = 100;
                _lastDownloadedPathYt = targetPath;
            }
            else
            {
                lblYtStatus.Text = "Uygun format bulunamadi.";
            }
        }

        // ====================================================================
        // =================== DEPENDENCIES ===================================
        // ====================================================================

        private string FFmpegExecutableName => Environment.OSVersion.Platform == PlatformID.Win32NT ? "ffmpeg.exe" : "ffmpeg";
        private string YtDlpExecutableName => Environment.OSVersion.Platform == PlatformID.Win32NT ? "yt-dlp.exe" : "yt-dlp";

        private async Task EnsureDependenciesAsync(Control? statusLabel = null)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string ffmpegPath = Path.Combine(baseDir, FFmpegExecutableName);
            string ytdlPath = Path.Combine(baseDir, YtDlpExecutableName);

            if (!File.Exists(ffmpegPath))
            {
                if (statusLabel != null) statusLabel.Text = "İlk indirme için gerekli video dönüştürücü bileşeni (FFmpeg) indiriliyor. Lütfen bekleyin...";
                await YoutubeDLSharp.Utils.DownloadFFmpeg(baseDir);
            }

            if (!File.Exists(ytdlPath))
            {
                if (statusLabel != null) statusLabel.Text = "Video indirme motoru (yt-dlp) hazırlanıyor/güncelleniyor. Lütfen bekleyin...";
                await YoutubeDLSharp.Utils.DownloadYtDlp(baseDir);
            }
        }

        // ====================================================================
        // =================== SOCIAL MEDIA ===================================
        // ====================================================================

        private string lastSocialUrl = "";
        private async void txtSocialUrl_TextChanged(object sender, EventArgs e)
        {
            string currentUrl = txtSocialUrl.Text;
            btnDownloadSocial.Enabled = false;

            if (string.IsNullOrWhiteSpace(currentUrl) || !currentUrl.StartsWith("http")) return;

            if (currentUrl == lastSocialUrl) return;
            lastSocialUrl = currentUrl;

            await Task.Delay(800);

            if (txtSocialUrl.Text != currentUrl) return;

            await FetchSocialResolutionsAsync(currentUrl);
        }

        private async Task FetchSocialResolutionsAsync(string url)
        {
            lblSocialStatus.Text = "Çözünürlükler otomatik çekiliyor...";
            cbSocialResolution.Enabled = false;

            try
            {
                await EnsureDependenciesAsync(lblSocialStatus);

                var ytdl = new YoutubeDL();
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                ytdl.YoutubeDLPath = Path.Combine(baseDir, YtDlpExecutableName);
                ytdl.FFmpegPath = Path.Combine(baseDir, FFmpegExecutableName);

                var res = await ytdl.RunVideoDataFetch(url);

                if (res.Success && res.Data != null && res.Data.Formats != null)
                {
                    var formats = res.Data.Formats
                        .Where(f => f.Height != null && f.Height > 0)
                        .GroupBy(f => (int)(f.Height ?? 0))
                        .Select(g => g.First())
                        .OrderByDescending(f => f.Height);

                    if (formats.Any())
                    {
                        cbSocialResolution.Items.Clear();
                        cbSocialResolution.Items.Add(new ComboBoxItem { Text = "En Yüksek", Value = 0 });

                        foreach (var f in formats)
                        {
                            int h = (int)(f.Height ?? 0);
                            int w = (int)(f.Width ?? 0);

                            if (h > w && w > 0)
                            {
                                cbSocialResolution.Items.Add(new ComboBoxItem { Text = $"{w}p", Value = h });
                            }
                            else
                            {
                                cbSocialResolution.Items.Add(new ComboBoxItem { Text = $"{h}p", Value = h });
                            }
                        }

                        cbSocialResolution.SelectedIndex = 0;
                        lblSocialStatus.Text = "Çözünürlük listesi hazır!";
                        btnDownloadSocial.Enabled = true;
                    }
                    else
                    {
                        ResetSocialResolutions("Video formatı bulunamadı.");
                        btnDownloadSocial.Enabled = true;
                    }
                }
                else
                {
                    ResetSocialResolutions("Otomatik veri alınamadı.");
                    btnDownloadSocial.Enabled = true;
                }
            }
            catch
            {
                ResetSocialResolutions("Hata oluştu, varsayılanlarla devam edilecek.");
                btnDownloadSocial.Enabled = true;
            }
            finally
            {
                cbSocialResolution.Enabled = true;
            }
        }

        private void ResetSocialResolutions(string msg)
        {
            lblSocialStatus.Text = msg;
            cbSocialResolution.Items.Clear();
            cbSocialResolution.Items.Add(new ComboBoxItem { Text = "En Yuksek", Value = 0 });
            cbSocialResolution.Items.Add(new ComboBoxItem { Text = "1080p", Value = 1080 });
            cbSocialResolution.Items.Add(new ComboBoxItem { Text = "720p", Value = 720 });
            cbSocialResolution.Items.Add(new ComboBoxItem { Text = "480p", Value = 480 });
            cbSocialResolution.SelectedIndex = 0;
        }

        private async void btnDownloadSocial_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSocialUrl.Text)) return;

            btnDownloadSocial.Enabled = false;
            btnShowFileSocial.Visible = false;
            lblSocialStatus.Text = "Gerekli dosyalar kontrol ediliyor...";
            progressBarSocial.Value = 10;

            try
            {
                await EnsureDependenciesAsync(lblSocialStatus);

                lblSocialStatus.Text = "Video bilgileri alınıyor...";
                progressBarSocial.Value = 30;

                var resItem = cbSocialResolution.SelectedItem as ComboBoxItem ?? new ComboBoxItem { Text = "En Yuksek", Value = 0 };

                string formatSelection = "bestvideo+bestaudio/best";
                if (resItem.Text != "En Yuksek")
                {
                    formatSelection = $"bestvideo[height<={resItem.Value}]+bestaudio/best[height<={resItem.Value}]/best";
                }

                var ytdl = new YoutubeDL();
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                ytdl.YoutubeDLPath = Path.Combine(baseDir, YtDlpExecutableName);
                ytdl.FFmpegPath = Path.Combine(baseDir, FFmpegExecutableName);

                string outputDir = txtOutputFolder.Text;
                if (string.IsNullOrEmpty(outputDir) || !Directory.Exists(outputDir))
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    if (fbd.ShowDialog() != DialogResult.OK) { lblSocialStatus.Text = "İşlem iptal edildi."; btnDownloadSocial.Enabled = true; return; }
                    outputDir = fbd.SelectedPath;
                }

                ytdl.OutputFolder = outputDir;

                var progress = new Progress<DownloadProgress>(p =>
                {
                    Invoke(new Action(() =>
                    {
                        progressBarSocial.Value = (int)(p.Progress * 100);
                        lblSocialStatus.Text = $"İndiriliyor: %{(int)(p.Progress * 100)} | Hız: {p.DownloadSpeed} | Kalan: {p.ETA}";
                    }));
                });

                var options = new OptionSet()
                {
                    Format = formatSelection,
                    MergeOutputFormat = DownloadMergeFormat.Mp4,
                    NoPlaylist = true
                };

                if (cbSocialFormat.SelectedItem?.ToString() == "MP3")
                {
                    options.ExtractAudio = true;
                    options.AudioFormat = AudioConversionFormat.Mp3;
                }
                else if (cbSocialFormat.SelectedItem?.ToString() == "WAV")
                {
                    options.ExtractAudio = true;
                    options.AudioFormat = AudioConversionFormat.Wav;
                }

                var downloadResult = await ytdl.RunVideoDownload(txtSocialUrl.Text.Trim(), overrideOptions: options, progress: progress);

                if (downloadResult.Success)
                {
                    lblSocialStatus.Text = "İndirme tamamlandı!";
                    progressBarSocial.Value = 100;
                    ShowToastNotification("Başarılı", "Video başarıyla indirildi!");
                    _lastDownloadedPathSocial = outputDir;
                    btnShowFileSocial.Visible = true;
                }
                else
                {
                    lblSocialStatus.Text = "Hata: İndirme işlemi başarısız. Video gizli veya silinmiş olabilir.";
                    progressBarSocial.Value = 0;
                }
            }
            catch (Exception ex)
            {
                lblSocialStatus.Text = "Hata: " + ex.Message;
                progressBarSocial.Value = 0;
            }
            finally
            {
                btnDownloadSocial.Enabled = true;
            }
        }

        // ====================================================================
        // =================== CLIPBOARD (PANO) ===============================
        // ====================================================================

        private void Form1_Activated(object? sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    string clipText = Clipboard.GetText().Trim();

                    // Don't re-paste the same URL
                    if (clipText == _lastClipboardUrl) return;

                    if (Uri.IsWellFormedUriString(clipText, UriKind.Absolute))
                    {
                        _lastClipboardUrl = clipText;

                        if (clipText.Contains("youtube.com") || clipText.Contains("youtu.be") || clipText.Contains("spotify.com"))
                        {
                            if (string.IsNullOrWhiteSpace(txtUrl.Text) || !txtUrl.Text.Contains(clipText))
                            {
                                txtUrl.Text = clipText;
                                materialTabControl1.SelectedTab = tabYoutube;

                                // Smart format: auto MP3 for music
                                if (clipText.Contains("spotify.com") || clipText.Contains("music.youtube"))
                                {
                                    cbYtFormat.SelectedItem = "MP3";
                                }
                            }
                        }
                        else if (clipText.Contains("instagram.com") || clipText.Contains("tiktok.com") || clipText.Contains("twitter.com") || clipText.Contains("x.com"))
                        {
                            if (string.IsNullOrWhiteSpace(txtSocialUrl.Text) || !txtSocialUrl.Text.Contains(clipText))
                            {
                                txtSocialUrl.Text = clipText;
                                materialTabControl1.SelectedTab = tabSocial;
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
}