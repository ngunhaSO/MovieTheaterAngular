using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;

namespace Core.Common.Extensions
{
    //===============================================================================//
    //     Author: JN                                                                //
    //     Desc: Extension method for IQueryable                                     //                                      //
    //     usage: List<Employee> lst = ds.Tables[0].ToList<Employee>();              //
    //===============================================================================//
    public static class IQueryableExtension
    {
        public static IQueryable<T> Include<T> (this IQueryable<T> query, string property) where T:new()
        {
            var objectQuery = query as ObjectQuery<T>;
            if(objectQuery == null)
            {
                throw new NotSupportedException("Include can only be called on ObjectQuery");
            }
            return objectQuery.Include(property);
        }
    }
}
