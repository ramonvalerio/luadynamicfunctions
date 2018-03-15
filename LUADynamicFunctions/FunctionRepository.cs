namespace LUADynamicFunctions
{
    public class FunctionRepository
    {
        public static string getFormulaByFunctionName(string functionName)
        {
            switch (functionName)
            {
                case "A":
                    return "x * 2";

                case "B":
                    return "x + 2";

                case "C":
                    return "x + 3";

                default:
                    return string.Empty;
            }
        }
    }
}
