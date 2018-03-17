﻿using NLua;

namespace DynaFunction.Domain.Model
{
    public class Constant
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public void CreateGlobalConstantValue(Lua state)
        {
            state[this.Name] = this.Value;
        }
    }
}