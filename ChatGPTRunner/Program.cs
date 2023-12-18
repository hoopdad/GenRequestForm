using GenReq.Models;
using ChatGPTRunner.Data;
using Microsoft.EntityFrameworkCore;
using ChatGPTRunner.Services;
using ChatGPTRunner.Models;

MyContext cntxt = new MyContext();
var db = new ApplicationDbContext(cntxt);

List<GenReq.Models.GenRequest> requests = null;
    requests = await db.GenRequest
//        .Where(b => b.Status == "Requested")
        .Where(b => b.Id==42)
        .ToListAsync();

ChatGPT gpt = new ChatGPT(cntxt);

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

        CompletionRequest compreq = new CompletionRequest(new List<Message>()
        {
            new Message() {role="user", content=Prompt},
            new Message() {role="user", content="Suggest a title for the blog created in the prior message"}
        });
        List<Choice> completionsResponse = await gpt.GetContentAsync(compreq);
        
        if (!cntxt.DEBUGGING)
        {
            req.Status = "Generated";
        }
        if (completionsResponse != null && completionsResponse.Count > 0 && completionsResponse[0].message != null)
        {
            req.GeneratedContent = completionsResponse[0].message.content;
            if (cntxt.DEBUGGING)
            {
                Console.WriteLine(req.GeneratedContent);
            }
            if (req.GeneratedContent.Trim().StartsWith("\""))
            {
                req.GeneratedContent = req.GeneratedContent.Trim().Substring(1);
            }
        }

        if (completionsResponse != null && completionsResponse.Count > 1 && completionsResponse[1].message != null)
        {
            req.GeneratedTitle = completionsResponse[1].message.content;
            if (cntxt.DEBUGGING)
            {
                Console.WriteLine(req.GeneratedTitle);
            }

            if (req.GeneratedTitle.Trim().StartsWith("\""))
            {
                req.GeneratedTitle = req.GeneratedTitle.Trim().Substring(1);
            }
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

