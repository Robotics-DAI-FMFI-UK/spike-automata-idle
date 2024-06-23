using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using State_Automata_IDLE.Programs;

namespace State_Automata_IDLE.States
{
    public class State
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Name { get; set; }
        public string ActualAutomata { get; set; }
        public string SubAutomata { get; set; }
        public Tasks Tasks { get; set; }
        public string Text { get; set; }
        [field: NonSerialized] public MainForm MainForm;
        [field: NonSerialized] public const int Radius = 50;

        [JsonConstructor]
        public State(int positionX, int positionY, string name, string actualAutomata, string subAutomata, Tasks tasks,
            string text)
        {
            PositionX = positionX;
            PositionY = positionY;
            Name = name;
            ActualAutomata = actualAutomata;
            SubAutomata = subAutomata;
            Tasks = tasks;
            Text = text;
        }

        protected State(MainForm mainForm, string actualAutomata, int x, int y, string name = "Unnamed")
        {
            MainForm = mainForm;
            ActualAutomata = actualAutomata;
            PositionX = x;
            PositionY = y;
            Name = name;
        }

        public State(State state)
        {
            PositionX = state.PositionX;
            PositionY = state.PositionY;
            Name = state.Name;
            ActualAutomata = state.ActualAutomata;
            SubAutomata = state.SubAutomata;
            Tasks = state.Tasks;
            Text = state.Text;
            MainForm = state.MainForm;
        }

        public virtual void Draw(Graphics graphics)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            graphics.DrawString(Name, new Font("", MainForm.FontSize * 3), Brushes.Black, PositionX, PositionY, sf);
            if (Tasks?.TextBox != null)
                Tasks.TextBox.Font = new Font("", MainForm.FontSize*2);
        }

        public State SelectClicked(int x, int y)
        {
            return Math.Sqrt(Math.Pow(PositionX - x, 2) + Math.Pow(PositionY - y, 2)) < Radius ? this : null;
        }

        public void Relocate(int eX, int eY)
        {
            PositionX = eX;
            PositionY = eY;
            RelocateTask(eX, eY);
            if (Name == "Init")
            {
                MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name][0].Relocate(eX, eY);
            }
            else if (IsFinite())
            {
                MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name][0].Relocate(eX, eY);
                MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name][1].Relocate(eX, eY);
            }
        }

        private void RelocateTask(int eX, int eY)
        {
            if (Tasks == null)
                return;
            Tasks.Point = new Point(eX - Script.Width / 2, eY - Script.Height / 2 + Script.Offset);
            Tasks.Button.Location = Tasks.Point;
        }

        public void Delete()
        {
            MainForm.DeleteAllTransitionWithState(this);
            MainForm.FSM.StateDict.Remove(ActualAutomata + ":" + Name);
            Tasks?.Disable();
            Tasks?.Delete();
        }

        public void Enable()
        {
            Tasks?.Enable();
            if (!MainForm.FSM.TransitionDict.ContainsKey(ActualAutomata + ":" + Name))
                return;
            foreach (Transition transition in MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name])
                transition.Enable();
        }

        public void Disable()
        {
            Tasks?.Disable();
            if (!MainForm.FSM.TransitionDict.ContainsKey(ActualAutomata + ":" + Name))
                return;
            foreach (Transition transition in MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name])
                transition.Disable();
        }

        public bool ContainsNodeOfTransition(Transition newTransition)
        {
            if (!MainForm.FSM.TransitionDict.ContainsKey(ActualAutomata + ":" + Name))
                return false;
            foreach (Transition transition in MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name])
                if (transition.From == newTransition.From && transition.To == newTransition.To)
                    return true;
            return false;
        }

        public void DeleteTransition(Transition transition)
        {
            MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name].Remove(transition);
        }

        public bool IsFinite()
        {
            return MainForm.FSM.TransitionDict.ContainsKey(ActualAutomata + ":" + Name)
                   && MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name].Count >= 1
                   && MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name][0].IsFinite();
        }

        public bool IsInit()
        {
            return MainForm.FSM.TransitionDict.ContainsKey(ActualAutomata + ":" + Name)
                   && MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name].Count >= 1
                   && MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name][0].IsInit();
        }

        public void Rename(string name)
        {
            MainForm.FSM.NamesOfStates.Remove(Name);
            MainForm.FSM.StateDict.Remove(ActualAutomata + ":" + Name);
            MainForm.FSM.AutomataDict[ActualAutomata].StatesList.Remove(Name);
            MainForm.FSM.AutomataDict[ActualAutomata].StatesList.Add(name);

            if (SubAutomata != null)
            {
                MainForm.FSM.AutomataDict[SubAutomata].Rename(name);
                SubAutomata = name;
                MainForm.FSM.AutomataDict[SubAutomata].TreeNode.Text = name;
            }

            if (MainForm.FSM.TransitionDict.ContainsKey(ActualAutomata + ":" + Name))
            {
                MainForm.FSM.TransitionDict.Add(ActualAutomata + ":" + name, new List<Transition>());
                foreach (Transition transition in MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name])
                {
                    MainForm.FSM.TransitionDict[ActualAutomata + ":" + name].Add(transition);
                    if (transition.From == Name)
                        transition.From = name;
                    if (transition.To == Name)
                        transition.To = name;
                }

                MainForm.FSM.TransitionDict.Remove(ActualAutomata + ":" + Name);
            }

            foreach (string nameOfState in MainForm.FSM.AutomataDict[ActualAutomata].StatesList)
            {
                if (!MainForm.FSM.TransitionDict.ContainsKey(ActualAutomata + ":" + nameOfState))
                    continue;
                foreach (Transition transition in MainForm.FSM.TransitionDict[ActualAutomata + ":" + nameOfState])
                {
                    if (transition.From == Name)
                        transition.From = name;
                    if (transition.To == Name)
                        transition.To = name;
                }
            }

            Name = name;
            MainForm.FSM.NamesOfStates.Add(Name);
            MainForm.FSM.StateDict.Add(ActualAutomata + ":" + Name, this);
        }

        public void DeleteAllTransitionWithState(State inputState)
        {
            if (!MainForm.FSM.TransitionDict.ContainsKey(ActualAutomata + ":" + Name))
                return;
            for (int i = 0; i < MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name].Count;)
            {
                Transition transition = MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name][i];
                if ((transition.From == inputState.Name || transition.To == inputState.Name)
                    && inputState.ActualAutomata == ActualAutomata)
                {
                    MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name].Remove(transition);
                    transition.Disable();
                }
                else
                    i++;
            }

            if (MainForm.FSM.TransitionDict[ActualAutomata + ":" + Name].Count == 0)
                MainForm.FSM.TransitionDict.Remove(ActualAutomata + ":" + Name);
        }
    }
}