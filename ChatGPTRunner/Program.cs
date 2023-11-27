using GenReq.Models;
using Azure.AI.OpenAI;
using Azure;
using ChatGPTRunner.Data;
using Microsoft.EntityFrameworkCore;


String ChatGPTEngine = "";
ChatGPTEngine = "gpt-4";
ChatGPTEngine = "gpt-3.5-turbo";
ChatGPTEngine = "gpt-3.5";
ChatGPTEngine = "text-davinci-003";

MyContext cntxt = new MyContext();
var db = new ApplicationDbContext(cntxt);

GenRequest fred = db.GenRequest.First(x => x.Id == 3);
fred.ContentTemplate = ContentTemplates.Freeform;
fred.ContentTemplate = ContentTemplates.TechnicalOverview;
fred.ContentTemplate = ContentTemplates.Howto;
fred.ContentTemplate = ContentTemplates.LearnNewSkill;


List<GenReq.Models.GenRequest> requests = null;
if (cntxt.DEBUGGING)
{
    requests = await db.GenRequest
        .Where(x => x.Id==3)
        .ToListAsync();
} 
else
{
    requests = await db.GenRequest
        .Where(b => b.Status == "Requested")
        .ToListAsync();
}


OpenAIClient client = new OpenAIClient(cntxt.APIKey);

foreach (GenRequest req in requests)
{
    try
    {
        Console.WriteLine("Running for " + req.OwningUserId);
        String strArticleStructure = "";
        switch(req.ContentTemplate)
        {
            case ContentTemplates.TechnicalOverview:
                strArticleStructure =
                    "Generate a technical overview blog post in HTML syntax about and titled " + req.Title
                    + ", including sections for an overview, key uses and benefits, what to look for, what to watch out for, links with HTML link syntax to popular products, and who are customers of those products. "
                    + "Each section should use an HTML H2 section title and lists of links should use HTML UL.";
                break;
            case ContentTemplates.Howto:
                strArticleStructure =
                    "Generate a How-to blog post in HTML syntax about and titled " + req.Title
                    + ", including sections for an overview, how to get ingredients or other pre-requisites, steps in the process, and what the final outcome should resemble. "
                    + "Each section should use an HTML H2 section title and lists of links should use HTML UL.";
                break;
            case ContentTemplates.LearnNewSkill:
                strArticleStructure =
                    "Generate a blog post in HTML syntax about and titled " + req.Title
                    + " for someone learning a new skill, including sections for an an overview, key uses and benefits, a path to expertise, links with HTML link syntax to popular sites with training materials, and links with HTML link syntax to sites with relevant certifications or products. "
                    + "Each section should use an HTML H2 section title and lists of links should use HTML UL.";
                break;
            case ContentTemplates.Freeform:
            default:
                strArticleStructure =
                    "Generate a blog post in HTML syntax about and titled " + req.Title +
                    " using your knowledge of that topic and applicable related topics. ";
                break;
        }
        String Prompt = 
            strArticleStructure +
            "Add keywords to help search engines find this post. " +
            "Make sure to use HTML formatting as much as possible.";

        CompletionsOptions completionsOptions = new()
        {
            DeploymentName = ChatGPTEngine,
            User = req.Actor,
            MaxTokens = 3000,
            Prompts = { Prompt, "What is a good title for this content?" }
        };

        Response<Completions> completionsResponse = client.GetCompletions(completionsOptions);

        if (!cntxt.DEBUGGING)
        {
            req.Status = "Generated";
        }
        req.GeneratedContent = completionsResponse.Value.Choices[0].Text;
        if (cntxt.DEBUGGING)
        {
            Console.WriteLine(req.GeneratedContent);
        }
        req.GeneratedTitle = completionsResponse.Value.Choices[1].Text;
        if (cntxt.DEBUGGING)
        {
            Console.WriteLine(req.GeneratedTitle);
        }

        if (req.GeneratedTitle.Trim().StartsWith("\""))
        {
            req.GeneratedTitle = req.GeneratedTitle.Trim().Substring(1);
        }
        if (req.GeneratedContent.Trim().StartsWith("\""))
        {
            req.GeneratedContent = req.GeneratedContent.Trim().Substring(1);
        }

        req.GeneratedDate = DateTime.Now;
        db.Update(req);
        await db.SaveChangesAsync();
        Console.WriteLine("Run complete");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
        req.Status = "Error: "+e.ToString();
        db.Update(req);
        await db.SaveChangesAsync();
    }
}


