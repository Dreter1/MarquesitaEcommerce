﻿using Marquesita.Models.Identity;
using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Marquesita.Infrastructure.EmailConfigurations.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public User User { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string OptionalURL { get; set; }
        public byte[] Attachments { get; set; }
        public Message(IEnumerable<string> to, string subject, User user, string content, string optionalURL, byte[] attachments)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            User = user;
            Content = content;
            OptionalURL = optionalURL;
            Attachments = attachments;
        }
    }
}
