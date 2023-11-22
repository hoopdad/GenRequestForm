using Azure.AI.OpenAI;
using Azure;
using ChatGPTRunner.Data;
using ChatGPTRunner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.Identity.Client;
using System.Numerics;

MyContext cntxt = new MyContext();
var db = new ApplicationDbContext(cntxt);

var requests = await db.GenRequest
    .Where(b => b.Status== "Requested")
    .ToListAsync();

OpenAIClient client = new OpenAIClient(cntxt.APIKey);

foreach (GenRequest req in requests)
{
    Console.WriteLine(req.Title);
    Console.WriteLine(req.Actor);
    String Prompt = "Generate a blog post in HTML syntax about and titled " + req.Title +
        ", including sections for an overview, key uses and benefits, what to look for, what to watch out for, links with HTML link syntax to popular products, and who are customers of those products. " +
        "Each section should use an HTML H2 section title and lists of links should use HTML UL." +
        "Add keywords to help search engines find this post. " +
        "Make sure to use HTML formatting as much as possible.";

    CompletionsOptions completionsOptions = new()
    {
        DeploymentName = "text-davinci-003",
        User = req.Actor,
        MaxTokens = 3000,
        Prompts = { Prompt,"What is a good title for this content?" }
    };

    Response<Completions> completionsResponse = client.GetCompletions(completionsOptions);

    req.GeneratedContent = completionsResponse.Value.Choices[0].Text;
    req.GeneratedTitle = completionsResponse.Value.Choices[1].Text;
    req.GeneratedDate = DateTime.Today;
    db.Update(req);
    await db.SaveChangesAsync();

    //    foreach (Choice choice in completionsResponse.Value.Choices)
    //    {
    //Console.WriteLine($"Response for prompt {choice.Index}: {choice.Text}");
    //}
}

