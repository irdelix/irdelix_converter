using System;
using System.IO;
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

namespace ConverterApp
{
    public class ComboBoxItem
    {
        public string Text { get; set; } = string.Empty;
        public int Value { get; set; }
        public override string ToString() { return Text; }
    }

    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            
            lbFiles.AllowDrop = true;
            cbYtFormat.SelectedIndex = 1;
            cbYtFormat.SelectedIndex = 1;
            ResetResolutions();

            btnDownloadYt.Enabled = false;
            btnDownloadSocial.Enabled = false;
            
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
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Green600, Primary.Green700, Primary.Green200, Accent.Green400, TextShade.WHITE
            );

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
        private void ResetResolutions()
        {
            cbYtResolution.Items.Clear();
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "En Yuksek", Value = 0 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "1080p", Value = 1080 });
            cbYtResolution.Items.Add(new ComboBoxItem { Text = "720p", Value = 720 });
            cbYtResolution.SelectedIndex = 0;

            cbResolution.Items.Clear();
            cbResolution.Items.Add(new ComboBoxItem { Text = "Orjinal", Value = 0 });
            cbResolution.Items.Add(new ComboBoxItem { Text = "1080p", Value = 1080 });
            cbResolution.Items.Add(new ComboBoxItem { Text = "720p", Value = 720 });
            cbResolution.Items.Add(new ComboBoxItem { Text = "480p", Value = 480 });
            cbResolution.SelectedIndex = 0;
        }

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



        private string lastYtUrl = "";
        private async void txtUrl_TextChanged(object sender, EventArgs e)
        {
            string currentUrl = txtUrl.Text;
            btnDownloadYt.Enabled = false;
            
            if (string.IsNullOrWhiteSpace(currentUrl) || !currentUrl.StartsWith("http")) return;

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
            lblYtStatus.Text = "Bilgiler aliniyor...";
            var youtube = new YoutubeClient();
            string url = txtUrl.Text;
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
            
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string safePlaylistName = Temizle(playlistData.PlaylistName);
                string targetZipPath = Path.Combine(fbd.SelectedPath, safePlaylistName + ".zip");
                string tempDirPath = Path.Combine(Path.GetTempPath(), safePlaylistName + "_" + Guid.NewGuid().ToString().Substring(0, 8));

                Directory.CreateDirectory(tempDirPath);

                await EnsureDependenciesAsync(lblYtStatus);

                int total = trackQueries.Count;
                int current = 0;

                foreach (string query in trackQueries)
                {
                    current++;
                    lblYtStatus.Text = $"YouTube'da araniyor ({current}/{total}): {query}";
                    
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
                                        
                                         if (val >= 99)
                                         {
                                             lblYtStatus.Text = $"[{current}/{total}] İşleniyor/Birleştiriliyor. Lütfen bekleyin...";
                                         }
                                        else
                                        {
                                            lblYtStatus.Text = $"[{current}/{total}] Indiriliyor: %{val} | Hiz: {FormatBytes(bytesPerSec)}/s";
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
                lblYtStatus.Text = "Islem tamamlandi: " + safePlaylistName + ".zip";
                MessageBox.Show("Playlist basariyla zip olarak indirildi!", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblYtStatus.Text = "Islem iptal edildi.";
            }
        }

        private async Task DownloadYoutubePlaylistAsZip(YoutubeClient youtube, string playlistUrl, string format, ComboBoxItem resItem)
        {
            var playlist = await youtube.Playlists.GetAsync(playlistUrl);
            string safePlaylistName = Temizle(playlist.Title);
            
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string targetZipPath = Path.Combine(fbd.SelectedPath, safePlaylistName + ".zip");
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
                    lblYtStatus.Text = $"Indiriliyor ({currentVideo}/{totalVideos}): {video.Title}";
                    
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
                                    
                                     if (val >= 99)
                                     {
                                         lblYtStatus.Text = $"[{currentVideo}/{totalVideos}] İşleniyor/Birleştiriliyor. Lütfen bekleyin...";
                                     }
                                    else
                                    {
                                        lblYtStatus.Text = $"[{currentVideo}/{totalVideos}] Indiriliyor: %{val} | Hiz: {FormatBytes(bytesPerSec)}/s";
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
                lblYtStatus.Text = "Islem tamamlandi: " + safePlaylistName + ".zip";
                MessageBox.Show("Playlist basariyla zip olarak indirildi!", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblYtStatus.Text = "Islem iptal edildi.";
            }
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
            
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string targetPath = Path.Combine(fbd.SelectedPath, titleFile + "." + format.ToLower());
                
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
                                
                                 if (val >= 99)
                                 {
                                     lblYtStatus.Text = "İşleniyor/Birleştiriliyor (Video ve Ses birleştiriliyor, lütfen bekleyin)...";
                                 }
                                else
                                {
                                    lblYtStatus.Text = $"Indiriliyor: %{val} | Hiz: {FormatBytes(bytesPerSec)}/s";
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

                    lblYtStatus.Text = "Indirme basariyla tamamlandi.";
                    progressBarYt.Value = 100;
                    MessageBox.Show($"Indirme basariyla tamamlandi!\nDosya: {titleFile}", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblYtStatus.Text = "Uygun format bulunamadi.";
                }
            }
            else
            {
                lblYtStatus.Text = "Islem iptal edildi.";
            }
        }

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
            lblSocialStatus.Text = "Gerekli dosyalar kontrol ediliyor...";
            progressBarSocial.Value = 10;

            try
            {
                await EnsureDependenciesAsync(lblSocialStatus);

                lblSocialStatus.Text = "Video bilgileri aliniyor...";
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

                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    ytdl.OutputFolder = fbd.SelectedPath;
            
                    var progress = new Progress<DownloadProgress>(p =>
                    {
                        Invoke(new Action(() =>
                        {
                            progressBarSocial.Value = (int)(p.Progress * 100);
                            lblSocialStatus.Text = $"Indiriliyor... Hiz: {p.DownloadSpeed}";
                        }));
                    });

                    var options = new OptionSet()
                    {
                        Format = formatSelection
                    };

                    var downloadResult = await ytdl.RunVideoDownload(txtSocialUrl.Text, overrideOptions: options, progress: progress);

                    if (downloadResult.Success)
                    {
                        lblSocialStatus.Text = "Indirme tamamlandi!";
                        progressBarSocial.Value = 100;
                    }
                    else
                    {
                        lblSocialStatus.Text = "Indirme basarisiz oldu.";
                        MessageBox.Show(string.Join("\n", downloadResult.ErrorOutput), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    lblSocialStatus.Text = "Islem iptal edildi.";
                }
            }
            catch (Exception ex)
            {
                lblSocialStatus.Text = "Hata olustu.";
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDownloadSocial.Enabled = true;
            }
        }
    }
}