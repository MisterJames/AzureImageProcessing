# AzureImageProcessing

#Application Details and Features

We need a witty application name for our app that runs on Azure. Perhaps... wittyapplicationonazure.com

## Features
*Sign up*

 - Twitter API 
 - you reserve your DNS name 

*Upload an image*
 - save as blob
 - "ingest" batch: oxford analysis 
 - automatically creates tags for images
 - create a tag for each category 
    - partition key = user name
    - row key = category
    - contents = all URLs in the user's category
    `[ {url:"", thumbnail:""}, {url:"", thumbnail:""}, ]`
 - add the image to each category
 - each image thumbnailed

#Topics
 1. Introduction (30 min)
 1. S Application and architecture walk-through (1 hour)
 1. J DNS (1 hour)
 1. S Batch (1 hour)
 1. J Azure storage (1 hour)
 1. S Elastic search (1 hour)
 1. J Autoscale (1 hour)
 1. Review and Summary, possibly questions? (45 min)
