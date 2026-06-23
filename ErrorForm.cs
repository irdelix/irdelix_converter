using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using MaterialSkin;
using MaterialSkin.Controls;

namespace ConverterApp
{
    public class ErrorForm : MaterialForm
    {
        private static HashSet<string> ReportedErrors = new HashSet<string>();
        
        private MaterialLabel lblMessage;
        private MaterialButton btnReport;
        private MaterialButton btnCancel;
        private string _errorMessage;
        private string _stackTrace;

        public ErrorForm(string userMessage, string errorMessage, string stackTrace)
        {
            _errorMessage = errorMessage;
            _stackTrace = stackTrace;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            this.Text = "Hata Oluştu";
            this.Size = new Size(600, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Sizable = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblMessage = new MaterialLabel();
            lblMessage.Text = userMessage;
            lblMessage.Location = new Point(20, 80);
            lblMessage.Size = new Size(560, 100);
            this.Controls.Add(lblMessage);

            btnReport = new MaterialButton();
            btnReport.Text = "Sisteme Bildir";
            btnReport.Location = new Point(20, 220);
            btnReport.Click += BtnReport_Click;
            this.Controls.Add(btnReport);

            btnCancel = new MaterialButton();
            btnCancel.Text = "İptal";
            btnCancel.Type = MaterialButton.MaterialButtonType.Outlined;
            btnCancel.Location = new Point(220, 220);
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private async void BtnReport_Click(object? sender, EventArgs e)
        {
            // Basit bir hata kodu üretiyoruz (Mesaj + StackTrace'in ilk 100 karakteri veya GetHashCode)
            string errorCode = (_errorMessage + _stackTrace).GetHashCode().ToString();

            if (ReportedErrors.Contains(errorCode))
            {
                MessageBox.Show("Zaten bu sorunu bildirdiniz. Lütfen bir süre sonra tekrar deneyiniz veya güncellemeleri bekleyiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            try
            {
                btnReport.Enabled = false;
                btnReport.Text = "Gönderiliyor...";

                string proxyUrl = "https://webhook.irdelix.com";

                var payload = new
                {
                    content = $"**Yeni Hata Raporu!**\n\n**Hata Mesajı:**\n```{_errorMessage}```\n**Stack Trace:**\n```{_stackTrace.Substring(0, Math.Min(_stackTrace.Length, 1500))}```"
                };

                string jsonPayload = JsonSerializer.Serialize(payload);

                using (var client = new HttpClient())
                {
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(proxyUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        ReportedErrors.Add(errorCode);
                        MessageBox.Show("Hata başarıyla sisteme bildirildi. Geliştirici en kısa sürede inceleyecektir.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Hata bildirimi gönderilemedi. Sunucu yanıtı: " + response.StatusCode, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata bildirimi sırasında bir sorun oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnReport.Enabled = true;
                btnReport.Text = "Sisteme Bildir";
            }
        }
    }
}
