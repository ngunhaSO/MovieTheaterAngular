using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Extensions
{
    //===============================================================================//
    //     Author: JN                                                                //
    //     Desc: Select values of a column in datatable                              //                                      //
    //     usage:                                                                    //
    //      var data = dataset.Tables["Tables"].AsEnumerable().                      //
    //                            Select(r => new CustomObject                       //
    //                           {                                                   //
    //                              Name = r.Field<string>("name")                   //
    //                           });                                                 //
    //===============================================================================//
    public static class IDataReaderExtension
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }
}
