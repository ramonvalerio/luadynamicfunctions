using DynaFunction.Domain.Model;
using System;

namespace DynaFunction.Repository
{
    internal static class ConstantRepository
    {
        static Constant getConstantByName(string name)
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