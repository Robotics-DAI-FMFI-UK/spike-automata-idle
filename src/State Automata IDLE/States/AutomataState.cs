using System.Drawing;
using Newtonsoft.Json;
using State_Automata_IDLE.Programs;

namespace State_Automata_IDLE.States
{
    public class AutomataState : State
    {
        [JsonConstructor]
        public AutomataState(int positionX, int positionY, string name, string actualAutomata, string subAutomata, Tasks tasks, string text)
            : base(positionX, positionY, name, actualAutomata, subAutomata, tasks, text)
        {
        }
        public AutomataState(MainForm mainForm, int x, int y, Automata actualAutomata, Automata subAutomata,
            string name = "Unnamed") : base(mainForm, actualAutomata.Name, x, y, name)
        {
            SubAutomata = subAutomata.Name;
        }
        
        public AutomataState(State state) : base(state)
        {
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            int r = Radius * 2;
            int x = PositionX - Radius;
            int y = PositionY - Radius;
            graphics.DrawEllipse(new Pen(Color.Coral, 10), x, y, r, r);
        }
    }
}