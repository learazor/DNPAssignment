using System.Text.Json;
using BlazorApp.Service;
using DTOs;

namespace Services
{
    public class HttpCommentService : ICommentService
    {
        private readonly HttpClient client;

        public HttpCommentService(HttpClient client)
        {
            this.client = client;
        }

        // Add a new comment
        public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
        {
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
            string response = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(response);
            }
            return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        // Update an existing comment
        public async Task UpdateCommentAsync(int commentId, UpdateCommentDto request)
        {
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"comments/{commentId}", request);
            if (!httpResponse.IsSuccessStatusCode)
            {
                string response = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }

        // Remove a comment
        public async Task RemoveCommentAsync(int commentId)
        {
            HttpResponseMessage httpResponse = await client.DeleteAsync($"comments/{commentId}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                string response = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }

        // Get a comment by ID
        public async Task<CommentDto> GetCommentByIdAsync(int commentId)
        {
            HttpResponseMessage httpResponse = await client.GetAsync($"comments/{commentId}");
            string response = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(response);
            }
            return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        // Get all comments for a specific post
        public async Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId)
        {
            HttpResponseMessage httpResponse = await client.GetAsync($"posts/{postId}/comments");
            string response = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(response);
            }
            return JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
