using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using State_Automata_IDLE.Programs;
using State_Automata_IDLE.States;

namespace State_Automata_IDLE
{
    public class Transition
    {
        public Color Color { get; set; } = Color.Black;
        public string From { get; set; }
        public string To { get; set; }
        public string AutomataName { get; set; }
        public string Label { get; set; } = "";
        public List<int> Points { get; set; }
        public Tasks Tasks { get; set; }
        public Conditions Conditions { get; set; }
        [field: NonSerialized] public Pen Pen = new Pen(Color.Black);
        [field: NonSerialized] public Brush Brush = new SolidBrush(Color.Black);
        [field: NonSerialized] public const int Radius = 10;
        [field: NonSerialized] public const int Corner = 75;
        [field: NonSerialized] public const int Upper = 100;
        [field: NonSerialized] public int First = -1;
        [field: NonSerialized] public int Second = -1;
        [field: NonSerialized] public MainForm MainForm;

        [JsonConstructor]
        public Transition(Color color, string from, string to, string automata, string label, List<int> points,
            Tasks tasks,
            Conditions conditions)
        {
            Color = color;
            Pen = new Pen(color);
            Brush = new SolidBrush(color);
            From = from;
            To = to;
            AutomataName = automata;
            Label = label;
            Points = points;
            Tasks = tasks;
            Conditions = conditions;
        }

        public Transition(MainForm mainForm, State from, int init = -1)
        {
            MainForm = mainForm;
            From = from.Name;
            AutomataName = from.ActualAutomata;
            if (init != -1)
                To = From;
            switch (init)
            {
                case 0:
                    CreateInitTransition();
                    break;
                case 1:
                    CreateFiniteTransition();
                    break;
                case 2:
                    CreateExitTransition();
                    break;
                default:
                    return;
            }

            RelocateScripts();
        }

        public bool IsFinite()
        {
            if (From == null)
                return false;
            return From == To && From != "Init";
        }

        public bool IsInit()
        {
            if (From == null)
                return false;
            return From == To && From == "Init";
        }

        private void CreateInitTransition()
        {
            To = From;
            Pen = new Pen(Color.Chartreuse, 2);
            Brush = new SolidBrush(Color.Chartreuse);
            Color = Color.Chartreuse;
            Points = new List<int>
            {
                MainForm.FSM.StateDict[AutomataName + ":" + To].PositionX - Corner,
                MainForm.FSM.StateDict[AutomataName + ":" + To].PositionY - Corner,
                MainForm.FSM.StateDict[AutomataName + ":" + From].PositionX,
                MainForm.FSM.StateDict[AutomataName + ":" + From].PositionY
            };
            Tasks = new Tasks(MainForm, Points[0], Points[1]);
        }

        private void CreateExitTransition()
        {
            To = From;
            Pen = new Pen(Color.Blue, 2);
            Brush = new SolidBrush(Color.Blue);
            Color = Color.Blue;
            Points = new List<int>
            {
                MainForm.FSM.StateDict[AutomataName + ":" + To].PositionX + Corner,
                MainForm.FSM.StateDict[AutomataName + ":" + To].PositionY + Corner,
                MainForm.FSM.StateDict[AutomataName + ":" + From].PositionX,
                MainForm.FSM.StateDict[AutomataName + ":" + From].PositionY
            };
            Tasks = new Tasks(MainForm, Points[0], Points[1]);
        }

        private void CreateFiniteTransition()
        {
            To = From;
            Pen = new Pen(Color.Red, 2);
            Brush = new SolidBrush(Color.Red);
            Color = Color.Red;
            Points = new List<int>
            {
                MainForm.FSM.StateDict[AutomataName + ":" + To].PositionX,
                MainForm.FSM.StateDict[AutomataName + ":" + To].PositionY - Upper,
                MainForm.FSM.StateDict[AutomataName + ":" + From].PositionX,
                MainForm.FSM.StateDict[AutomataName + ":" + From].PositionY
            };
            Tasks = new Tasks(MainForm, Points[0], Points[1]);
            Conditions = new Conditions(MainForm, Points[0], Points[1]);
        }

        public void End(State to)
        {
            if (to == null)
                return;
            To = to.Name;
            if (From == to.Name ||
                (MainForm.FSM.StateDict.ContainsKey(AutomataName + ":" + From) &&
                 MainForm.FSM.StateDict[AutomataName + ":" + From].IsFinite()) ||
                (MainForm.FSM.StateDict.ContainsKey(AutomataName + ":" + to.Name) &&
                 MainForm.FSM.StateDict[AutomataName + ":" + to.Name].IsInit()) ||
                (MainForm.FSM.StateDict.ContainsKey(AutomataName + ":" + From) &&
                 MainForm.FSM.StateDict[AutomataName + ":" + From].ContainsNodeOfTransition(this)))
                return;

            To = to.Name;
            if (!MainForm.FSM.TransitionDict.ContainsKey(AutomataName + ":" + From))
            {
                MainForm.FSM.TransitionDict.Add(AutomataName + ":" + From, new List<Transition>());
            }

            MainForm.FSM.TransitionDict[AutomataName + ":" + From].Add(this);
            Point middle = new Point(
                (MainForm.FSM.StateDict[AutomataName + ":" + From].PositionX +
                 MainForm.FSM.StateDict[AutomataName + ":" + To].PositionX) / 2,
                (MainForm.FSM.StateDict[AutomataName + ":" + From].PositionY +
                 MainForm.FSM.StateDict[AutomataName + ":" + To].PositionY) / 2);
            Points = new List<int>
            {
                MainForm.FSM.StateDict[AutomataName + ":" + From].PositionX,
                MainForm.FSM.StateDict[AutomataName + ":" + From].PositionY,
                middle.X,
                middle.Y,
                MainForm.FSM.StateDict[AutomataName + ":" + To].PositionX,
                MainForm.FSM.StateDict[AutomataName + ":" + To].PositionY
            };
            Tasks = new Tasks(MainForm, Points[2], Points[3]);
            Conditions = new Conditions(MainForm, Points[2], Points[3]);
            RelocateScripts();
        }

        public void Draw(Graphics graphics)
        {
            if (To == null)
                return;
            if (Tasks?.TextBox != null)
                Tasks.TextBox.Font = new Font("", Tasks.MainForm.FontSize * 2);
            if (Conditions?.TextBox != null)
                Conditions.TextBox.Font = new Font("", Conditions.MainForm.FontSize * 2);
            if (IsFinite() || IsInit())
                DrawSpecial(graphics);
            else
                DrawPoints(graphics);
        }

        private void DrawLabel(Graphics graphics)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            if (Tasks == null)
            {
                return;
            }

            graphics.DrawString(Label, new Font("", Tasks.MainForm.FontSize * 2), Brushes.Black, Points[2],
                Points[3] - 15, sf);
        }

        private void DrawPoints(Graphics graphics)
        {
            for (int i = 0; i < Points.Count - 2; i += 2)
            {
                if (i != 0)
                    graphics.FillEllipse(Brush, Points[i] - Radius / 2, Points[i + 1] - Radius / 2, Radius, Radius);
                if (i == 0 || i == Points.Count - 4)
                {
                    List<double> points = FoundPoints(Points[i], Points[i + 1], Points[i + 2], Points[i + 3]);
                    double x = points[0];
                    double y = points[1];
                    if (i == 0)
                        graphics.DrawLine(Pen, new Point((int)(Points[i] + x), (int)(Points[i + 1] + y)),
                            new Point(Points[i + 2], Points[i + 3]));
                    else
                        graphics.DrawLine(Pen, new Point(Points[i], Points[i + 1]),
                            new Point((int)(Points[i + 2] - x), (int)(Points[i + 3] - y)));
                }
                else
                    graphics.DrawLine(Pen, new Point(Points[i], Points[i + 1]),
                        new Point(Points[i + 2], Points[i + 3]));
            }

            DrawArrow(graphics);
            DrawLabel(graphics);
        }

        private void DrawArrow(Graphics graphics)
        {
            int i = Points.Count - 4;
            List<double> points = FoundPoints(Points[i], Points[i + 1], Points[i + 2], Points[i + 3]);
            Point end = new Point((int)(Points[i + 2] - points[0]), (int)(Points[i + 3] - points[1]));
            Point start = new Point((int)(Points[i + 2] - points[0] * 1.5), (int)(Points[i + 3] - points[1] * 1.5));
            float size = 30;
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            PointF[] point = new PointF[3];
            point[0] = end;
            point[1] = new PointF(
                end.X - size * (float)Math.Cos(angle - Math.PI / 6),
                end.Y - size * (float)Math.Sin(angle - Math.PI / 6)
            );
            point[2] = new PointF(
                end.X - size * (float)Math.Cos(angle + Math.PI / 6),
                end.Y - size * (float)Math.Sin(angle + Math.PI / 6)
            );

            graphics.FillPolygon(Brushes.Aquamarine, point);
            graphics.DrawPolygon(Pen, point);
        }

        private List<double> FoundPoints(int x1, int y1, int x2, int y2)
        {
            double x = Math.Abs(x1 - x2);
            double y = Math.Abs(y1 - y2);
            double c = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double alpha = Math.Asin(y / c);
            double fromY = Math.Sin(alpha) * Radius * 5;
            double fromX = Math.Cos(alpha) * Radius * 5;
            if (x1 > x2)
                fromX *= -1;
            if (y1 > y2)
                fromY *= -1;
            return new List<double> { fromX, fromY };
        }

        private void DrawSpecial(Graphics graphics)
        {
            List<double> points = FoundPoints(Points[0], Points[1], Points[2], Points[3]);
            double x = points[0];
            double y = points[1];
            if (Color == Color.Chartreuse)
            {
                graphics.FillEllipse(Brush, Points[0] - Radius / 2, Points[1] - Radius / 2, Radius,
                    Radius);
                graphics.DrawLine(Pen, new Point(Points[0], Points[1]),
                    new Point((int)(Points[2] - x), (int)(Points[3] - y)));
            }

            if (Color == Color.Red)
            {
                graphics.FillEllipse(Brush, Points[0] - Radius / 2, Points[1] - Radius / 2, Radius,
                    Radius);
                graphics.DrawLine(Pen, new Point(Points[0], Points[1]),
                    new Point(Points[2], (int)(Points[3] - y)));
            }

            if (Color == Color.Blue)
            {
                graphics.FillEllipse(Brush, Points[0] - Radius / 2, Points[1] - Radius / 2, Radius,
                    Radius);
                graphics.DrawLine(Pen, new Point(Points[0], Points[1]),
                    new Point((int)(Points[2] - x), (int)(Points[3] - y)));
            }
        }

        public void Relocate(int x = -1, int y = -1)
        {
            if (Color == Color.Chartreuse)
                Points = new List<int> { x - Corner, y - Corner, x, y };
            else if (Color == Color.Blue)
                Points = new List<int> { x + Corner, y + Corner, x, y };
            else if (Color == Color.Red)
                Points = new List<int> { x, y - Upper, x, y };
            else
            {
                ResetStart();
                ResetEnd();
                RelocateRequested(x, y);
            }

            RelocateScripts();
        }

        public void RelocateScripts()
        {
            if (Points.Count == 4)
            {
                int x = Points[0] - Script.Width - 2;
                int y = Points[1] - Script.Height / 2 + Script.Offset;
                if (Conditions != null)
                {
                    Conditions.Point = new Point(x, y);
                    Conditions.Button.Location = Conditions.Point;
                }

                x = x + Script.Width + 4;
                if (Tasks != null)
                {
                    Tasks.Point = new Point(x, y);
                    Tasks.Button.Location = Tasks.Point;
                }
            }
            else
            {
                int x = Points[2] - Script.Width - 2;
                int y = Points[3] - Script.Height / 2 + Script.Offset;
                Conditions.Point = new Point(x, y);
                Conditions.Button.Location = Conditions.Point;
                x = x + Script.Width + 4;
                Tasks.Point = new Point(x, y);
                Tasks.Button.Location = Tasks.Point;
            }
        }

        private void RelocateRequested(int x, int y)
        {
            if (x == -1 || First == -1)
                return;

            Points[First] = x;
            Points[Second] = y;
        }

        private void ResetStart()
        {
            Points[0] = MainForm.FSM.StateDict[AutomataName + ":" + From].PositionX;
            Points[1] = MainForm.FSM.StateDict[AutomataName + ":" + From].PositionY;
        }

        private void ResetEnd()
        {
            Points[Points.Count - 2] = MainForm.FSM.StateDict[AutomataName + ":" + To].PositionX;
            Points[Points.Count - 1] = MainForm.FSM.StateDict[AutomataName + ":" + To].PositionY;
        }

        public Transition FindClickedTransition(int x, int y)
        {
            if (To == null || IsInit() || IsFinite())
                return null;
            for (int i = 2; i < Points.Count - 2; i += 2)
            {
                if (Math.Sqrt(Math.Pow(Points[i] - x, 2) + Math.Pow(Points[i + 1] - y, 2)) < Radius)
                {
                    First = i;
                    Second = i + 1;
                    return this;
                }
            }

            return null;
        }

        public bool DeletePoint()
        {
            Points.RemoveAt(Second);
            Points.RemoveAt(First);
            RemoveReference();
            return Points.Count <= 4 && Color == Color.Black;
        }

        public void DeleteTransition()
        {
            Disable();
        }

        public void RemoveReference()
        {
            First = -1;
            Second = -1;
        }

        public void AddPoint()
        {
            Points.Insert(Second + 1, Points[Second]);
            Points.Insert(Second + 1, Points[First]);
            First += 2;
            Second += 2;
        }

        public void Enable()
        {
            Tasks?.Enable();
            Conditions?.Enable();
        }

        public void Disable()
        {
            Tasks?.Disable();
            Conditions?.Disable();
        }
    }
}