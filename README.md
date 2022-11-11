### [Boundary detection using ArcGIS and ODM [Open Drone Map]](https://github.com/CR-Digital-Innovation/drone-land-survey/wiki/ArcGIS)

![GitHub all releases](https://img.shields.io/github/downloads/CR-Digital-Innovation/drone-land-survey/total)
![GitHub language count](https://img.shields.io/github/languages/count/CR-Digital-Innovation/drone-land-survey)
![GitHub top language](https://img.shields.io/github/languages/top/CR-Digital-Innovation/drone-land-survey?color=yellow)
![GitHub forks](https://img.shields.io/github/forks/CR-Digital-Innovation/drone-land-survey?style=social)
![GitHub Repo stars](https://img.shields.io/github/stars/CR-Digital-Innovation/drone-land-survey?style=social)

![image](https://user-images.githubusercontent.com/112068881/201333870-2a226c4c-adf6-4cfb-b8b9-edbf6aba9823.png)

CriticalRiver's process flow of implementing Land Survey using Drones

![image](https://user-images.githubusercontent.com/112068881/201337338-23421099-e2e8-4586-a7db-6764be55e0d8.png)

The entire workflow of the application is classified into the following functional areas
1. Vendor Registration
2. Drone Image Synchronization
3. Drone Survey Details Amendment and Orthomosaic Initialization 
4. Orthomosaic Image Generation 
5. Orthomosaic Quality Check and Boundary Generation
6. Notification  
7. Boundary Verification System
8. Dashboards and Reports


![image](https://user-images.githubusercontent.com/112068881/201334152-0a49a9e9-a504-4c41-b29f-16c72fa40ea4.png)

The below diagram deptics the functional flow of how the captured images from drone are getting stored on cloud containers and then these images are procssed and convered into Orthomosic image using our AI capablities. These are further mapped to respective owners property. The application also has the capablity of manually editting the the defined boundaries using WebGIS (ESRI) custome application built on Mobile & WebApps. The entire process has various notifications sent  to respective stakeholders at every stage.

![image](https://user-images.githubusercontent.com/112068881/201334198-fd208786-ba2d-48c3-82b8-acdeb65c18fd.png)

              The above dashboard is showing the various live stages of processing across various districts

![image](https://user-images.githubusercontent.com/112068881/201334228-286eb471-fdcf-435e-8b10-f2c36be7c55c.png)
              The above dashboard is a drilldown report of processed images by each vendors
