using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatChit.Models.RequestModel
{
    public class PostRequestModel
    {
        public string? content { get; set; }
        public string image { get; set; }
    }
}
