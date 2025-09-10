using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using powerplatform.ai.powerautomatedoc.Models;

namespace powerplatform.ai.powerautomatedoc.Services
{
    public class ADOService
    {
        private readonly HttpClient _httpClient;

        private readonly string _pat;
        private readonly string _organizationUri;
        private readonly string _projectName;
        private readonly string _repositoryName;

        public ADOService(string pat, string organizationUri, string projectName, string repositoryName)
        {
            _httpClient = new HttpClient();
            _pat = pat;
            _organizationUri = organizationUri;
            _projectName = projectName;
            _repositoryName = repositoryName;
        }

        private string ADOBaseURL(string organizationUri, string projectName) => $"{organizationUri}{projectName}/_apis";

        public async Task<ADORepos> GetRepositoryItems()
        {
            var basicPat = Convert.ToBase64String(Encoding.UTF8.GetBytes($":{_pat}"));

            var reqMsg = new HttpRequestMessage(HttpMethod.Get, $"{ADOBaseURL(_organizationUri, _projectName)}/git/repositories/{_repositoryName}/items?recursionLevel=Full&latestProcessedChange=true&api-version=7.0");
            reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicPat);

            var response = await _httpClient.SendAsync(reqMsg);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var repos = JsonSerializer.Deserialize<ADORepos>(content, option);

                return repos;
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<string> GetReposItemContent(string itemUrl)
        {
            var basicPat = Convert.ToBase64String(Encoding.UTF8.GetBytes($":{_pat}"));

            var itemReqMsg = new HttpRequestMessage(HttpMethod.Get, itemUrl);
            itemReqMsg.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicPat);

            var response = await _httpClient.SendAsync(itemReqMsg);

            if (response.IsSuccessStatusCode)
            {
                var fileContent = await response.Content.ReadAsStringAsync();

                return fileContent;
            }

            return null;
        }

        public async Task<ADOProjectWikis> GetProjectWikis()
        {
            var basicPat = Convert.ToBase64String(Encoding.UTF8.GetBytes($":{_pat}"));

            var reqMsg = new HttpRequestMessage(HttpMethod.Get, $"{ADOBaseURL(_organizationUri, _projectName)}/wiki/wikis?api-version=7.0");

            reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicPat);

            var response = await _httpClient.SendAsync(reqMsg);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var projectWikis = JsonSerializer.Deserialize<ADOProjectWikis>(content, option);

                return projectWikis;
            }

            return null;
        }

        public async Task<bool> CreateWikiPage(string projectWikiId, string pageName, string pageContent, string eTag = null)
        {
            var basicPat = Convert.ToBase64String(Encoding.UTF8.GetBytes($":{_pat}"));

            var reqMsg = new HttpRequestMessage(HttpMethod.Put, $"{ADOBaseURL(_organizationUri, _projectName)}/wiki/wikis/{projectWikiId}/pages?path={pageName}&api-version=7.");

            reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicPat);

            if (!string.IsNullOrEmpty(eTag))
            {
                reqMsg.Headers.Add("If-Match", eTag);
            }

            reqMsg.Content = new StringContent(JsonSerializer.Serialize(new { content = pageContent }), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(reqMsg);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<HttpResponseMessage> WikiPageExist(string projectWikiId, string pageName)
        {
            var basicPat = Convert.ToBase64String(Encoding.UTF8.GetBytes($":{_pat}"));

            var reqMsg = new HttpRequestMessage(HttpMethod.Get, $"{ADOBaseURL(_organizationUri, _projectName)}/wiki/wikis/{projectWikiId}/pages?path={pageName}&api-version=7.");

            reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicPat);

            var response = await _httpClient.SendAsync(reqMsg);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            return null;
        }

        public async Task<bool> DeleteWikiPage(string projectWikiId, string pageName)
        {
            var basicPat = Convert.ToBase64String(Encoding.UTF8.GetBytes($":{_pat}"));

            var reqMsg = new HttpRequestMessage(HttpMethod.Delete, $"{ADOBaseURL(_organizationUri, _projectName)}/wiki/wikis/{projectWikiId}/pages?path={pageName}&api-version=7.");

            reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicPat);

            var response = await _httpClient.SendAsync(reqMsg);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
