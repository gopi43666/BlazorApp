using BlazorApp.Models;

using Newtonsoft.Json;

using StudentHttpHandler;

namespace BlazorApp.Data
{
    public class StudentRepository
    {
        private readonly string? studentUrl;
        public readonly IHttpHandler _httpHandler;

        public StudentRepository(IHttpHandler httpHandler, IConfiguration configuration)
        {
            studentUrl = configuration.GetValue<string>("studentUri");
            _httpHandler = httpHandler;
        }

        public async Task<List<StudentModel>> GetAllStudents()
        {
            return await _httpHandler.GetAsync<List<StudentModel>>(studentUrl);
        }

        public async Task<StudentModel> GetByIdAsync(int id)
        {
            try
            {
                var url = $"{studentUrl}/{id}";
                var user = await _httpHandler.GetAsync<StudentModel>(url);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<StudentModel> PostAsync(StudentModel model)
        {
            StudentModel student = new StudentModel();
            try
            {
                var response = await _httpHandler.PostAsJsonAsync<StudentModel>(studentUrl!, model);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<StudentModel>(responseContent)!;
                }
                return student;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> PutAsync(int id, StudentModel model)
        {
            bool updated = false;
            try
            {
                var url = $"{studentUrl}/{id}";
                var response = await _httpHandler.PutAsJsonAsync<StudentModel>(url, model);
                if (response.IsSuccessStatusCode)
                    updated = true;
            }
            catch (Exception ex)
            {
                updated = false;
            }
            return updated;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool deleted = false;
            try
            {
                var url = $"{studentUrl}/{id}";
                var response = await _httpHandler.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                    deleted = true;
            }
            catch (Exception ex)
            {
                deleted = false;
            }
            return deleted;
        }
    }
}
