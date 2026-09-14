using System.Drawing;

namespace SmartHighlineTool;

public sealed class RepairConsentForm : Form
{
    public RepairConsentForm(
        string pin,
        byte state03F0,
        byte state03F8,
        byte state03F9)
    {
        Text = Localization.T("dialog.repairTitle");

        ClientSize =
            new Size(620, 610);

        FormBorderStyle =
            FormBorderStyle.FixedDialog;

        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;

        StartPosition =
            FormStartPosition.CenterParent;

        Font =
            new Font("Segoe UI", 10F);

        BackColor =
            Color.White;

        var lblTitle = new Label
        {
            Text =
                Localization.T("dialog.repairHeading"),

            Font = new Font(
                "Segoe UI",
                17F,
                FontStyle.Bold),

            AutoSize = true,

            Location =
                new Point(25, 20)
        };

        var lblSubtitle = new Label
        {
            Text =
                Localization.T("dialog.repairSubtitle"),

            ForeColor =
                Color.DimGray,

            AutoSize = true,

            Location =
                new Point(28, 58)
        };

        var warningBox = new TextBox
        {
            Location =
                new Point(28, 95),

            Size =
                new Size(564, 300),

            Multiline = true,
            ReadOnly = true,

            BorderStyle =
                BorderStyle.FixedSingle,

            BackColor =
                Color.FromArgb(
                    248, 248, 248),

            Font =
                new Font(
                    "Segoe UI",
                    10F),

            ScrollBars =
                ScrollBars.Vertical,

            Text = Localization.T("dialog.repairWarning")
        };

        var detailsBox = new Label
        {
            Location =
                new Point(28, 410),

            Size =
                new Size(564, 80),

            BorderStyle =
                BorderStyle.FixedSingle,

            BackColor =
                Color.FromArgb(
                    245, 246, 248),

            Padding =
                new Padding(10),

            Font =
                new Font(
                    "Consolas",
                    9.5F),

            Text =
                Localization.T("dialog.recoveredPin", pin) + "\r\n" +
                Localization.T("dialog.stateDetails", state03F0, state03F8, state03F9)
        };

        var chkAccept =
            new CheckBox
            {
                Location =
                    new Point(28, 505),

                Size =
                    new Size(560, 50),

                AutoSize = false,

                Text = Localization.T("dialog.repairAuthorization")
            };

        var btnCancel =
            new Button
            {
                Text = Localization.T("dialog.cancel"),

                Size =
                    new Size(100, 38),

                Location =
                    new Point(305, 560),

                DialogResult =
                    DialogResult.Cancel
            };

        var btnCreate =
            new Button
            {
                Text =
                    Localization.T("dialog.createRepair"),

                Size =
                    new Size(175, 38),

                Location =
                    new Point(417, 560),

                Enabled = false,

                DialogResult =
                    DialogResult.OK
            };

        chkAccept.CheckedChanged +=
            (_, _) =>
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

        AcceptButton =
            btnCreate;

        CancelButton =
            btnCancel;
    }
}