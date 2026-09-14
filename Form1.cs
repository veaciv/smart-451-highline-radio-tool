using System.Drawing;

namespace SmartHighlineTool;

public partial class Form1 : Form
{
    private byte[]? _currentDump;
    private string? _currentFilePath;
    private EepromResult? _currentResult;

    private readonly Label lblTitle;
    private readonly Label lblSubtitle;
    private readonly Label lblDrop;
    private readonly Label lblRadio;
    private readonly Label lblPinTitle;
    private readonly Label lblStatusTitle;

    private readonly Label lblPin;
    private readonly Label lblPinVerification;

    private readonly Label lblState;

    private readonly Button btnOpen;
    private readonly Button btnReset;
    private readonly Button btnChangePin;
    private readonly Button btnAdvanced;

    private readonly TextBox txtAdvanced;

    private readonly Panel pinPanel;
    private readonly Panel statusPanel;

    private readonly LinkLabel lblCredits;
    private readonly ComboBox cmbLanguage;

    public Form1()
    {
        InitializeComponent();

        Text = Localization.T("app.title");

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

        lblTitle = new Label
        {
            Text = Localization.T("header.title"),

            Font = new Font(
                "Segoe UI",
                20F,
                FontStyle.Bold),

            AutoSize = true,

            Location = new Point(28, 22)
        };

        lblSubtitle = new Label
        {
            Text = Localization.T("header.subtitle"),

            ForeColor = Color.DimGray,

            AutoSize = true,

            Location = new Point(31, 61)
        };

        cmbLanguage = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 125,
            Location = new Point(500, 24)
        };

        cmbLanguage.Items.AddRange(new object[]
        {
            "English", "Français", "Русский", "Română", "Español"
        });
        cmbLanguage.SelectedIndex = 0;
        cmbLanguage.SelectedIndexChanged += CmbLanguage_SelectedIndexChanged;

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
            Text = Localization.T("drop.file"),

            Font = new Font(
                "Segoe UI",
                11F,
                FontStyle.Bold),

            AutoSize = true,

            Location = new Point(18, 17)
        };

        btnOpen = new Button
        {
            Text = Localization.T("open.eeprom"),

            Width = 130,
            Height = 32,

            Location = new Point(18, 46)
        };

        btnOpen.Click += BtnOpen_Click;

        lblRadio = new Label
        {
            Text = Localization.T("status.none"),

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

        lblPinTitle = new Label
        {
            Text = Localization.T("radio.code"),

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
            Text = Localization.T("pin.decode"),

            ForeColor = Color.DimGray,

            AutoSize = true,

            Location = new Point(175, 67)
        };

        btnChangePin = new Button
        {
            Text = Localization.T("pin.change"),

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
        // LOCK / ERROR STATE PANEL
        //

        statusPanel = new Panel
        {
            Location = new Point(30, 344),

            Width = 600,
            Height = 88,

            BackColor = Color.White,

            BorderStyle = BorderStyle.FixedSingle
        };

        lblStatusTitle = new Label
        {
            Text = Localization.T("state.title"),

            ForeColor = Color.DimGray,

            Font = new Font(
                "Segoe UI",
                9F,
                FontStyle.Bold),

            AutoSize = true,

            Location = new Point(18, 14)
        };

        lblState = new Label
        {
            Text = Localization.T("state.unknown"),

            Font = new Font(
                "Segoe UI",
                11F,
                FontStyle.Bold),

            AutoSize = true,

            Location = new Point(18, 42)
        };

        btnReset = new Button
        {
            Text = Localization.T("reset.create"),

            Width = 175,
            Height = 36,

            Location = new Point(400, 27),

            Enabled = false
        };

        btnReset.Click += BtnReset_Click;

        statusPanel.Controls.Add(lblStatusTitle);
        statusPanel.Controls.Add(lblState);
        statusPanel.Controls.Add(btnReset);

        //
        // ADVANCED
        //

        btnAdvanced = new Button
        {
            Text = Localization.T("advanced.show"),

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
        Controls.Add(cmbLanguage);

        Controls.Add(filePanel);

        Controls.Add(pinPanel);
        Controls.Add(statusPanel);

        Controls.Add(btnAdvanced);
        Controls.Add(txtAdvanced);

        Controls.Add(lblCredits);

        Resize += Form1_Resize;

        ApplyLanguage();
    }

    private void CmbLanguage_SelectedIndexChanged(object? sender, EventArgs e)
    {
        Localization.CurrentLanguage = (AppLanguage)cmbLanguage.SelectedIndex;
        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        Text = Localization.T("app.title");
        lblTitle.Text = Localization.T("header.title");
        lblSubtitle.Text = Localization.T("header.subtitle");
        lblDrop.Text = _currentFilePath is null
            ? Localization.T("drop.file")
            : Path.GetFileName(_currentFilePath);
        btnOpen.Text = Localization.T("open.eeprom");
        lblPinTitle.Text = Localization.T("radio.code");
        btnChangePin.Text = Localization.T("pin.change");
        lblStatusTitle.Text = Localization.T("state.title");
        btnReset.Text = Localization.T("reset.create");
        btnAdvanced.Text = txtAdvanced.Visible
            ? Localization.T("advanced.hide")
            : Localization.T("advanced.show");

        if (_currentResult is null)
        {
            lblRadio.Text = Localization.T("status.none");
            lblPinVerification.Text = Localization.T("pin.decode");
            lblState.Text = Localization.T("state.unknown");
        }
        else
        {
            lblRadio.Text = _currentResult.RadioId is not null
                ? Localization.T("radio.id", _currentResult.RadioId)
                : Localization.T("status.loaded");
            UpdateLocalizedResultText();
            UpdateAdvancedInfo();
        }
    }

    private void UpdateLocalizedResultText()
    {
        if (_currentResult is null)
        {
            return;
        }

        lblPinVerification.Text = _currentResult.PinsMatch
            ? $"✓ Copy A: {_currentResult.PinA}    ✓ Copy B: {_currentResult.PinB}\r\n{Localization.T("pin.match")}"
            : $"Copy A: {_currentResult.PinA}    Copy B: {_currentResult.PinB}\r\n{Localization.T("pin.mismatch")}";

        if (_currentResult.ResetValuesAlreadyPresent)
        {
            lblState.Text = Localization.T("state.present");
        }
        else
        {
            lblState.Text = Localization.T(
                "state.values",
                _currentResult.State03F0,
                _currentResult.State03F8,
                _currentResult.State03F9);
        }
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
                Title = Localization.T("dialog.open"),

                Filter =
                    Localization.T("dialog.filterAll")
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

            CreateClientBackup(path);

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

            lblRadio.Text = result.RadioId is not null
                ? Localization.T("radio.id", result.RadioId)
                : Localization.T("status.loaded");

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
                    Localization.T("pin.match");

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
                    Localization.T("pin.mismatch");

                lblPinVerification.ForeColor =
                    Color.DarkRed;

                btnChangePin.Enabled = false;
            }

            //
            // LOCK / ERROR STATE
            //

            if (result.ResetValuesAlreadyPresent)
            {
                lblState.Text =
                    Localization.T("state.present");

                lblState.ForeColor =
                    Color.FromArgb(
                        25, 110, 60);

                btnReset.Enabled = false;
            }
            else
            {
                lblState.Text = Localization.T(
                    "state.values",
                    result.State03F0,
                    result.State03F8,
                    result.State03F9);

                lblState.ForeColor =
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
                Localization.T("error.read", ex.Message),

                Localization.T("error.readTitle"),

                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static void CreateClientBackup(string sourcePath)
    {
        string backupDirectory =
            Path.Combine(AppContext.BaseDirectory, "Backups");

        Directory.CreateDirectory(backupDirectory);

        string baseName =
            Path.GetFileNameWithoutExtension(sourcePath);

        string timestamp =
            DateTime.Now.ToString("yyyyMMdd-HHmmssfff");

        string backupPath =
            Path.Combine(
                backupDirectory,
                $"{baseName}.back.{timestamp}.bin");

        using FileStream backup =
            new FileStream(
                backupPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None);

        using FileStream source =
            new FileStream(
                sourcePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

        source.CopyTo(backup);
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

        lblState.Text =
            Localization.T("state.unknown");

        lblState.ForeColor =
            Color.DimGray;

        btnReset.Enabled = false;
        btnChangePin.Enabled = false;
        btnAdvanced.Enabled = false;

        MessageBox.Show(
            error,

            Localization.T("error.unsupportedTitle"),

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
            // PIN CHANGE MUST NOT ALTER
            // 03F0 OR 03F9.
            //

            if (generatedResult.State03F0 !=
                    _currentResult.State03F0 ||
                generatedResult.State03F9 !=
                    _currentResult.State03F9)
            {
                throw new InvalidOperationException(
                    "Generated EEPROM unexpectedly " +
                    "changed unrelated state bytes.");
            }

            string directory =
                Path.GetDirectoryName(
                    _currentFilePath) ?? "";

            string baseName =
                Path.GetFileNameWithoutExtension(
                    _currentFilePath);

            //
            // Do not expose PIN in filename.
            //

            string suggestedName =
                $"{baseName}.pin-changed.bin";

            using var saveDialog =
                new SaveFileDialog
                {
                    Title =
                        Localization.T("dialog.savePin"),

                    Filter =
                        Localization.T("dialog.filter"),

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
            // NEVER OVERWRITE ORIGINAL.
            //

            if (string.Equals(
                sourcePath,
                destinationPath,
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    Localization.T("warning.noOverwritePin"),

                    Localization.T("error.safety"),

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            File.WriteAllBytes(
                destinationPath,
                modified);

            //
            // READ SAVED FILE BACK FROM DISK
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
            // VERIFY 03F0 / 03F9 UNCHANGED
            //

            if (saved.State03F0 !=
                    _currentResult.State03F0 ||
                saved.State03F9 !=
                    _currentResult.State03F9)
            {
                throw new InvalidOperationException(
                    "Saved EEPROM unexpectedly changed " +
                    "unrelated state bytes.");
            }

            MessageBox.Show(
                Localization.T(
                    "success.pin",
                    _currentResult.PinA,
                    newPin,
                    saved.State03F0,
                    _currentResult.State03F8,
                    saved.State03F8,
                    saved.State03F9,
                    destinationPath),

                Localization.T("success.pinTitle"),

                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                Localization.T("error.pinChange", ex.Message),

                Localization.T("error.pinChangeTitle"),

                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    //
    // REPAIR / RESET
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
                _currentResult.State03F0,
                _currentResult.State03F8,
                _currentResult.State03F9);

        if (consentForm.ShowDialog(this) !=
            DialogResult.OK)
        {
            return;
        }

        try
        {
            byte[] resetDump =
                EepromAnalyzer
                    .CreateResetDump(
                        _currentDump);

            string directory =
                Path.GetDirectoryName(
                    _currentFilePath) ?? "";

            string baseName =
                Path.GetFileNameWithoutExtension(
                    _currentFilePath);

            string suggestedName =
                $"{baseName}.repair-reset.bin";

            using var dialog =
                new SaveFileDialog
                {
                    Title =
                        Localization.T("dialog.saveReset"),

                    Filter =
                        Localization.T("dialog.filter"),

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

            //
            // NEVER OVERWRITE ORIGINAL.
            //

            if (string.Equals(
                sourceFullPath,
                destinationFullPath,
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    Localization.T("warning.noOverwriteReset"),

                    Localization.T("error.safety"),

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            File.WriteAllBytes(
                destinationFullPath,
                resetDump);

            //
            // READ SAVED FILE BACK
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
            // VERIFY PIN UNCHANGED
            //

            if (!resetResult.PinsMatch ||
                resetResult.PinA !=
                    _currentResult.PinA ||
                resetResult.PinB !=
                    _currentResult.PinB)
            {
                throw new InvalidOperationException(
                    "PIN verification failed " +
                    "after saving repair/reset dump.");
            }

            //
            // VERIFY RESET STATE
            //

            if (!resetResult.ResetValuesAlreadyPresent)
            {
                throw new InvalidOperationException(
                    "Repair/reset state verification failed.");
            }

            //
            // FINAL BYTE-BY-BYTE VERIFICATION
            //

            var allowedOffsets =
                new HashSet<int>
                {
                    EepromAnalyzer.State03F0Offset,
                    EepromAnalyzer.State03F8Offset,
                    EepromAnalyzer.State03F9Offset
                };

            for (int i = 0;
                 i < _currentDump.Length;
                 i++)
            {
                if (_currentDump[i] ==
                    verification[i])
                {
                    continue;
                }

                if (!allowedOffsets.Contains(i))
                {
                    throw new InvalidOperationException(
                        $"Unexpected saved-file change " +
                        $"at 0x{i:X4}.");
                }
            }

            MessageBox.Show(
                    Localization.T(
                        "success.reset",
                        resetResult.PinA,
                        _currentResult.State03F0,
                        _currentResult.State03F8,
                        _currentResult.State03F9,
                        destinationFullPath),

                Localization.T("success.resetTitle"),

                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                Localization.T("error.reset", ex.Message),

                Localization.T("error.resetTitle"),

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
                ? Localization.T("advanced.hide")
                : Localization.T("advanced.show");

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
            Localization.T("advanced.file", _currentDump.Length) + "\r\n" +

            Localization.T("advanced.sha", _currentResult.Sha256) + "\r\n" +

            Localization.T(
                "advanced.radio",
                _currentResult.RadioId ?? Localization.T("advanced.notDetected")) + "\r\n" +

            $"\r\n" +

            Localization.T("advanced.copyA") + "\r\n" +

            Localization.T("advanced.offset", EepromAnalyzer.PinAOffset) + "\r\n" +

            Localization.T("advanced.raw", HexBytes(_currentDump, EepromAnalyzer.PinAOffset, 4)) + "\r\n" +

            Localization.T("advanced.decoded", _currentResult.PinA) + "\r\n" +

            $"\r\n" +

            Localization.T("advanced.copyB") + "\r\n" +

            Localization.T("advanced.offset", EepromAnalyzer.PinBOffset) + "\r\n" +

            Localization.T("advanced.raw", HexBytes(_currentDump, EepromAnalyzer.PinBOffset, 4)) + "\r\n" +

            Localization.T("advanced.decoded", _currentResult.PinB) + "\r\n" +

            $"\r\n" +

            Localization.T("advanced.stateBytes") + "\r\n" +

            $"0x03F0:         {_currentResult.State03F0:X2}\r\n" +
            $"0x03F8:         {_currentResult.State03F8:X2}\r\n" +
            $"0x03F9:         {_currentResult.State03F9:X2}\r\n" +

            $"\r\n" +

            Localization.T("advanced.verified") + "\r\n" +

            $"0x03F0 → 00\r\n" +
            $"0x03F8 → 00\r\n" +
            $"0x03F9 → 00\r\n" +

            $"\r\n" +

            Localization.T("advanced.unknownMeaning") + "\r\n";
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