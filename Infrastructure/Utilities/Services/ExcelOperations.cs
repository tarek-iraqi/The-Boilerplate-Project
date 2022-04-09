using Application.Contracts;
using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public class ExcelOperations : IExcelOperations
    {
        public async Task Export<T>(IEnumerable<T> data, string filePath, string sheetName)
        {
            var excel = new ExcelMapper();

            await excel.SaveAsync(filePath, data, sheetName);
        }

        public async Task<IEnumerable<T>> Read<T>(string filePath)
        {
            var excel = new ExcelMapper();

            return await excel.FetchAsync<T>(filePath);
        }
    }
}
