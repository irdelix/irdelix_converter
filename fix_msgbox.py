import re
with open('Form1.cs', 'r', encoding='utf-8') as f:
    text = f.read()

text = text.replace('MessageBox.Show("Listede zaten ses/video dosyasi var. Resim dosyasi ekleyemezsiniz.", "Tur Uyumsuzlugu", MessageBoxButtons.OK, MessageBoxIcon.Warning);', 'ShowToastNotification("Tur Uyumsuzlugu", "Listede zaten ses/video dosyasi var. Resim dosyasi ekleyemezsiniz.");')
text = text.replace('MessageBox.Show("Listede zaten resim dosyasi var. Ses/video dosyasi ekleyemezsiniz.", "Tur Uyumsuzlugu", MessageBoxButtons.OK, MessageBoxIcon.Warning);', 'ShowToastNotification("Tur Uyumsuzlugu", "Listede zaten resim dosyasi var. Ses/video dosyasi ekleyemezsiniz.");')
text = text.replace('MessageBox.Show("Playlist basariyla zip olarak indirildi!", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);', 'ShowToastNotification("Basarili", "Playlist basariyla zip olarak indirildi!");')
text = text.replace('MessageBox.Show($"Indirme basariyla tamamlandi!\\nDosya: {titleFile}", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);', 'ShowToastNotification("Basarili", $"Indirme basariyla tamamlandi! Dosya: {titleFile}");')
text = text.replace('MessageBox.Show(string.Join("\\n", downloadResult.ErrorOutput), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);', 'ShowToastNotification("Hata", "Indirme basarisiz oldu");')
text = text.replace('MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);', 'ShowToastNotification("Hata", ex.Message);')

with open('Form1.cs', 'w', encoding='utf-8') as f:
    f.write(text)
