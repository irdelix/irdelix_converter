import re
with open('Form1.cs', 'r', encoding='utf-8') as f:
    content = f.read()

settings_controls = """
        private TabPage tabSettings;
        private MaterialSkin.Controls.MaterialTextBox txtOutputPath;
        private MaterialSkin.Controls.MaterialButton btnBrowseOutput;
        private MaterialSkin.Controls.MaterialSwitch swTheme;
        private MaterialSkin.Controls.MaterialComboBox cbThemeColor;

        private void InitializeSettingsTab()
        {
            tabSettings = new TabPage("Ayarlar");
            tabSettings.BackColor = System.Drawing.Color.White;

            var cardOutput = new MaterialSkin.Controls.MaterialCard();
            cardOutput.Location = new System.Drawing.Point(20, 20);
            cardOutput.Size = new System.Drawing.Size(760, 100);

            txtOutputPath = new MaterialSkin.Controls.MaterialTextBox();
            txtOutputPath.Hint = "Indirme Klasoru (Bos birakilirsa her seferinde sorar)";
            txtOutputPath.Location = new System.Drawing.Point(20, 20);
            txtOutputPath.Size = new System.Drawing.Size(600, 50);

            btnBrowseOutput = new MaterialSkin.Controls.MaterialButton();
            btnBrowseOutput.Text = "SEC";
            btnBrowseOutput.Location = new System.Drawing.Point(640, 25);
            btnBrowseOutput.Click += (s, e) => {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog()) {
                    if (fbd.ShowDialog() == DialogResult.OK) {
                        txtOutputPath.Text = fbd.SelectedPath;
                    }
                }
            };

            cardOutput.Controls.Add(txtOutputPath);
            cardOutput.Controls.Add(btnBrowseOutput);

            var cardTheme = new MaterialSkin.Controls.MaterialCard();
            cardTheme.Location = new System.Drawing.Point(20, 140);
            cardTheme.Size = new System.Drawing.Size(760, 100);

            swTheme = new MaterialSkin.Controls.MaterialSwitch();
            swTheme.Text = "Karanlik Tema";
            swTheme.Checked = true;
            swTheme.Location = new System.Drawing.Point(20, 30);
            swTheme.CheckedChanged += (s, e) => {
                MaterialSkinManager.Instance.Theme = swTheme.Checked ? MaterialSkinManager.Themes.DARK : MaterialSkinManager.Themes.LIGHT;
            };

            cbThemeColor = new MaterialSkin.Controls.MaterialComboBox();
            cbThemeColor.Hint = "Tema Rengi";
            cbThemeColor.Items.AddRange(new object[] { "Varsayilan (Mavi-Gri)", "Yesil", "Mavi", "Kirmizi", "Turuncu" });
            cbThemeColor.SelectedIndex = 0;
            cbThemeColor.Location = new System.Drawing.Point(250, 20);
            cbThemeColor.Size = new System.Drawing.Size(300, 50);
            cbThemeColor.SelectedIndexChanged += (s, e) => {
                var mgr = MaterialSkinManager.Instance;
                switch (cbThemeColor.SelectedIndex) {
                    case 0: mgr.ColorScheme = new ColorScheme(Primary.BlueGrey900, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE); break;
                    case 1: mgr.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Green400, TextShade.WHITE); break;
                    case 2: mgr.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue200, Accent.Blue400, TextShade.WHITE); break;
                    case 3: mgr.ColorScheme = new ColorScheme(Primary.Red600, Primary.Red700, Primary.Red200, Accent.Red400, TextShade.WHITE); break;
                    case 4: mgr.ColorScheme = new ColorScheme(Primary.Orange600, Primary.Orange700, Primary.Orange200, Accent.Orange400, TextShade.WHITE); break;
                }
            };

            cardTheme.Controls.Add(swTheme);
            cardTheme.Controls.Add(cbThemeColor);

            tabSettings.Controls.Add(cardOutput);
            tabSettings.Controls.Add(cardTheme);

            materialTabControl1.Controls.Add(tabSettings);
        }

        private string GetOutputPath()
        {
            if (txtOutputPath != null && !string.IsNullOrWhiteSpace(txtOutputPath.Text))
                return txtOutputPath.Text;

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    return fbd.SelectedPath;
            }
            return null;
        }
"""

content = content.replace('LoadMontserratFont();', 'InitializeSettingsTab();\n            LoadMontserratFont();')
content = content.replace('public partial class Form1 : MaterialForm\n    {', 'public partial class Form1 : MaterialForm\n    {\n' + settings_controls)

with open('Form1.cs', 'w', encoding='utf-8') as f:
    f.write(content)
