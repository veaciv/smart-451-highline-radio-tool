namespace SmartHighlineTool;

partial class Form1
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(
        bool disposing)
    {
        if (disposing &&
            components is not null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components =
            new System.ComponentModel.Container();

        AutoScaleDimensions =
            new SizeF(7F, 15F);

        AutoScaleMode =
            AutoScaleMode.Font;

        ClientSize =
            new Size(800, 450);

        Name = "Form1";

        Text =
            "Smart 451 Highline Radio Tool";
    }
}