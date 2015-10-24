using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Extensions
{
    public static class EnumExtension
    {
        //==================================================================//
        //     Author: JN                                                   //
        //     Desc: Parse an enum                                          //
        //     usage: .ParseEnum<EnumName>(string_that_match_enum_item);    //
        //==================================================================//
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
