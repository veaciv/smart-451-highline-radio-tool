using System.Drawing;

namespace SmartHighlineTool;

public sealed class RepairConsentForm : Form
{
    public RepairConsentForm(
        string pin,
        byte counter)
    {
        Text = "EEPROM Counter Reset";

        ClientSize = new Size(620, 600);

        FormBorderStyle =
            FormBorderStyle.FixedDialog;

        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;

        StartPosition =
            FormStartPosition.CenterParent;

        Font =
            new Font("Segoe UI", 10F);

        BackColor = Color.White;

        var lblTitle = new Label
        {
            Text = "EEPROM Attempt Counter Reset",
            Font = new Font(
                "Segoe UI",
                17F,
                FontStyle.Bold),

            AutoSize = true,
            Location = new Point(25, 20)
        };

        var lblSubtitle = new Label
        {
            Text =
                "Please read before creating " +
                "a modified EEPROM file.",

            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(28, 58)
        };

        var warningBox = new TextBox
        {
            Location = new Point(28, 95),
            Size = new Size(564, 295),

            Multiline = true,
            ReadOnly = true,

            BorderStyle =
                BorderStyle.FixedSingle,

            BackColor =
                Color.FromArgb(248, 248, 248),

            Font =
                new Font("Segoe UI", 10F),

            ScrollBars =
                ScrollBars.Vertical,

            Text =
                "This operation creates a modified COPY " +
                "of your EEPROM dump.\r\n\r\n" +

                "The original file will NOT be modified.\r\n\r\n" +

                "The generated file resets the known EEPROM " +
                "attempt counter at offset 0x03F0 to 00.\r\n\r\n" +

                "This counter location was independently confirmed " +
                "through comparison of known-good Smart Highline  " +
                "EEPROM example.\r\n\r\n" +

                "Compatibility with every radio, firmware version, " +
                "or EEPROM layout is not guaranteed. Incorrect " +
                "EEPROM data or programming may make the radio " +
                "unusable.\r\n\r\n" +

                "Before continuing:\r\n\r\n" +

                "• Keep a verified untouched backup of the " +
                "original EEPROM.\r\n" +

                "• Confirm that the radio and EEPROM are " +
                "compatible.\r\n" +

                "• Verify the EEPROM after programming.\r\n" +

                "• Use this function only on equipment you own " +
                "or are authorized to service.\r\n\r\n" +

                "This software does not program EEPROM hardware " +
                "directly."
        };

        var detailsBox = new Label
        {
            Location = new Point(28, 405),
            Size = new Size(564, 75),

            BorderStyle =
                BorderStyle.FixedSingle,

            BackColor =
                Color.FromArgb(245, 246, 248),

            Padding = new Padding(10),

            Font =
                new Font("Consolas", 9.5F),

            Text =
                $"Recovered PIN: {pin}\r\n" +
                $"Stored counter: {counter} (0x{counter:X2})\r\n" +
                $"Modification: 0x03F0  {counter:X2} → 00"
        };

        var chkAccept = new CheckBox
        {
            Location = new Point(28, 495),
            Size = new Size(560, 50),

            AutoSize = false,

            Text =
                "I confirm that I am authorized to service " +
                "this equipment and accept responsibility " +
                "for programming and using the generated file."
        };

        var btnCancel = new Button
        {
            Text = "Cancel",

            Size = new Size(100, 38),
            Location = new Point(305, 550),

            DialogResult =
                DialogResult.Cancel
        };

        var btnCreate = new Button
        {
            Text = "Create Counter Reset",

            Size = new Size(175, 38),
            Location = new Point(417, 550),

            Enabled = false,

            DialogResult =
                DialogResult.OK
        };

        chkAccept.CheckedChanged += (_, _) =>
        {
            btnCreate.Enabled =
                chkAccept.Checked;
        };

        Controls.Add(lblTitle);
        Controls.Add(lblSubtitle);
        Controls.Add(warningBox);
        Controls.Add(detailsBox);
        Controls.Add(chkAccept);
        Controls.Add(btnCancel);
        Controls.Add(btnCreate);

        AcceptButton = btnCreate;
        CancelButton = btnCancel;
    }
}