---
title: "Bountease: a BugBounty recon script"
subtitle: ""
draft: false
author: "Hato0"
description: "Doing passive and active reconnaissance for given domain or IP. This script is fully automated and aim to facilitate the recon for bug hunters."
Last Update: <time datetime="{{ .Page.Lastmod.Format "Mon Jan 10 17:13:38 2020 -0700" }}" class="text-muted">  {{ $.Page.Lastmod.Format "January 02, 2006" }} </time>
Date: 2022-08-18

tags: ["recon", "redteam", "automation", "development"]
categories: ["RedTeam - Tools"]

hiddenFromHomePage: false
hiddenFromSearch: false
twemoji: false
lightgallery: false
ruby: false
fraction: true
fontawesome: true
linkToMarkdown: true
rssFullText: false
toc:
  enable: true
  auto: true
code:
  copy: true
  maxShownLines: 50
share:
  enable: true
comment:
  enable: false
---
## Summary

As I love both side of cybersecurity and as my job is blue team oriented, I'm doing the red-teaming side on my personal life.  
In that context, you may understand that I don't have a lot of time to perform the enumeration and reconnaissance of the target.  

Fortunately, this phase is highly automatable, and that's what this post is about.
I've developed a recon script for Bug Bounty hunters that fit my needs (actually, I'll be the principal user) named Bountease.

## Language used

To be honest, I did what was the fastest but still readable by everyone. 
This project is 100% coded in python and the report part is generated in Markdown.

Why Markdown ? 
I use Markdown for all my reports (personal and professional) so it's basically simpler for me and I find it very easy to modify / custom for other users.

## Functionalities

Two scan have been developed (they can be use in standalone or combined).  

First scan part is a fully passive recon, which include the following functionalities:
* Subdomain enum using :
    * CRT
    * Wayback machine
    * Google dorks
    * Dnsdumpster
* S3 enumeration
* Whois information 
* Social information :
    * Mail format 
    * Linkedin employee data  
   
Second scan part is an active recon, which include the following functionalities:
* Port scanning
* Service reconnaissance
* Subdomain enumeration 
* API Discovery
* Security Header check
* WAF enumeration
* Directory Listing check

On top of these functionalities, as it's developed for Bug Bounty, you are able to specify a specific User-Agent and a request rate limit.

{{< admonition note "Note" >}} {{< version 0.2.10 >}}
You can choose to execute each module individually by modifying the configuration file.
{{< /admonition >}}

## Usage

Every information you will need to use this tool can be found in the GitHub [repo](https://github.com/Hato0/Bountease/).

If you have any trouble or find possible improvement (there are a lot that can be done), feel free to open an issue.
In case of trouble using it, you can reach me.

## Conclusion

Pretty happy to have developed this tool for my and others usage. Reconnaissance take use massive time during Bug Bounties and reducing it is highly valuable. 

Hope that some people will find this script useful and use it wisely. 