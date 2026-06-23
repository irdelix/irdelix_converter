import json
import os

transcript_path = r"C:\Users\irdelix\.gemini\antigravity-ide\brain\d3842463-5265-4080-b387-bd933b06b66e\.system_generated\logs\transcript.jsonl"
form1_cs_content = None
form1_designer_content = None

with open(transcript_path, 'r', encoding='utf-8') as f:
    for line in f:
        try:
            entry = json.loads(line)
            if entry.get("step_index", 0) >= 847:
                break
                
            if entry.get("type") == "PLANNER_RESPONSE" and "tool_calls" in entry:
                for tc in entry["tool_calls"]:
                    # check for multi_replace_file_content or replace_file_content or write_to_file or run_command
                    # Actually, if we just want the full file content, the easiest way is if the agent read it or wrote it.
                    # Usually, the agent runs `cat` or `Get-Content` or it's inside `view_file` response.
                    pass
            
            # check the tool responses
            if entry.get("type") == "TOOL_RESPONSE" and "responses" in entry:
                for tr in entry["responses"]:
                    if tr.get("name") == "view_file":
                        # if it read the file
                        out = tr.get("response", {}).get("output", "")
                        if "public partial class Form1" in out and "namespace WinFormsApp2" in out:
                            form1_cs_content = out
                        if "partial class Form1" in out and "Windows Form Designer generated code" in out:
                            form1_designer_content = out
                    elif tr.get("name") == "run_command":
                        out = tr.get("response", {}).get("output", "")
                        if "public partial class Form1" in out and "namespace WinFormsApp2" in out:
                            form1_cs_content = out
                        if "partial class Form1" in out and "Windows Form Designer generated code" in out:
                            form1_designer_content = out
        except Exception as e:
            pass

if form1_cs_content:
    with open("Form1_cs_recovered.txt", "w", encoding="utf-8") as f:
        f.write(form1_cs_content)
if form1_designer_content:
    with open("Form1_designer_recovered.txt", "w", encoding="utf-8") as f:
        f.write(form1_designer_content)
print("Done. Found Form1.cs:", form1_cs_content is not None, " Found Form1.Designer.cs:", form1_designer_content is not None)
