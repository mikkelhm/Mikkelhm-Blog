using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http;
using System.Xml;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Mikkelhm.Web.Controllers
{
    public class AdminController : UmbracoApiController
    {
        [HttpGet]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [HttpGet]
        public string ImportData()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(HttpContext.Current.Server.MapPath("~/") + @"App_Data\\blogml.xml");
            var categories = xmlDocument.SelectNodes("/blog/categories/category");
            var categoryList = new Dictionary<string, string>();
            foreach (XmlNode category in categories)
            {
                categoryList.Add(category.Attributes["id"].Value, category.FirstChild.InnerText);
            }

            var posts = xmlDocument.SelectNodes("/blog/posts/post");
            var postList = new List<dynamic>();
            var home = Services.ContentService.GetByLevel(1).FirstOrDefault(x => x.ContentType.Alias == "Home");
            var blogPostRepository = home.Children().FirstOrDefault(x => x.ContentType.Alias == "BlogPostRepository");

            foreach (XmlNode post in posts)
            {
                var postUrl = post.Attributes["post-url"].Value;
                var dateCreated = post.Attributes["date-created"].Value;
                var postTitle = post.SelectSingleNode("title").InnerText;
                var postContent = post.SelectSingleNode("content").InnerText;
                var postCategoryList = new List<string>();
                var postCategories = post.SelectNodes("categories/category");
                if (postCategories.Count > 0)
                {
                    foreach (XmlNode postCategory in postCategories)
                    {
                        postCategoryList.Add(categoryList[postCategory.Attributes["ref"].Value]);
                    }
                }
                var tags = post.SelectNodes("tags/tag");
                var tagList = new List<string>();
                if (tags.Count > 0)
                {
                    foreach (XmlNode tag in tags)
                    {
                        tagList.Add(tag.Attributes["ref"].Value);
                    }
                }

                var existingPost = Services.ContentService.GetChildrenByName(blogPostRepository.Id, postTitle);
                if (!existingPost.Any())
                {
                    var blogpost = Services.ContentService.CreateContent(postTitle, blogPostRepository.Id, "legacyBlogPost");
                    blogpost.SetValue("legacyPostUrl", postUrl);
                    blogpost.SetValue("blogPostDate", dateCreated);
                    blogpost.SetValue("content", postContent);
                    blogpost.SetTags("looslyDefinedCategories", tagList, true, "Loosly");
                    blogpost.SetTags("generalCategories", postCategoryList, true, "General");
                    Services.ContentService.SaveAndPublishWithStatus(blogpost);
                }
                else
                {
                    //existingPost.First().SetTags();
                    existingPost.First().SetTags("looslyDefinedCategories", tagList, true, "Loosly");
                    existingPost.First().SetTags("generalCategories", postCategoryList, true, "General");
                    Services.ContentService.SaveAndPublishWithStatus(existingPost.First());
                }
            }
            return categoryList.Count.ToString();
        }

        [HttpGet]
        public string SaveAndPublishAllBlogPosts()
        {
            var home = Services.ContentService.GetByLevel(1).FirstOrDefault(x => x.ContentType.Alias == "Home");
            var count = 0;

            if (home != null)
            {
                var blogPostRepository = home.Children().FirstOrDefault(x => x.ContentType.Alias == "BlogPostRepository");
                foreach (var child in blogPostRepository.Children())
                {
                    //child.SetTags("looslyDefinedCategories", Umbraco.TagQuery.GetTagsForEntity(child.Id, "Loosly").Select(x => x.Text), true, "Loosly");
                    //child.SetTags("generalCategories", Umbraco.TagQuery.GetTagsForEntity(child.Id, "General").Select(x => x.Text), true, "General");
                    Services.ContentService.SaveAndPublishWithStatus(child);
                    count++;
                }
            }
            return count.ToString();
        }
    }
}