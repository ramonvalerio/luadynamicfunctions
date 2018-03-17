using DynaFunction.Domain.Model;

namespace DynaFunction.Repository
{
    public class FunctionRepository
    {
        public Functor getFunctorByName(string name)
        {
            switch (name)
            {
                case "A":
                    {
                        var functor = new Functor();
                        functor.Name = name;
                        functor.Expression = "Multiple10(x * 2)";
                        return functor;
                    }
                    
                case "B":
                    {
                        var functor = new Functor();
                        functor.Name = name;
                        functor.Expression = "Multiple10(x + 2)";
                        return functor;
                    }
                    
                case "C":
                    {
                        var functor = new Functor();
                        functor.Name = name;
                        functor.Expression = "Multiple10(x + 3)";
                        return functor;
                    }
                case "Multiple10":
                    {
                        var functor = new Functor();
                        functor.Name = name;
                        functor.Expression = "(x * 10)";
                        return functor;
                    }
                case "Multiple50":
                    {
                        var functor = new Functor();
                        functor.Name = name;
                        functor.Expression = "(x * 50)";
                        return functor;
                    }
                case "Multiple100":
                    {
                        var functor = new Functor();
                        functor.Name = name;
                        functor.Expression = "(x * 100)";
                        return functor;
                    }

                default:
                    throw new System.Exception("Fóruma com função inexistente.");
            }
        }
    }
}