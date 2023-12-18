using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Azure.AI.OpenAI;

namespace ChatGPTRunner.Models
{
    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class CompletionRequest
    {
        [Required]
        public string model { get; set; }

        public List<Message> messages { get; set; }
        public double? temperature { get; set; }

        public CompletionRequest(List<Message> msgs)
        {
            this.model = "";
            this.model = "gpt-3.5";
            this.model = "text-davinci-003";
            this.model = "gpt-3.5-turbo";
            this.model = "gpt-4";

            //            this.model = model;
            //            this.temperature = temp;
            this.temperature = 0.7;
            this.messages = msgs;
        }
        public CompletionRequest(string role, string content)
            : this(new List<Message>() {new Message() { role = role, content = content } })
        {
        }
    }


    public class Usage
    {
        public long? prompt_tokens { get; set; }//        "prompt_tokens": 13,
        public long? completion_tokens { get; set; } //        "completion_tokens": 7,
        public long? total_tokens { get; set; } //        "total_tokens": 20
    }

    public class Choice
    {
        public Message? message { get; set; }
        public string? logprobs { get; set;  } //            "logprobs": null,
        public string? finish_reason { get; set;  }//      "finish_reason": "stop",
        public long? index; //            "index": 0
    }

    public class CompletionResponse
    {
        public string? id { get; set; }//    "id": "chatcmpl-abc123",
        //public string? Object { get; set; } //         // "object": "chat.completion",
        public long? created { get; set; }//    "created": 1677858242,
        public string? model { get; set; } //    "model": "gpt-3.5-turbo-1106",
        public Usage usage { get; set; } //    "usage": {
        public List<Choice> choices {get; set; }

    }
}
