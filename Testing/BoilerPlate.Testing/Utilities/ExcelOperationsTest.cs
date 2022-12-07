using Application.Contracts;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Services;
using Xunit;

namespace BoilerPlate.Testing.Utilities;

public class ExcelOperationsTest
{
    private readonly string _fileDirectory;

    public ExcelOperationsTest()
    {
        _fileDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/_shared/files";
    }

    [Fact]
    public async Task ExportUsersDataToExcel()
    {
        var users = new List<User>
        {
            new User{Id = 1, Name = "Tarek", Email = "tarek.iraqi@gmail.com"},
            new User{Id = 2, Name = "Ahmed", Email = "ahmed.iraqi@gmail.com"},
            new User{Id = 3, Name = "Mamdouh", Email = "mamdouh.iraqi@gmail.com"}
        };

        IExcelOperations excelOperations = new ExcelOperations();

        await excelOperations.Export(users, Path.Combine(_fileDirectory, "users.xlsx"), "users");

        var result = File.Exists(Path.Combine(_fileDirectory, "users.xlsx"));

        result.ShouldBe(true);
    }

    [Fact]
    public async Task ReadExcelFile()
    {
        IExcelOperations excelOperations = new ExcelOperations();

        var users = await excelOperations.Read<User>(Path.Combine(_fileDirectory, "users.xlsx"));

        users.Count().ShouldBe(3);
        users.First().Id.ShouldBe(1);
        users.First().Name.ShouldBe("Tarek");
        users.First().Email.ShouldBe("tarek.iraqi@gmail.com");
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}