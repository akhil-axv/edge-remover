using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EdgeRemover;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

internal sealed class MainForm : Form
{
    private readonly TextBox _log = new()
    {
        Dock = DockStyle.Fill,
        Multiline = true,
        ReadOnly = true,
        ScrollBars = ScrollBars.Vertical,
        WordWrap = true,
        Font = new Font("Consolas", 10),
        BackColor = Color.FromArgb(25, 28, 34),
        ForeColor = Color.FromArgb(230, 234, 240)
    };
    private readonly Button _checkButton = new() { Text = "Check availability", AutoSize = true };
    private readonly Button _uninstallButton = new() { Text = "Uninstall Edge", AutoSize = true, Enabled = false };
    private readonly LinkLabel _settingsLink = new() { Text = "Open Windows Installed apps settings", AutoSize = true };
    private bool _edgeDetected;

    public MainForm()
    {
        Text = "Edge Remover";
        MinimumSize = new Size(650, 430);
        Size = new Size(760, 520);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10);

        var title = new Label
        {
            Text = "Microsoft Edge removal helper",
            Font = new Font("Segoe UI Semibold", 16),
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 8)
        };
        var notice = new Label
        {
            Text = "Uses Windows Package Manager only. Windows may block Edge removal on this PC. This app never deletes browser files or edits the registry.",
            AutoSize = true,
            MaximumSize = new Size(690, 0),
            ForeColor = Color.FromArgb(65, 70, 78),
            Margin = new Padding(0, 0, 0, 10)
        };
        var top = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(16),
            BackColor = BackColor
        };
        top.Controls.Add(title);
        top.Controls.Add(notice);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(16, 0, 16, 12),
            BackColor = BackColor
        };
        _checkButton.Click += async (_, _) => await CheckAsync();
        _uninstallButton.Click += async (_, _) => await UninstallAsync();
        _settingsLink.LinkClicked += (_, _) => OpenSettings();
        buttons.Controls.Add(_checkButton);
        buttons.Controls.Add(_uninstallButton);
        buttons.Controls.Add(_settingsLink);

        var logPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16, 0, 16, 16) };
        logPanel.Controls.Add(_log);

        Controls.Add(logPanel);
        Controls.Add(buttons);
        Controls.Add(top);

        WriteLine("Choose “Check availability” to see whether winget can manage Microsoft Edge.");
        WriteLine("No changes are made until you choose “Uninstall Edge” and confirm.");
    }

    private async Task CheckAsync()
    {
        SetBusy(true);
        WriteLine("\r\nChecking for Microsoft Edge through winget...");
        try
        {
            var result = await RunWingetAsync("list", "--id", "Microsoft.Edge", "--exact", "--disable-interactivity");
            WriteOutput(result.Output);
            _edgeDetected = result.ExitCode == 0 &&
                result.Output.Contains("Microsoft.Edge", StringComparison.OrdinalIgnoreCase);
            _uninstallButton.Enabled = _edgeDetected;
            if (_edgeDetected)
                WriteLine("Edge appears in winget. Uninstall may still be unavailable on this Windows edition or in your region.");
            else
                WriteLine("winget did not confirm a removable Edge package. Use Windows settings to check available options.");
        }
        catch (Win32Exception)
        {
            _edgeDetected = false;
            _uninstallButton.Enabled = false;
            WriteLine("Windows Package Manager (winget) was not found. Open Installed apps settings to check Windows' supported options.");
        }
        catch (Exception ex)
        {
            _edgeDetected = false;
            _uninstallButton.Enabled = false;
            WriteLine($"Could not check package availability: {ex.Message}");
        }
        finally { SetBusy(false); }
    }

    private async Task UninstallAsync()
    {
        if (!_edgeDetected) return;

        var answer = MessageBox.Show(
            "This will ask winget to uninstall Microsoft Edge using Windows' package management interface. Windows may refuse the request. Continue?",
            "Confirm Edge uninstall",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2);
        if (answer != DialogResult.Yes) return;

        SetBusy(true);
        WriteLine("\r\nStarting winget uninstall. Follow any prompts shown by Windows...");
        try
        {
            var result = await RunWingetAsync(
                "uninstall", "--id", "Microsoft.Edge", "--exact",
                "--interactive", "--accept-source-agreements");
            WriteOutput(result.Output);
            if (result.ExitCode == 0)
            {
                WriteLine("winget completed successfully. Restart Windows if prompted, then verify the browser list in Settings.");
                _uninstallButton.Enabled = false;
                _edgeDetected = false;
            }
            else
            {
                WriteLine($"winget exited with code {result.ExitCode}. Windows may not permit Edge removal on this device.");
                WriteLine("No fallback file deletion was attempted. Check Windows Installed apps for the supported option.");
            }
        }
        catch (Win32Exception)
        {
            WriteLine("winget could not be started. Open Installed apps settings to check available options.");
        }
        catch (Exception ex)
        {
            WriteLine($"Uninstall could not be completed: {ex.Message}");
        }
        finally { SetBusy(false); }
    }

    private static async Task<(int ExitCode, string Output)> RunWingetAsync(params string[] args)
    {
        var start = new ProcessStartInfo
        {
            FileName = "winget.exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        foreach (var arg in args) start.ArgumentList.Add(arg);

        using var process = new Process { StartInfo = start };
        var output = new StringBuilder();
        process.OutputDataReceived += (_, e) => { if (e.Data is not null) output.AppendLine(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data is not null) output.AppendLine(e.Data); };
        if (!process.Start()) throw new InvalidOperationException("winget did not start.");
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();
        return (process.ExitCode, output.ToString());
    }

    private void OpenSettings()
    {
        try
        {
            Process.Start(new ProcessStartInfo("ms-settings:appsfeatures") { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            WriteLine($"Could not open Settings: {ex.Message}");
        }
    }

    private void SetBusy(bool busy)
    {
        _checkButton.Enabled = !busy;
        _uninstallButton.Enabled = !busy && _edgeDetected;
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    private void WriteOutput(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        WriteLine(text.Trim());
    }

    private void WriteLine(string text)
    {
        _log.AppendText(text + Environment.NewLine);
    }
}
