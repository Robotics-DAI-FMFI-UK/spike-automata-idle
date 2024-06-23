using System;
using System.Drawing;
using Newtonsoft.Json;

namespace State_Automata_IDLE.Programs
{
    public class Tasks : Script
    {
        [field: NonSerialized] public new string ButtonText = "T";
        public string NewText { get; set; }  = "";

        [JsonConstructor]
        public Tasks(Point point, string newText): base(point, newText)
        {
            Button.Text = ButtonText;
        }
        public Tasks(MainForm mainForm, int x, int y) : base(mainForm, x, y)
        {
            Text = NewText;
            Button.Text = ButtonText;
        }
    }
}