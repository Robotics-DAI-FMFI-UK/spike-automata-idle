using System.Drawing;
using Newtonsoft.Json;
using State_Automata_IDLE.Programs;

namespace State_Automata_IDLE.States
{
    public class BlankState : State

    {
        [JsonConstructor]
        public BlankState(int positionX, int positionY, string name, string actualAutomata, string subAutomata, Tasks tasks, string text)
            : base(positionX, positionY, name, actualAutomata, subAutomata, tasks, text)
        {
        }

        public BlankState(MainForm mainForm, string actualAutomata, int x, int y, string name = "Unnamed")
            : base(mainForm, actualAutomata, x, y, name)
        {
        }
        
        public BlankState(State state) : base(state)
        {
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            int r = Radius * 2;
            int x = PositionX - Radius;
            int y = PositionY - Radius;
            graphics.DrawEllipse(new Pen(Color.Aqua, 10), x, y, r, r);
        }
    }
}