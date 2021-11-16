# WebCrawler_Csharp
A simple web crawler application developed in c# console.

# Goal
The goal of this project is to create an application that crawls a particular domain to retrive a sitemap of that particular domain. 
The sitemap contains the top-level pages and all the accessible links from each of them which I have called "Childlinks". The crawler will crawl all the child links to 
retrieve the static assests attached to it.

# Class
I have designed the classes that will be used for this crawler;
- TopLevelLink:
    public class TopLink
        {
            public string Name { get; set; }
            public string Link { get; set; }
            public List<ChildLink> ChildLinks { get; set; }
        }
- ChildLevelLink:
     public class ChildLink
        {
            public string Name { get; set; }
            public string Link { get; set; }
            public List<string> StaticAssests { get; set; }
        }
        
# Result
The result is streamed to a file called "sitemap.json" in the project directory
