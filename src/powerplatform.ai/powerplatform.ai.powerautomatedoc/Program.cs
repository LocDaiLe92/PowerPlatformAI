using powerplatform.ai.powerautomatedoc.Services;
using System.Text;

namespace powerplatform.ai.powerautomatedoc
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            if (args.Length == 0)
            {
                throw new Exception("No args found...");
            }

            var pat = args[0]; 
            var organizationUri = args[1]; 
            var projectName = args[2];
            var repository = args[3]; 

            var validFileExtensions = new List<string>()
            {
                "json"
            };

            var adoService = new ADOService(pat, organizationUri, projectName, repository);
            var openAIService = new OpenAIService();

            var adoReposItems = await adoService.GetRepositoryItems();

            if (adoReposItems != null)
            {

                var projectWikis = await adoService.GetProjectWikis();

                if (projectWikis != null)
                {
                    var projectWiki = projectWikis.Value.FirstOrDefault(x => x.Type == "projectWiki");

                    if (projectWiki != null)
                    {
                        var pageName = $"Power Automate Documentation";

                        var deleteWikiPage = await adoService.DeleteWikiPage(projectWiki.Id, pageName);

                        var wikiPageCreated = await adoService.CreateWikiPage(projectWiki.Id, pageName, "Root");

                        var items = adoReposItems.Value.Where(x => !x.IsFolder && x.Path.StartsWith("/PowerAutomateForDocumentation")).ToList();

                        foreach (var item in items)
                        {
                            if (item.GitObjectType == "blob" && validFileExtensions.Any(x => item.Path.EndsWith(x)) && !item.Path.StartsWith("/."))
                            {
                                var fileContent = await adoService.GetReposItemContent(item.Url);

                                if (fileContent != null)
                                {
                                    var completionResponse = await openAIService.RequestChatGptResponse(fileContent);

                                    if (completionResponse != null)
                                    {
                                        //var path = item.Path.Substring(1).Replace("/", "_");

                                        var fileName = item.Path.Split('/').Last();

                                        StringBuilder sb = new StringBuilder();

                                        var itemGitPath = $"https://dev.azure.com/twodaygroup/Dynamics 365/_git/Power Automate Documentation?path={item.Path}";

                                        sb.Append($"[{item.Path}]({itemGitPath.Replace(" ", "%20")})");
                                        sb.AppendLine("\n");
                                        sb.AppendLine(completionResponse.Content.FirstOrDefault()!.Text);

                                        var wikiCodeExplainPageExistResponse = await adoService.WikiPageExist(projectWiki.Id, $"{pageName}/{fileName}");

                                        var eTag_CodeExplain = "";

                                        if (wikiCodeExplainPageExistResponse != null)
                                        {
                                            eTag_CodeExplain = wikiCodeExplainPageExistResponse.Headers.ETag.ToString();
                                        }

                                        var wikiCodeExplainPageCreated = await adoService.CreateWikiPage(projectWiki.Id, $"{pageName}/{fileName}", sb.ToString(), eTag_CodeExplain);


                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
