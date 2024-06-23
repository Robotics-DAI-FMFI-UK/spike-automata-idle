using System;
using System.Drawing;
using Newtonsoft.Json;

namespace State_Automata_IDLE.Programs
{
    public class Conditions : Script
    {
        [field: NonSerialized] public new string ButtonText = "C";
        public string NewText { get; set; } = "";

        [JsonConstructor]
        public Conditions(Point point, string newText) : base(point, newText)
        {
            Button.Text = ButtonText;
        }

        public Conditions(MainForm mainForm, int x, int y) : base(mainForm, x, y)
        {
            Text = NewText;
            Button.Text = ButtonText;
        }
    }
}