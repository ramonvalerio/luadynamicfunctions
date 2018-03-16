using System.Collections.Generic;

namespace LUADynamicFunctions
{
    public class Functor
    {
        public string Name { get; set; }
        public string Expression { get; set; }
        public List<double?> Data { get; set; }

        public Functor()
        {
            Data = new List<double?>();
            Data.Add(null);
            Data.Add(null);

            for (int i = 0; i < 50; i++)
                Data.Add(i);
        }

        public string GetScriptFunction()
        {
            var result = $@"function {this.Name}(x)
                                if x == nil then x = 0 end
                            return {this.Expression}
                        end";

            return result;
        }
    }
}
