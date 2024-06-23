using System;
using System.Linq;
using Newtonsoft.Json;

namespace State_Automata_IDLE.Memory
{
    public class Variables
    {
        public string VariableType { get; set; }
        public string InitValue { get; set; }
        [field: NonSerialized] private readonly Data _emptyData;

        [JsonConstructor]
        public Variables(string type, string initValue, int i = -1)
        {
            VariableType = type;
            InitValue = initValue;
            _emptyData = new Data();
        }

        public Variables(string type, string initValue)
        {
            VariableType = type;
            _emptyData = new Data();
            SetNewValue(initValue);
        }

        public string GetVariableType()
        {
            return VariableType;
        }

        public string GetInitValue()
        {
            return InitValue;
        }

        public void SetNewValue(string newValue)
        {
            if (VariableType == "bool" || _emptyData.GetPortModules().Contains(VariableType))
                InitValue = newValue.ToUpper();
            else
                InitValue = newValue;
        }

        public string GetExportType()
        {
            // <variable-type> ::= <var-type-string> | <var-type-number> | <var-type-boolean> | <var-type-port>
            switch (VariableType)
            {
                // <var-type-string> ::= "6"
                case "string":
                    return "6";
                // <var-type-number> ::= "7"
                case "int":
                    return "7";
                // <var-type-boolean> ::= "8"
                case "bool":
                    return "8";
                // <var-type-port> ::= <var-type-single-port> | <var-type-double-port>
                default:
                    return ExportPort();
            }
        }

        private string ExportPort()
        {
            switch (VariableType)
            {
                /*
                <var-type-single-port> ::= <device-type> <port-number-plus-2>
                <device-type> ::= "0" | "1" | "2" | "3" | "4" | "5"          ; 0 - force sensor, 1 - color sensor, 2 - distance US sensor, 4 - medium motor, 5 - large motor
                <port-number-plus-2> ::= "0" | "1" | "2" | "3" | "4" | "5"   ; for port A, specify 0, for port E, specify 4, etc.
                <var-type-double-port> ::= <device-type-drive-base> <port-number-plus-2> <port-number-plus-2>
                <device-type-drive-base> ::= "1"   ; denotes drive base with two medium motors attached to the two ports specified above
                ; example port types: "12" - color sensor on port 3,  "145" - motor base on ports E (left), F (right)
             */
                case "ForceSensor":
                    return "0" + (InitValue[0] - 'A');
                case "ColorSensor":
                    return "1" + (InitValue[0] - 'A');
                case "DistanceSensor":
                    return "2" + (InitValue[0] - 'A');
                case "MediumMotor":
                    return "4" + (InitValue[0] - 'A');
                case "LargeMotor":
                    return "5" + (InitValue[0] - 'A');
                case "DriveBase":
                    return "1" + (InitValue[0] - 'A') + (InitValue[2] - 'A');
                default:
                    throw new Exception("Invalid variable type: " + VariableType);
            }
        }

        public string GetExportValue()
        {
            // <variable-init-value> ::= <constant-string-value> | <constant-numeric-value> | <constant-boolean-value>
            // ; the type of init value should match the specified variable type; port-type variable initial values are ignored (can be empty string, or 0)
            if (VariableType == "bool" || VariableType == "int" || VariableType == "string")
                return InitValue;
            return "";
        }
    }
}