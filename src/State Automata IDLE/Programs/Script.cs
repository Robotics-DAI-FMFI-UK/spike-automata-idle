using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace State_Automata_IDLE.Programs
{
    public class Script
    {
        [field: NonSerialized] public static int Width = 20;
        [field: NonSerialized] public static int Height = 20;
        [field: NonSerialized] public static int Offset = 20;
        [field: NonSerialized] public Point Size = new Point(120, 75);
        public Point Point { get; set; }
        [field: NonSerialized] public Button Button;
        [field: NonSerialized] public MainForm MainForm;
        [field: NonSerialized] public string ButtonText = "S";
        public string Text = "";
        [field: NonSerialized] public TextBox TextBox;

        public event EventHandler ShowScript = (sender, args) =>
        {
            Script script = (Script)sender;
            script.TextBox = new TextBox();
            script.TextBox.Text = script.Text;
            script.TextBox.Multiline = true;
            script.TextBox.Visible = true;
            script.TextBox.ReadOnly = false;
            script.TextBox.Enabled = true;
            script.TextBox.Location = new Point(script.Point.X + 20, script.Point.Y + 20);
            script.TextBox.Size = new Size(script.Size);
            script.TextBox.Font = new Font("", script.MainForm.FontSize*2);
            script.MainForm.GetControl().Add(script.TextBox);
            script.TextBox.TextChanged += (o, eventArgs) => script.Text = script.TextBox.Text;
        };
        
        [JsonConstructor]
        public Script(Point point, string text)
        {
            Point = point;
            Button = new Button();
            Text = text;
            Button.Size = new Size(Width, Height);
            Button.Location = Point;
            Button.Click += Show;
            Button.Visible = true;
        }

        protected Script(MainForm mainForm, int x, int y)
        {
            MainForm = mainForm;
            Point = new Point(x - Width / 2, y - Height / 2 + Offset);
            Button = new Button();
            Button.Text = ButtonText;
            Button.Size = new Size(Width, Height);
            Button.Location = Point;
            Button.Click += Show;
            Button.Visible = true;
            MainForm.GetControl().Add(Button);
        }

        private void Show(object sender, EventArgs e)
        {
            if (TextBox != null)
            {
                MainForm.GetTreeView().Select();
                MainForm.GetControl().Remove(TextBox);
                TextBox = null;
            }
            else
            {
                ShowScript?.Invoke(this, EventArgs.Empty);
                if (TextBox != null)
                {
                    TextBox.Select();
                    TextBox.SelectedText = TextBox.SelectedText;
                }
            }
        }

        public void Delete()
        {
            MainForm.GetControl().Remove(Button);
            if (TextBox != null)
            {
                MainForm.GetControl().Remove(TextBox);
                TextBox = null;
            }
        }

        public void Disable()
        {
            Delete();
        }

        public void Enable()
        {
            MainForm.GetControl().Add(Button);
        }
    }
}