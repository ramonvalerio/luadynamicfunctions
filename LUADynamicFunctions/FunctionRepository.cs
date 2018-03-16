namespace LUADynamicFunctions
{
    public class FunctionRepository
    {
        public Functor getFormulaByFunctionName(string functionName)
        {
            switch (functionName)
            {
                case "A":
                    {
                        var functor = new Functor();
                        functor.Name = functionName;
                        functor.Expression = "x * 2";
                        return functor;
                    }
                    
                case "B":
                    {
                        var functor = new Functor();
                        functor.Name = functionName;
                        functor.Expression = "x + 2";
                        return functor;
                    }
                    
                case "C":
                    {
                        var functor = new Functor();
                        functor.Name = functionName;
                        functor.Expression = "x + 3";
                        return functor;
                    }

                default:
                    throw new System.Exception("Fóruma com função inexistente.");
            }
        }
    }
}