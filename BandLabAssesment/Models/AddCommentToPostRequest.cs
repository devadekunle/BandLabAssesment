﻿namespace BandLabAssesment.Models;

public class AddCommentToPostRequest
{
    public string Content { get; set; }
    public string CreatorId { get; set; }
    public string Creator { get; set; }
}