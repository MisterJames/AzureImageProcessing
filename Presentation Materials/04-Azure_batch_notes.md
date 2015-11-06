Used to handle very large, easily divisible jobs

job scheduling as a service
compute intensive applications at scale
100, 1000s or 10 000s of vms

examples
-media transcoding (media services on azure actually makes use of batch under the covers)
-rendering
-image analysis and processing
-builds
-test
-engineering calculations like simulations or fluid dynamics

Could always be done using pure VMs but to do so requires quite a lot of plumbing code. It seems easy at first but there are a lot of corner cases

batches by region

batch explorer

When to use batch vs. when to use some other distribution mechanism like service bus
When should you create a new batch pool and when should use reuse an existing one?
- create a pool for each set of tasks that need to run totally independently. So you're likely to want a pool for "the accouting application" rather than a pool for "month end processing" or "march's month end processing"

A pool contains jobs and each job contains one or more tasks. A pool also contains nodes each of which is an instance of a virtual machine. When a pool stats up it builds the virtual machine from the 

os family when creating pool https://azure.microsoft.com/en-us/documentation/articles/cloud-services-guestos-update-matrix/\]
you can, and really should, specify "*" when 
nodes can be interrupted during processing so tasks should be indempotent or at least side effect free until the end
In general we want to use the latest OS version when we start and then update as we see fit

Job preparation tasks run before any task. Runs on every single node. Equally the release task does the same thing. Examples of setup tasks might include uploading large data sets or installing specialized tools. The release tasks might clean up this data. 

autoscale can be a formula so if your system makes use of a particular resource heavily you can monitor that and trigger scalling. For insance say your task are heavily IO based then you could check that

failure recovery

schedule batches so they run automatically every once in a while. You could put in a task to run every month

cost only the cost of the resouces no additional costs