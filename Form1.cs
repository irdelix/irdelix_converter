using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using NReco.VideoConverter;
using ImageMagick;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using MaterialSkin;
using MaterialSkin.Controls;

namespace ConverterApp
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            cbYtFormat.SelectedIndex = 0;
            
            lbFiles.SelectionMode = SelectionMode.MultiExtended;
            lbFiles.KeyDown += lbFiles_KeyDown;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey800,
                Primary.BlueGrey900,
                Primary.BlueGrey500,
                Accent.LightBlue200,
                TextShade.WHITE
            );
        }

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

            string previousSelection = cbTargetFormat.SelectedItem?.ToString();
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

        private void lbFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lbFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            AddFilesFiltered(files);
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

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void lbFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                btnRemoveFile_Click(sender, e);
            }
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count == 0 || cbTargetFormat.SelectedItem == null) return;

            btnConvert.Enabled = false;
            try
            {
                string targetExt = cbTargetFormat.SelectedItem.ToString();
                int total = lbFiles.Items.Count;
                int current = 0;

                foreach (string file in lbFiles.Items)
                {
                    current++;
                    lblStatus.Text = $"Cevriliyor: {current} / {total}";
                    progressBar.Value = 0;

                    try
                    {
                        string dir = Path.GetDirectoryName(file) ?? "";
                        string name = Path.GetFileNameWithoutExtension(file);
                        string sourceExt = Path.GetExtension(file).ToLower();
                        string targetFile = Path.Combine(dir, name + "." + targetExt);

                        bool isVideoOrAudioTarget = targetExt == "mp3" || targetExt == "wav" || targetExt == "mp4";
                        bool isImageTarget = targetExt == "png" || targetExt == "jpg" || targetExt == "ico" || targetExt == "bmp" || targetExt == "gif";

                        bool isSourceVideoOrAudio = sourceExt == ".mp4" || sourceExt == ".avi" || sourceExt == ".mkv" || sourceExt == ".mp3" || sourceExt == ".wav";
                        bool isSourceImage = sourceExt == ".png" || sourceExt == ".jpg" || sourceExt == ".jpeg" || sourceExt == ".bmp" || sourceExt == ".gif" || sourceExt == ".webp";

                        if (isVideoOrAudioTarget && !isSourceVideoOrAudio)
                        {
                            MessageBox.Show(name + " dosyasi ses/video formatina cevrilemez.", "Uyumsuz Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        if (isImageTarget && !isSourceImage)
                        {
                            MessageBox.Show(name + " dosyasi resim formatina cevrilemez.", "Uyumsuz Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        if (isVideoOrAudioTarget)
                        {
                            await Task.Run(() =>
                            {
                                var convert = new FFMpegConverter();
                                convert.ConvertProgress += (s, ev) =>
                                {
                                    Invoke(new Action(() =>
                                    {
                                        if (ev.TotalDuration.TotalSeconds > 0)
                                        {
                                            int p = (int)((ev.Processed.TotalSeconds / ev.TotalDuration.TotalSeconds) * 100);
                                            if (p >= 0 && p <= 100) progressBar.Value = p;
                                        }
                                    }));
                                };
                                convert.ConvertMedia(file, targetFile, targetExt);
                            });
                        }
                        else if (isImageTarget)
                        {
                            await Task.Run(() =>
                            {
                                using (var image = new MagickImage(file))
                                {
                                    if (targetExt == "png") image.Format = MagickFormat.Png;
                                    if (targetExt == "jpg") image.Format = MagickFormat.Jpeg;
                                    if (targetExt == "bmp") image.Format = MagickFormat.Bmp;
                                    if (targetExt == "gif") image.Format = MagickFormat.Gif;
                                    if (targetExt == "ico")
                                    {
                                        image.Format = MagickFormat.Ico;
                                        image.Resize(256, 256);
                                    }
                                    image.Write(targetFile);
                                }
                            });
                            progressBar.Value = 100;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"'{file}' cevrilirken hata olustu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                lblStatus.Text = "Tum islemler bitti.";
            }
            finally
            {
                progressBar.Value = 0;
                btnConvert.Enabled = true;
            }
        }

        private string Temizle(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        private async void btnDownloadYt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrl.Text)) return;

            btnDownloadYt.Enabled = false;
            lblStatus.Text = "Bilgiler aliniyor...";
            var youtube = new YoutubeClient();
            string videoUrl = txtUrl.Text;

            try
            {
                if (videoUrl.Contains("spotify.com"))
                {
                    lblStatus.Text = "Spotify sarkisi araniyor...";
                    using (var client = new System.Net.Http.HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                        string oembedUrl = "https://open.spotify.com/oembed?url=" + videoUrl;
                        
                        string json = await client.GetStringAsync(oembedUrl);
                        
                        using (var doc = System.Text.Json.JsonDocument.Parse(json))
                        {
                            string title = doc.RootElement.TryGetProperty("title", out var t) ? t.GetString() : "";
                            string author = doc.RootElement.TryGetProperty("author_name", out var a) ? a.GetString() : "";
                            string searchQuery = $"{title} {author} official audio";
                            
                            lblStatus.Text = "YouTube'da bulunuyor...";
                            
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
                string format = cbYtFormat.SelectedItem.ToString();
                
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Nereye kaydedilsin?";
                
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string targetPath = Path.Combine(fbd.SelectedPath, titleFile + "." + format.ToLower());
                    
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
                    IStreamInfo streamInfo = null;

                    if (format.ToUpper() == "MP3" || format.ToUpper() == "WAV")
                    {
                        streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                        targetPath = Path.Combine(fbd.SelectedPath, titleFile + ".mp3");
                    }
                    else
                    {
                        streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
                    }

                    if (streamInfo != null)
                    {
                        lblStatus.Text = "Indiriliyor...";
                        var progress = new Progress<double>(p =>
                        {
                            Invoke(new Action(() =>
                            {
                                int val = (int)(p * 100);
                                if (val >= 0 && val <= 100) progressBar.Value = val;
                            }));
                        });

                        await youtube.Videos.Streams.DownloadAsync(streamInfo, targetPath, progress);
                        lblStatus.Text = "Indirme bitti.";
                        progressBar.Value = 0;
                    }
                    else
                    {
                        lblStatus.Text = "Uygun format bulunamadi.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Hata: " + ex.Message; 
            }
            
            btnDownloadYt.Enabled = true;
        }
    }
}