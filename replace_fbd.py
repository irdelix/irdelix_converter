import re

with open('Form1.cs', 'r', encoding='utf-8') as f:
    text = f.read()

# Pattern to replace the FolderBrowserDialog block with GetOutputPath()
pattern = r'FolderBrowserDialog fbd = new FolderBrowserDialog\(\);\s*if \(fbd\.ShowDialog\(\) == DialogResult\.OK\)'
replacement = r'string targetFolder = GetOutputPath();\n            if (targetFolder != null)'

text = re.sub(pattern, replacement, text)

# Replace fbd.SelectedPath with targetFolder in the specific blocks
text = text.replace('fbd.SelectedPath', 'targetFolder')

with open('Form1.cs', 'w', encoding='utf-8') as f:
    f.write(text)
