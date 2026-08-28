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

    private readonly Label lblCounter;

    private readonly Button btnOpen;
    private readonly Button btnReset;
    private readonly Button btnChangePin;
    private readonly Button btnAdvanced;

    private readonly TextBox txtAdvanced;

    private readonly Panel pinPanel;
    private readonly Panel statusPanel;

    private readonly LinkLabel lblCredits;

    public Form1()
    {
        InitializeComponent();

        Text = "Smart 451 Highline Radio Tool";

        Width = 680;
        Height = 600;

        MinimumSize = new Size(620, 520);

        StartPosition = FormStartPosition.CenterScreen;

        BackColor = Color.FromArgb(245, 246, 248);

        Font = new Font("Segoe UI", 10F);

        AllowDrop = true;

        DragEnter += Form1_DragEnter;
        DragDrop += Form1_DragDrop;

        //
        // HEADER
        //

        var lblTitle = new Label
        {
            Text = "Smart 451 Highline",

            Font = new Font(
                "Segoe UI",
                20F,
                FontStyle.Bold),

            AutoSize = true,

            Location = new Point(28, 22)
        };

        var lblSubtitle = new Label
        {
            Text = "M95128 EEPROM diagnostic & recovery utility",

            ForeColor = Color.DimGray,

            AutoSize = true,

            Location = new Point(31, 61)
        };

        //
        // FILE PANEL
        //

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

            Font = new Font(
                "Segoe UI",
                11F,
                FontStyle.Bold),

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

        filePanel.Controls.Add(lblDrop);
        filePanel.Controls.Add(btnOpen);
        filePanel.Controls.Add(lblRadio);

        //
        // PIN PANEL
        //

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

            Font = new Font(
                "Segoe UI",
                9F,
                FontStyle.Bold),

            AutoSize = true,

            Location = new Point(18, 14)
        };

        lblPin = new Label
        {
            Text = "----",

            Font = new Font(
                "Consolas",
                34F,
                FontStyle.Bold),

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

        btnChangePin = new Button
        {
            Text = "Change PIN (Experimental)",

            Width = 180,
            Height = 32,

            Location = new Point(395, 16),

            Enabled = false
        };

        btnChangePin.Click += BtnChangePin_Click;

        pinPanel.Controls.Add(lblPinTitle);
        pinPanel.Controls.Add(lblPin);
        pinPanel.Controls.Add(lblPinVerification);
        pinPanel.Controls.Add(btnChangePin);

        //
        // COUNTER PANEL
        //

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
            Text = "ATTEMPT COUNTER",

            ForeColor = Color.DimGray,

            Font = new Font(
                "Segoe UI",
                9F,
                FontStyle.Bold),

            AutoSize = true,

            Location = new Point(18, 14)
        };

        lblCounter = new Label
        {
            Text = "Unknown",

            Font = new Font(
                "Segoe UI",
                12F,
                FontStyle.Bold),

            AutoSize = true,

            Location = new Point(18, 42)
        };

        btnReset = new Button
        {
            Text = "Reset Counter",

            Width = 165,
            Height = 36,

            Location = new Point(410, 27),

            Enabled = false
        };

        btnReset.Click += BtnReset_Click;

        statusPanel.Controls.Add(lblStatusTitle);
        statusPanel.Controls.Add(lblCounter);
        statusPanel.Controls.Add(btnReset);

        //
        // ADVANCED
        //

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

        //
        // FOOTER
        //

        lblCredits = new LinkLabel
        {
            Text = "© 2026 Veaci · GitHub",

            AutoSize = true,

            Font = new Font("Segoe UI", 9F),

            ForeColor = Color.DimGray,

            LinkColor = Color.FromArgb(70, 100, 160),

            ActiveLinkColor = Color.FromArgb(40, 70, 130),

            Location = new Point(30, 500)
        };

        lblCredits.LinkClicked += (_, _) =>
        {
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo
                {
                    FileName =
                        "https://github.com/veaciv/smart-451-highline-radio-tool",

                    UseShellExecute = true
                });
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

    //
    // OPEN FILE
    //

    private void BtnOpen_Click(
        object? sender,
        EventArgs e)
    {
        using var dialog =
            new OpenFileDialog
            {
                Title =
                    "Open M95128 EEPROM Dump",

                Filter =
                    "EEPROM binary files (*.bin)|*.bin|" +
                    "All files (*.*)|*.*"
            };

        if (dialog.ShowDialog() ==
            DialogResult.OK)
        {
            LoadEeprom(
                dialog.FileName);
        }
    }

    //
    // LOAD + ANALYZE
    //

    private void LoadEeprom(
        string path)
    {
        try
        {
            byte[] data =
                File.ReadAllBytes(path);

            EepromResult result =
                EepromAnalyzer.Analyze(data);

            _currentDump = data;
            _currentFilePath = path;
            _currentResult = result;

            if (!result.IsValid)
            {
                ShowInvalid(
                    result.Error);

                return;
            }

            lblDrop.Text =
                Path.GetFileName(path);

            lblRadio.Text =
                result.RadioId is not null
                    ? $"Bosch / Smart ID: {result.RadioId}"
                    : "16 KB M95128 dump loaded";

            //
            // PIN
            //

            if (result.PinsMatch)
            {
                lblPin.Text =
                    result.PinA;

                lblPin.ForeColor =
                    Color.FromArgb(
                        25, 110, 60);

                lblPinVerification.Text =
                    $"✓ Copy A: {result.PinA}    " +
                    $"✓ Copy B: {result.PinB}\r\n" +
                    "Both encoded PIN copies match";

                lblPinVerification.ForeColor =
                    Color.FromArgb(
                        25, 110, 60);

                btnChangePin.Enabled = true;
            }
            else
            {
                lblPin.Text = "????";

                lblPin.ForeColor =
                    Color.DarkRed;

                lblPinVerification.Text =
                    $"Copy A: {result.PinA}    " +
                    $"Copy B: {result.PinB}\r\n" +
                    "PIN copies do not match — " +
                    "do not use this code";

                lblPinVerification.ForeColor =
                    Color.DarkRed;

                btnChangePin.Enabled = false;
            }

            //
            // COUNTER
            //

            if (result.Counter == 0)
            {
                lblCounter.Text =
                    "Stored counter: 0 — already reset";

                lblCounter.ForeColor =
                    Color.FromArgb(
                        25, 110, 60);

                btnReset.Enabled = false;
            }
            else
            {
                lblCounter.Text =
                    $"Stored counter: {result.Counter}";

                lblCounter.ForeColor =
                    Color.DarkOrange;

                btnReset.Enabled =
                    result.PinsMatch;
            }

            btnAdvanced.Enabled = true;

            UpdateAdvancedInfo();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not read EEPROM file." +
                $"\r\n\r\n{ex.Message}",

                "Read Error",

                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    //
    // INVALID FILE
    //

    private void ShowInvalid(
        string error)
    {
        lblPin.Text = "----";

        lblPin.ForeColor =
            Color.Black;

        lblPinVerification.Text =
            error;

        lblPinVerification.ForeColor =
            Color.DarkRed;

        lblCounter.Text =
            "Unknown";

        lblCounter.ForeColor =
            Color.DimGray;

        btnReset.Enabled = false;
        btnChangePin.Enabled = false;
        btnAdvanced.Enabled = false;

        MessageBox.Show(
            error,

            "Unsupported EEPROM",

            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }

    //
    // CHANGE PIN
    //

    private void BtnChangePin_Click(
        object? sender,
        EventArgs e)
    {
        if (_currentDump is null ||
            _currentFilePath is null ||
            _currentResult is null ||
            !_currentResult.PinsMatch)
        {
            return;
        }

        using var dialog =
            new PinChangeForm(
                _currentResult.PinA);

        if (dialog.ShowDialog(this) !=
            DialogResult.OK)
        {
            return;
        }

        string newPin =
            dialog.NewPin;

        try
        {
            byte[] modified =
                EepromAnalyzer
                    .CreatePinChangeDump(
                        _currentDump,
                        newPin);

            EepromResult generatedResult =
                EepromAnalyzer.Analyze(
                    modified);

            //
            // VERIFY GENERATED PIN
            //

            if (!generatedResult.PinsMatch ||
                generatedResult.PinA != newPin ||
                generatedResult.PinB != newPin)
            {
                throw new InvalidOperationException(
                    "Generated EEPROM PIN " +
                    "verification failed.");
            }

            //
            // COUNTER MUST NOT CHANGE
            //

            if (generatedResult.Counter !=
                _currentResult.Counter)
            {
                throw new InvalidOperationException(
                    "Generated EEPROM unexpectedly " +
                    "changed the attempt counter.");
            }

            string directory =
                Path.GetDirectoryName(
                    _currentFilePath) ?? "";

            string baseName =
                Path.GetFileNameWithoutExtension(
                    _currentFilePath);

            //
            // Don't put the PIN in filename.
            //

            string suggestedName =
                $"{baseName}.pin-changed.bin";

            using var saveDialog =
                new SaveFileDialog
                {
                    Title =
                        "Save PIN Changed EEPROM",

                    Filter =
                        "EEPROM binary file (*.bin)|*.bin",

                    FileName =
                        suggestedName,

                    InitialDirectory =
                        directory
                };

            if (saveDialog.ShowDialog() !=
                DialogResult.OK)
            {
                return;
            }

            string sourcePath =
                Path.GetFullPath(
                    _currentFilePath);

            string destinationPath =
                Path.GetFullPath(
                    saveDialog.FileName);

            //
            // NEVER overwrite original.
            //

            if (string.Equals(
                sourcePath,
                destinationPath,
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "The modified EEPROM cannot " +
                    "overwrite the original dump.",

                    "Safety Check",

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            File.WriteAllBytes(
                destinationPath,
                modified);

            //
            // READ FILE BACK FROM DISK
            //

            byte[] verification =
                File.ReadAllBytes(
                    destinationPath);

            if (!modified.SequenceEqual(
                verification))
            {
                throw new IOException(
                    "Saved-file verification failed.");
            }

            EepromResult saved =
                EepromAnalyzer.Analyze(
                    verification);

            //
            // VERIFY SAVED PIN
            //

            if (!saved.PinsMatch ||
                saved.PinA != newPin ||
                saved.PinB != newPin)
            {
                throw new InvalidOperationException(
                    "Saved EEPROM does not decode " +
                    "to the requested PIN.");
            }

            //
            // COUNTER MUST STILL BE SAME
            //

            if (saved.Counter !=
                _currentResult.Counter)
            {
                throw new InvalidOperationException(
                    "Saved EEPROM unexpectedly changed " +
                    "the attempt counter.");
            }

            MessageBox.Show(
                $"PIN change dump created successfully." +
                $"\r\n\r\n" +

                $"Original PIN: {_currentResult.PinA}\r\n" +
                $"New PIN: {newPin}\r\n\r\n" +

                $"Counter remains: {saved.Counter}\r\n" +

                $"0x03F8: " +
                $"{_currentResult.Status03F8:X2} → " +
                $"{saved.Status03F8:X2}\r\n\r\n" +

                $"Saved as:\r\n" +
                $"{destinationPath}\r\n\r\n" +

                "The original EEPROM file was not modified.",

                "PIN Change Dump Created",

                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"PIN change dump was NOT created." +
                $"\r\n\r\n{ex.Message}",

                "PIN Change Failed",

                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    //
    // COUNTER RESET
    //

    private void BtnReset_Click(
        object? sender,
        EventArgs e)
    {
        if (_currentDump is null ||
            _currentFilePath is null ||
            _currentResult is null ||
            !_currentResult.PinsMatch)
        {
            return;
        }

        using var consentForm =
            new RepairConsentForm(
                _currentResult.PinA,
                _currentResult.Counter);

        if (consentForm.ShowDialog(this) !=
            DialogResult.OK)
        {
            return;
        }

        try
        {
            byte[] resetDump =
                EepromAnalyzer
                    .CreateCounterResetDump(
                        _currentDump);

            string directory =
                Path.GetDirectoryName(
                    _currentFilePath) ?? "";

            string baseName =
                Path.GetFileNameWithoutExtension(
                    _currentFilePath);

            string suggestedName =
                $"{baseName}.counter-reset.bin";

            using var dialog =
                new SaveFileDialog
                {
                    Title =
                        "Save Counter Reset EEPROM",

                    Filter =
                        "EEPROM binary file (*.bin)|*.bin",

                    FileName =
                        suggestedName,

                    InitialDirectory =
                        directory
                };

            if (dialog.ShowDialog() !=
                DialogResult.OK)
            {
                return;
            }

            string sourceFullPath =
                Path.GetFullPath(
                    _currentFilePath);

            string destinationFullPath =
                Path.GetFullPath(
                    dialog.FileName);

            if (string.Equals(
                sourceFullPath,
                destinationFullPath,
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "The counter-reset dump cannot " +
                    "overwrite the original EEPROM file.",

                    "Safety Check",

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            File.WriteAllBytes(
                destinationFullPath,
                resetDump);

            //
            // READ BACK FROM DISK
            //

            byte[] verification =
                File.ReadAllBytes(
                    destinationFullPath);

            if (!resetDump.SequenceEqual(
                verification))
            {
                throw new IOException(
                    "Saved file verification failed.");
            }

            EepromResult resetResult =
                EepromAnalyzer.Analyze(
                    verification);

            //
            // VERIFY PIN
            //

            if (!resetResult.PinsMatch ||
                resetResult.PinA !=
                _currentResult.PinA)
            {
                throw new InvalidOperationException(
                    "PIN verification failed " +
                    "after saving counter reset dump.");
            }

            //
            // VERIFY COUNTER
            //

            if (resetResult.Counter != 0)
            {
                throw new InvalidOperationException(
                    "Counter verification failed " +
                    "after saving.");
            }

            //
            // FINAL BYTE-BY-BYTE VERIFICATION
            //

            for (int i = 0;
                 i < _currentDump.Length;
                 i++)
            {
                if (_currentDump[i] ==
                    verification[i])
                {
                    continue;
                }

                if (i !=
                    EepromAnalyzer.CounterOffset)
                {
                    throw new InvalidOperationException(
                        $"Unexpected saved-file change " +
                        $"at 0x{i:X4}.");
                }
            }

            MessageBox.Show(
                $"Counter reset dump created successfully." +
                $"\r\n\r\n" +

                $"Recovered PIN: {resetResult.PinA}\r\n" +

                $"Counter: {_currentResult.Counter} → 0\r\n\r\n" +

                $"Saved as:\r\n" +
                $"{destinationFullPath}\r\n\r\n" +

                "Only EEPROM offset 0x03F0 was modified.\r\n" +
                "Your original EEPROM file was not modified.",

                "Counter Reset Created",

                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Counter reset dump was NOT created." +
                $"\r\n\r\n{ex.Message}",

                "Safety Check Failed",

                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    //
    // ADVANCED VIEW
    //

    private void BtnAdvanced_Click(
        object? sender,
        EventArgs e)
    {
        txtAdvanced.Visible =
            !txtAdvanced.Visible;

        btnAdvanced.Text =
            txtAdvanced.Visible
                ? "Hide Advanced"
                : "Show Advanced";

        if (txtAdvanced.Visible)
        {
            Height = 750;

            lblCredits.Location =
                new Point(30, 665);
        }
        else
        {
            Height = 600;

            lblCredits.Location =
                new Point(30, 500);
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

            $"Radio ID:       " +
            $"{_currentResult.RadioId ?? "Not detected"}\r\n" +

            $"\r\n" +

            $"PIN COPY A\r\n" +

            $"Offset:         0x{EepromAnalyzer.PinAOffset:X4}\r\n" +

            $"Raw:            " +
            $"{HexBytes(_currentDump, EepromAnalyzer.PinAOffset, 4)}\r\n" +

            $"Decoded:        {_currentResult.PinA}\r\n" +

            $"\r\n" +

            $"PIN COPY B\r\n" +

            $"Offset:         0x{EepromAnalyzer.PinBOffset:X4}\r\n" +

            $"Raw:            " +
            $"{HexBytes(_currentDump, EepromAnalyzer.PinBOffset, 4)}\r\n" +

            $"Decoded:        {_currentResult.PinB}\r\n" +

            $"\r\n" +

            $"ATTEMPT COUNTER\r\n" +

            $"Offset:         0x{EepromAnalyzer.CounterOffset:X4}\r\n" +

            $"Raw value:      {_currentResult.Counter:X2}\r\n" +

            $"Decimal:        {_currentResult.Counter}\r\n" +

            $"\r\n" +

            $"PIN-CHANGE STATE\r\n" +

            $"0x03F8:         {_currentResult.Status03F8:X2}\r\n" +

            $"Code change:    set to 00 in verified samples\r\n" +

            $"\r\n" +

            $"RESEARCH BYTE\r\n" +

            $"0x03F9:         {_currentResult.Status03F9:X2}\r\n" +

            $"Meaning:        Not currently established\r\n";
    }

    private static string HexBytes(
        byte[] data,
        int offset,
        int count)
    {
        return string.Join(
            " ",
            data
                .Skip(offset)
                .Take(count)
                .Select(
                    b => b.ToString("X2")));
    }

    //
    // DRAG AND DROP
    //

    private void Form1_DragEnter(
        object? sender,
        DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(
                DataFormats.FileDrop) == true)
        {
            e.Effect =
                DragDropEffects.Copy;
        }
        else
        {
            e.Effect =
                DragDropEffects.None;
        }
    }

    private void Form1_DragDrop(
        object? sender,
        DragEventArgs e)
    {
        if (e.Data?.GetData(
                DataFormats.FileDrop)
            is not string[] files ||
            files.Length == 0)
        {
            return;
        }

        LoadEeprom(
            files[0]);
    }

    //
    // RESIZE
    //

    private void Form1_Resize(
        object? sender,
        EventArgs e)
    {
        int width =
            ClientSize.Width - 60;

        if (width < 500)
        {
            return;
        }

        foreach (Control control
                 in Controls)
        {
            if (control == pinPanel ||
                control == statusPanel ||
                control == txtAdvanced)
            {
                control.Width =
                    width;
            }
        }
    }

    //
    // DESIGNER LOAD EVENT
    //

    private void Form1_Load(
        object sender,
        EventArgs e)
    {
    }
}