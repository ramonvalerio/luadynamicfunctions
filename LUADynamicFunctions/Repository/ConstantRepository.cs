using DynaFunction.Domain.Model;
using System;

namespace DynaFunction.Repository
{
    public class ConstantRepository
    {
        public Constant getConstantByName(string name)
        {
            switch (name)
            {
                case "PI":
                    {
                        var constant = new Constant();
                        constant.Name = name;
                        constant.Value = Math.PI.ToString();
                        return constant;
                    }

                default:
                    throw new Exception("Fóruma com função inexistente.");
            }
        }
    }
}