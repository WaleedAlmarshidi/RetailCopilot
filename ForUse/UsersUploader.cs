// using System.Net.Http;
// using System.Net.Http.Json;
// using System.Threading.Tasks;
// using ClosedXML.Excel;


// public class RegisterUserInputModel
// {
//     public string Email { get; set; }
//     public string Password { get; set; }
//     public string ExternalId { get; set; }
// }
// public class ExcelToApi
// {
//     private readonly HttpClient _httpClient;

//     public ExcelToApi(HttpClient httpClient)
//     {
//         _httpClient = httpClient;
//     }

//     public async Task ProcessExcelFileAsync(string filePath)
//     {
//         using var workbook = new XLWorkbook(filePath);
//         var worksheet = workbook.Worksheet(1); // Assuming data is in the first worksheet

//         foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
//         {
//             var registerUserInput = new RegisterUserInputModel
//             {
//                 Email = row.Cell(4).GetValue<string>(),
//                 Password = "DefaultPassword123!", // Use a default password or generate one
//                 ExternalId = row.Cell(1).GetValue<string>()
//             };

//             await RegisterUserAsync(registerUserInput);
//         }
//     }

//     private async Task RegisterUserAsync(RegisterUserInputModel input)
//     {
//         var response = await _httpClient.PostAsJsonAsync("api/Users/Register", input);

//         if (response.IsSuccessStatusCode)
//         {
//             Console.WriteLine($"User {input.Email} registered successfully.");
//         }
//         else
//         {
//             Console.WriteLine($"Failed to register user {input.Email}. Status Code: {response.StatusCode}");
//         }
//     }
// }

// // public class Program
// // {
// //     public static async Task Main(string[] args)
// //     {
// //     }
// // }
// var httpClient = new HttpClient
// {
//     BaseAddress = new Uri("http://localhost:5000/") // Replace with your actual base address
// };

// var excelToApi = new ExcelToApi(httpClient);
// await excelToApi.ProcessExcelFileAsync("path/to/your/excel/file.xlsx");

// Console.WriteLine("Processing completed.");