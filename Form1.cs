using System.Drawing;

namespace SmartHighlineTool;

public partial class Form1 : Form
{
    private byte[]? _currentDump;
    private string? _currentFilePath;
    private EepromResult? _currentResult;

    private readonly Label lblDrop;
    private readonly Label lblRadio;
    private readonly Label lblPin;
    private readonly Label lblPinVerification;
    private readonly Label lblLockStatus;
    private readonly Button btnOpen;
    private readonly Button btnReset;
    private readonly Button btnAdvanced;
    private readonly TextBox txtAdvanced;
    private readonly Panel pinPanel;
    private readonly Panel statusPanel;

    private readonly LinkLabel lblCredits;

    public Form1()
    {
        InitializeComponent();

        Text = "Smart Highline EEPROM Tool";
        Width = 680;
        Height = 600;
        MinimumSize = new Size(620, 520);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(245, 246, 248);
        Font = new Font("Segoe UI", 10F);

        AllowDrop = true;
        DragEnter += Form1_DragEnter;
        DragDrop += Form1_DragDrop;

        // Header
        var lblTitle = new Label
        {
            Text = "Bosch Smart Highline",
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(28, 22)
        };

        var lblSubtitle = new Label
        {
            Text = "M95128 EEPROM decoder & reset utility",
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(31, 61)
        };

        // Drop / open section
        var filePanel = new Panel
        {
            Location = new Point(30, 95),
            Width = 600,
            Height = 90,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        lblDrop = new Label
        {
            Text = "Drop a 16 KB M95128 .bin file here",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(18, 17)
        };

        btnOpen = new Button
        {
            Text = "Open EEPROM",
            Width = 130,
            Height = 32,
            Location = new Point(18, 46)
        };

        btnOpen.Click += BtnOpen_Click;

        lblRadio = new Label
        {
            Text = "No EEPROM loaded",
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(165, 53)
        };

        lblCredits = new LinkLabel
        {
            Text = "© 2026 Built by Veaci · GitHub",
            AutoSize = true,
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.DimGray,
            LinkColor = Color.FromArgb(70, 100, 160),
            ActiveLinkColor = Color.FromArgb(40, 70, 130),
            Location = new Point(30, 500)
        };

        lblCredits.LinkClicked += (_, _) =>
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/veaciv/smart-451-highline-radio-tool",
                UseShellExecute = true
            });
        };

        filePanel.Controls.Add(lblDrop);
        filePanel.Controls.Add(btnOpen);
        filePanel.Controls.Add(lblRadio);

        // PIN section
        pinPanel = new Panel
        {
            Location = new Point(30, 202),
            Width = 600,
            Height = 125,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var lblPinTitle = new Label
        {
            Text = "RADIO CODE",
            ForeColor = Color.DimGray,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(18, 14)
        };

        lblPin = new Label
        {
            Text = "----",
            Font = new Font("Consolas", 34F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 35)
        };

        lblPinVerification = new Label
        {
            Text = "Load an EEPROM to decode",
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(175, 67)
        };

        pinPanel.Controls.Add(lblPinTitle);
        pinPanel.Controls.Add(lblPin);
        pinPanel.Controls.Add(lblPinVerification);

        // Status section
        statusPanel = new Panel
        {
            Location = new Point(30, 344),
            Width = 600,
            Height = 88,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var lblStatusTitle = new Label
        {
            Text = "LOCK STATUS",
            ForeColor = Color.DimGray,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(18, 14)
        };

        lblLockStatus = new Label
        {
            Text = "Unknown",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(18, 42)
        };

        btnReset = new Button
        {
            Text = "Create Repair / Reset Dump",
            Width = 165,
            Height = 36,
            Location = new Point(410, 27),
            Enabled = false
        };

        btnReset.Click += BtnReset_Click;

        statusPanel.Controls.Add(lblStatusTitle);
        statusPanel.Controls.Add(lblLockStatus);
        statusPanel.Controls.Add(btnReset);

        // Advanced
        btnAdvanced = new Button
        {
            Text = "Show Advanced",
            Width = 140,
            Height = 32,
            Location = new Point(30, 450),
            Enabled = false
        };

        btnAdvanced.Click += BtnAdvanced_Click;

        txtAdvanced = new TextBox
        {
            Location = new Point(30, 490),
            Width = 600,
            Height = 160,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 9F),
            BackColor = Color.White,
            Visible = false
        };

        Controls.Add(lblTitle);
        Controls.Add(lblSubtitle);
        Controls.Add(filePanel);
        Controls.Add(pinPanel);
        Controls.Add(statusPanel);
        Controls.Add(btnAdvanced);
        Controls.Add(txtAdvanced);
        Controls.Add(lblCredits);

        Resize += Form1_Resize;
    }

    private void BtnOpen_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Open M95128 EEPROM Dump",
            Filter = "EEPROM binary files (*.bin)|*.bin|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
            LoadEeprom(dialog.FileName);
    }

    private void LoadEeprom(string path)
    {
        try
        {
            byte[] data = File.ReadAllBytes(path);
            EepromResult result = EepromAnalyzer.Analyze(data);

            _currentDump = data;
            _currentFilePath = path;
            _currentResult = result;

            if (!result.IsValid)
            {
                ShowInvalid(result.Error);
                return;
            }

            lblDrop.Text = Path.GetFileName(path);

            lblRadio.Text = result.RadioId is not null
                ? $"Bosch / Smart ID: {result.RadioId}"
                : "16 KB M95128 dump loaded";

            if (result.PinsMatch)
            {
                lblPin.Text = result.PinA;
                lblPin.ForeColor = Color.FromArgb(25, 110, 60);

                lblPinVerification.Text =
                    $"✓ Copy A: {result.PinA}    ✓ Copy B: {result.PinB}\r\n" +
                    "Both encoded PIN copies match";

                lblPinVerification.ForeColor = Color.FromArgb(25, 110, 60);
                btnReset.Enabled = true;
            }
            else
            {
                lblPin.Text = "????";
                lblPin.ForeColor = Color.DarkRed;

                lblPinVerification.Text =
                    $"Copy A: {result.PinA}    Copy B: {result.PinB}\r\n" +
                    "PIN copies do not match — do not use code";

                lblPinVerification.ForeColor = Color.DarkRed;
                btnReset.Enabled = false;
            }

            if (result.PossibleBlocked)
            {
                lblLockStatus.Text = "⚠ Possible blocked / error state";
                lblLockStatus.ForeColor = Color.DarkOrange;
            }
            else
            {
                lblLockStatus.Text = "No known blocked flag detected";
                lblLockStatus.ForeColor = Color.FromArgb(25, 110, 60);
            }

            btnAdvanced.Enabled = true;
            UpdateAdvancedInfo();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not read EEPROM file.\r\n\r\n{ex.Message}",
                "Read Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ShowInvalid(string error)
    {
        lblPin.Text = "----";
        lblPin.ForeColor = Color.Black;

        lblPinVerification.Text = error;
        lblPinVerification.ForeColor = Color.DarkRed;

        lblLockStatus.Text = "Unknown";
        lblLockStatus.ForeColor = Color.DimGray;

        btnReset.Enabled = false;
        btnAdvanced.Enabled = false;

        MessageBox.Show(
            error,
            "Unsupported EEPROM",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }

    private void BtnReset_Click(object? sender, EventArgs e)
    {
        if (_currentDump is null ||
            _currentFilePath is null ||
            _currentResult is null ||
            !_currentResult.PinsMatch)
        {
            return;
        }

        using var consentForm = new RepairConsentForm(
    _currentResult.PinA,
    _currentDump[0x03F0],
    _currentDump[0x03F8],
    _currentDump[0x03F9]);

        if (consentForm.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            byte[] resetDump = EepromAnalyzer.CreateResetDump(_currentDump);

            string directory =
                Path.GetDirectoryName(_currentFilePath) ?? "";

            string baseName =
                Path.GetFileNameWithoutExtension(_currentFilePath);

            string suggestedName =
                $"{baseName}.reset.bin";

            using var dialog = new SaveFileDialog
            {
                Title = "Save Reset EEPROM",
                Filter = "EEPROM binary file (*.bin)|*.bin",
                FileName = suggestedName,
                InitialDirectory = directory
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string sourceFullPath =
                Path.GetFullPath(_currentFilePath);

            string destinationFullPath =
                Path.GetFullPath(dialog.FileName);

            if (string.Equals(
                sourceFullPath,
                destinationFullPath,
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "The reset dump cannot overwrite the original EEPROM file.",
                    "Safety Check",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            File.WriteAllBytes(destinationFullPath, resetDump);

            // Read it back from disk and verify it.
            byte[] verification =
                File.ReadAllBytes(destinationFullPath);

            if (!resetDump.SequenceEqual(verification))
            {
                throw new IOException(
                    "Saved file verification failed.");
            }

            var resetResult =
                EepromAnalyzer.Analyze(verification);

            if (!resetResult.PinsMatch ||
                resetResult.PinA != _currentResult.PinA)
            {
                throw new InvalidOperationException(
                    "PIN verification failed after saving reset dump.");
            }

            MessageBox.Show(
                $"Reset dump created successfully.\r\n\r\n" +
                $"PIN: {resetResult.PinA}\r\n" +
                $"Saved as:\r\n{destinationFullPath}\r\n\r\n" +
                "Your original EEPROM file was not modified.",
                "Reset Dump Created",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Reset dump was NOT created.\r\n\r\n{ex.Message}",
                "Safety Check Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnAdvanced_Click(object? sender, EventArgs e)
    {
        txtAdvanced.Visible = !txtAdvanced.Visible;

        btnAdvanced.Text =
            txtAdvanced.Visible
                ? "Hide Advanced"
                : "Show Advanced";

        if (txtAdvanced.Visible)
        {
            Height = 750;
            lblCredits.Location = new Point(30, 665);
        }
        else
        {
            Height = 600;
            lblCredits.Location = new Point(30, 500);
        }
    }

    private void UpdateAdvancedInfo()
    {
        if (_currentDump is null ||
            _currentResult is null)
        {
            txtAdvanced.Text = "";
            return;
        }

        txtAdvanced.Text =
            $"File size:      {_currentDump.Length:N0} bytes\r\n" +
            $"SHA-256:        {_currentResult.Sha256}\r\n" +
            $"Radio ID:       {_currentResult.RadioId ?? "Not detected"}\r\n" +
            $"\r\n" +
            $"PIN COPY A\r\n" +
            $"Offset:         0x03E0\r\n" +
            $"Raw:            {HexBytes(_currentDump, 0x03E0, 4)}\r\n" +
            $"Decoded:        {_currentResult.PinA}\r\n" +
            $"\r\n" +
            $"PIN COPY B\r\n" +
            $"Offset:         0x03E8\r\n" +
            $"Raw:            {HexBytes(_currentDump, 0x03E8, 4)}\r\n" +
            $"Decoded:        {_currentResult.PinB}\r\n" +
            $"\r\n" +
            $"STATUS BYTES\r\n" +
            $"0x03F0:         {_currentResult.Status03F0:X2}\r\n" +
            $"0x03F8:         {_currentResult.Status03F8:X2}\r\n" +
            $"0x03F9:         {_currentResult.Status03F9:X2}\r\n";
    }

    private static string HexBytes(
        byte[] data,
        int offset,
        int count)
    {
        return string.Join(
            " ",
            data.Skip(offset)
                .Take(count)
                .Select(b => b.ToString("X2")));
    }

    private void Form1_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            e.Effect = DragDropEffects.Copy;
        else
            e.Effect = DragDropEffects.None;
    }

    private void Form1_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files ||
            files.Length == 0)
        {
            return;
        }

        LoadEeprom(files[0]);
    }

    private void Form1_Resize(object? sender, EventArgs e)
    {
        int width = ClientSize.Width - 60;

        if (width < 500)
            return;

        foreach (Control control in Controls)
        {
            if (control == pinPanel ||
                control == statusPanel ||
                control == txtAdvanced)
            {
                control.Width = width;
            }
        }
    }
}