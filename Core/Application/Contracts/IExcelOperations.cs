using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IExcelOperations
{
    Task Export<T>(IEnumerable<T> data, string filePath, string sheetName);
    Task<IEnumerable<T>> Read<T>(string filePath);
}