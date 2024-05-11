---
title: "OwlGuard: A multi-instance SIEM rule manager."
subtitle: ""
draft: false
author: "Hato0"
description: "OwlGuard aims to simplify the management of multiple SIEM instances and to manage rules more efficiently."
Last Update: <time datetime="{{ .Page.Lastmod.Format "Mon Jan 10 17:13:38 2020 -0700" }}" class="text-muted">  {{ $.Page.Lastmod.Format "January 02, 2006" }} </time>
Date: 2024-05-11

tags: ["web", "blueteam", "development", "Open-Source"]
categories: ["BlueTeam - Tools"]

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

OwlGuard is a platform designed to provide security teams a better way to manage SIEM rules. This tool aims to onboard clients efficiently just by using already developed rules and a simple connection to their SIEM. For now, only SPLUNK is supported, if the project get interest, other SIEM will be included in the support list.

## Language used 

When I'm doing a project, I'm not only doing it to develop something that will be helpful for me or other but also to learn new things. In that case, I've chosen to develop the application using Django. 
Django is a web python framework used by many companies to create dynamic applications. I nevered used Django (used Flask previously for web app in Python) so it was a good way to try it out. This framework fits pretty well with the needs of this application (background task, multiple threads, Python, ...) so why not giving it a shot? 
Looking back, that choice was on point with the requirements and I'm happy to have learned a new framework.

## Functionalities 

In term of functionalities, this application covers pretty much all your needs for rule management (except very specific ones). Let's break them down one by one.

### Rule management

The main purpose of this tool was to centralize the management of rules for big companies with multiple SIEM/Client to manage at once. This can also help companies that are regularly changing SIEM to avoid the constant switch and redevelopment of rules. 

The rule management system is built on two main components: 
* SIGMA base interactions.
* Direct connection to the SIEM using their API.

To base myself on SIGMA was obvious as this is the only generic and widely used format. The application allow you to import yaml file in a SIGMA format to create new rules. Any modification to the rule in the application will be automatically replicated to the SIGMA file stored on the server. 

![Upload pannel](https://www.hato0.xyz/images/tools/OwlGuard/upload_view.png)

The application also gives the possibility to any user to view the rule and the versioning *(for all features inside OwlGuard a history is kept for all modifications)*. A filtering on tags (mitre, custom, etc) is also possible to quickly understand the coverage of a particular technic or actor.

![Overview rule dashboard](https://www.hato0.xyz/images/tools/OwlGuard/overviewRule.png)

The rule details and the history:

![Rule history](https://www.hato0.xyz/images/tools/OwlGuard/history.png)

In the screen above, you can also see a column 'Status by Connector'. This column indicates the status of each rules for each connectors. In this example, only one has been created (can only access one demo instance in my home lab), but in case of multiple instances you will be able to quickly identify status anomalies. The platform provides you with an easy way to associate rules to SIEM through their connector as you can see below:

![Associate connector/rule](https://www.hato0.xyz/images/tools/OwlGuard/associate.png)

And to adjust their status also (only if associated with the connector): 

![Rule status by connector](https://www.hato0.xyz/images/tools/OwlGuard/status.png)

As you probably saw on the screen above, a Splunk logo is to be seen in the detection section. This is the case for rules that have been onboarded into a Splunk instance through OwlGuard. You will be able to view the SPL query and to modify it if needed just by clicking on the Splunk logo. In the default configuration, OwlGuard will update the SIEM with modifications every 15 mins in a bidirectional way (SIEM modifications will be included to OwlGuard and vice versa).

### Connectors

Connectors are the way to connect OwlGuard to SIEMs. You can have as many connectors as you want, just to keep in mind, for now only Splunk is supported as this is the only one for which I was able to have a developer licence for now, will continue the development if this tool gets some interest. 

So how to register a connector? That's pretty simple, you will only need a few pieces of information, like the endpoint of your SIEM, API credentials, and that's pretty much it. Before the creation, OwlGuard will try to perform a test connection to ensure a proper setup. 

![Connector add](https://www.hato0.xyz/images/tools/OwlGuard/connector.png)

Once you have set your connector, you will be able to use the rule functionalities described above (especially the association and the status). You will also be able to switch from one connector to the other to only have the data for a specific connector (on the top right of the nav bar). The global view without being connector specific will always be available.  

The main page of the connector section will provide you with an overview of the connectors and switch colors depending on the state of the connector (light red=enable but not focus, green=enable and focus, red=disable).

![Connector dash](https://www.hato0.xyz/images/tools/OwlGuard/connectorPage.png)

### Documentation and Test scripts

I've merged those two topics because they are quite similar in terms of functionality for now. 
The documentation will provide you with a way to indicate to your analyst how to handle the rule and specify any action to perform for remediation. The Offensive Script section will provide you a way to manage your testing script for each rules and to ensure that you can test rules if needed. Both functionalities can be link to specific rules. The main dashboard provides an indication on which rules are not documented yet as long as some statistics. 

![Connector dash](https://www.hato0.xyz/images/tools/OwlGuard/mainDash.png)

One feature that I've in mind for the future to be integrated (again, if folks are interested in using the tool) is to give the possibility to launch test scripts and assess the results (detected or not) directly from OwlGuard. This would enable security teams to have a rapid checkup of their SIEM health or rule health. 

## Usage

The project's [README](https://github.com/Hato0/OwlGuard) should be pretty clear on how to run the project but anyway if you need help, feel free to reach me will be happy to show you around. 

## Conclusion

This tool was used in a real life exercise over two weeks and some bugs have been founds but all of them have been corrected since. Pretty happy with the result I had. 
OwlGuard reflect my vision of rule management that is not perfect but tried to create a tool accessible and usable by everyone to have a better management and overview of what's going on in their environment. 

Was a good way to deep dive into Django and to challenge a bit my coding/scripting abilities. Onto the next one!