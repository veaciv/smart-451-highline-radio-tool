using System.Drawing;
using System.Windows.Forms;

namespace SmartHighlineTool;

public sealed class RepairConsentForm : Form
{
    public RepairConsentForm(
        string pin,
        byte status03F0,
        byte status03F8,
        byte status03F9)
    {
        Text = "Experimental EEPROM Repair";
        ClientSize = new Size(620, 590);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.White;

        var lblTitle = new Label
        {
            Text = "Experimental EEPROM Repair / Reset",
            Font = new Font("Segoe UI", 17F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(25, 20)
        };

        var lblSubtitle = new Label
        {
            Text = "Please read before creating a modified EEPROM file.",
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(28, 58)
        };

        var warningBox = new TextBox
        {
            Location = new Point(28, 95),
            Size = new Size(564, 285),
            Multiline = true,
            ReadOnly = true,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(248, 248, 248),
            Font = new Font("Segoe UI", 10F),
            ScrollBars = ScrollBars.Vertical,
            Text =
                "This operation creates a modified COPY of your EEPROM dump.\r\n\r\n" +

                "The original file will NOT be modified.\r\n\r\n" +

                "The generated file changes known lock/error-state bytes based on " +
                "independently researched EEPROM examples.\r\n\r\n" +

                "Compatibility with every radio, firmware version, or EEPROM layout " +
                "is not guaranteed. Incorrect EEPROM data or programming may make " +
                "the radio unusable.\r\n\r\n" +

                "Before continuing:\r\n\r\n" +

                "• Keep a verified untouched backup of the original EEPROM.\r\n" +
                "• Confirm that the detected radio and EEPROM are compatible.\r\n" +
                "• Verify the EEPROM after programming.\r\n" +
                "• Use this function only on equipment you own or are authorized to service.\r\n\r\n" +

                "This software does not program hardware directly. You are responsible " +
                "for deciding whether to program and use the generated file."
        };

        var detailsBox = new Label
        {
            Location = new Point(28, 397),
            Size = new Size(564, 72),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(245, 246, 248),
            Padding = new Padding(10),
            Font = new Font("Consolas", 9.5F),
            Text =
                $"Recovered PIN: {pin}\r\n" +
                $"Changes: 03F0 {status03F0:X2}→00   " +
                $"03F8 {status03F8:X2}→00   " +
                $"03F9 {status03F9:X2}→00"
        };

        var chkAccept = new CheckBox
        {
            Location = new Point(28, 485),
            Size = new Size(560, 45),
            Text =
                "I confirm that I am authorized to service this equipment and accept " +
                "responsibility for programming and using the generated file.",
            AutoSize = false
        };

        var btnCreate = new Button
        {
            Text = "Create Repair Dump",
            Size = new Size(175, 38),
            Location = new Point(417, 538),
            Enabled = false,
            DialogResult = DialogResult.OK
        };

        var btnCancel = new Button
        {
            Text = "Cancel",
            Size = new Size(100, 38),
            Location = new Point(305, 538),
            DialogResult = DialogResult.Cancel
        };

        chkAccept.CheckedChanged += (_, _) =>
        {
            btnCreate.Enabled = chkAccept.Checked;
        };

        Controls.Add(lblTitle);
        Controls.Add(lblSubtitle);
        Controls.Add(warningBox);
        Controls.Add(detailsBox);
        Controls.Add(chkAccept);
        Controls.Add(btnCreate);
        Controls.Add(btnCancel);

        AcceptButton = btnCreate;
        CancelButton = btnCancel;
    }
}