using System.Drawing;

namespace SmartHighlineTool;

public sealed class PinChangeForm : Form
{
    private readonly TextBox txtPin;
    private readonly TextBox txtConfirm;

    private readonly CheckBox chkAccept;

    private readonly Button btnCreate;

    private readonly Label lblValidation;

    public string NewPin =>
        txtPin.Text;

    public PinChangeForm(
        string currentPin)
    {
        Text =
            "Experimental Radio PIN Change";

        ClientSize =
            new Size(590, 475);

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
                "Change Radio PIN",

            Font = new Font(
                "Segoe UI",
                17F,
                FontStyle.Bold),

            AutoSize = true,

            Location =
                new Point(25, 20)
        };

        var lblExperimental =
            new Label
            {
                Text =
                    "Experimental EEPROM modification",

                ForeColor =
                    Color.DarkOrange,

                Font = new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold),

                AutoSize = true,

                Location =
                    new Point(28, 59)
            };

        var warning = new Label
        {
            Text =
                "This creates a modified COPY of the EEPROM. " +
                "It does not program the radio directly.\r\n\r\n" +

                "The PIN encoding has been independently verified " +
                "against multiple known EEPROM/code pairs and  " +
                "7354 → 1111 example.\r\n\r\n" +

                "Compatibility with every radio or firmware revision " +
                "is not guaranteed. Always retain the original EEPROM.",

            Location =
                new Point(28, 92),

            Size =
                new Size(530, 105),

            ForeColor =
                Color.DimGray
        };

        var lblCurrent =
            new Label
            {
                Text =
                    $"Current PIN: {currentPin}",

                Font = new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Bold),

                AutoSize = true,

                Location =
                    new Point(28, 210)
            };

        var lblNew =
            new Label
            {
                Text = "New PIN",

                AutoSize = true,

                Location =
                    new Point(28, 252)
            };

        txtPin =
            new TextBox
            {
                Location =
                    new Point(155, 247),

                Width = 120,

                Font = new Font(
                    "Consolas",
                    14F,
                    FontStyle.Bold),

                MaxLength = 4
            };

        var lblConfirm =
            new Label
            {
                Text = "Confirm PIN",

                AutoSize = true,

                Location =
                    new Point(28, 295)
            };

        txtConfirm =
            new TextBox
            {
                Location =
                    new Point(155, 290),

                Width = 120,

                Font = new Font(
                    "Consolas",
                    14F,
                    FontStyle.Bold),

                MaxLength = 4
            };

        txtPin.KeyPress +=
            DigitsOnly_KeyPress;

        txtConfirm.KeyPress +=
            DigitsOnly_KeyPress;

        txtPin.TextChanged +=
            ValidateForm;

        txtConfirm.TextChanged +=
            ValidateForm;

        lblValidation =
            new Label
            {
                Text =
                    "Enter a four-digit PIN.",

                ForeColor =
                    Color.DimGray,

                AutoSize = true,

                Location =
                    new Point(295, 260)
            };

        chkAccept =
            new CheckBox
            {
                Text =
                    "I confirm that I am authorized to service this " +
                    "equipment and accept responsibility for using " +
                    "the generated EEPROM file.",

                Location =
                    new Point(28, 345),

                Size =
                    new Size(525, 50)
            };

        chkAccept.CheckedChanged +=
            ValidateForm;

        var btnCancel =
            new Button
            {
                Text = "Cancel",

                Size =
                    new Size(100, 38),

                Location =
                    new Point(325, 415),

                DialogResult =
                    DialogResult.Cancel
            };

        btnCreate =
            new Button
            {
                Text =
                    "Create PIN Change Dump",

                Size =
                    new Size(165, 38),

                Location =
                    new Point(437, 415),

                Enabled = false,

                DialogResult =
                    DialogResult.OK
            };

        Controls.Add(lblTitle);
        Controls.Add(lblExperimental);
        Controls.Add(warning);
        Controls.Add(lblCurrent);

        Controls.Add(lblNew);
        Controls.Add(txtPin);

        Controls.Add(lblConfirm);
        Controls.Add(txtConfirm);

        Controls.Add(lblValidation);

        Controls.Add(chkAccept);

        Controls.Add(btnCancel);
        Controls.Add(btnCreate);

        AcceptButton =
            btnCreate;

        CancelButton =
            btnCancel;
    }

    private void DigitsOnly_KeyPress(
        object? sender,
        KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) &&
            !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void ValidateForm(
        object? sender,
        EventArgs e)
    {
        bool validLength =
            txtPin.Text.Length == 4;

        bool digits =
            txtPin.Text.All(char.IsDigit);

        bool matches =
            txtPin.Text ==
            txtConfirm.Text;

        if (!validLength || !digits)
        {
            lblValidation.Text =
                "PIN must contain exactly 4 digits.";

            lblValidation.ForeColor =
                Color.DarkOrange;

            btnCreate.Enabled = false;

            return;
        }

        if (!matches)
        {
            lblValidation.Text =
                "PIN entries do not match.";

            lblValidation.ForeColor =
                Color.DarkRed;

            btnCreate.Enabled = false;

            return;
        }

        lblValidation.Text =
            "✓ PIN entries match";

        lblValidation.ForeColor =
            Color.FromArgb(
                25, 110, 60);

        btnCreate.Enabled =
            chkAccept.Checked;
    }
}