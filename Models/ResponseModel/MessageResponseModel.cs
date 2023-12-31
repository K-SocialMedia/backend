﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models.ResponseModel
{
    public class MessageResponseModel
    {
        public int id { get; set; }
        public Guid senderId { get; set; }
        public string senderName { get; set; }
        public Guid receiverId { get; set; }
        public string receiverName { get; set; }
        public string content { get; set; }
        public string image {  get; set; }
        public DateTime createAt { get; set; }
        public bool isRead { get; set; }
    }
}
