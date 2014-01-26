TwitterStream [![Build status](https://ci.appveyor.com/api/projects/status?id=tmkgym7060pe2534)](https://ci.appveyor.com/project/twitterstream)
=============



C# console tool to save Twitter stream to files. 
Please add your token values to app.config file and run.

TwitterStream saves raw twits stream to sequence gzipped files ("yyyyMMdd_HHmmss.gz"). Each file stores 500,000 twits.

Application requires up to 1.5 Mbps bandwidth and generates about 2Gb data per day.

